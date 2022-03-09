//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Device.Gpio;
using System;
using System.Threading;
using Iot.Device.Buzzer;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Networking;
using nanoFramework.Azure.Devices.Provisioning.Client;
using nanoFramework.Azure.Devices.Client;
using System.Security.Cryptography.X509Certificates;

namespace Feather
{
    public class Program
    {
        //Pins for the Feather.
        GpioController _GpioController;
        

        //GPIO.
        GpioPin led;
        GpioPin redLed;
        GpioPin greenLed;
        GpioPin tilt;
        Buzzer buzzer;

        //Azure device client.
        DeviceClient device;

        //Store the current state of the alarm.
        bool ALARM_ENABLED = true;

        //Time of last telemetry send.
        DateTime lastTelemetrySendTime = DateTime.MinValue;

        //Time of alarm disabled.
        DateTime alarmDisabledTime = DateTime.MinValue;

        public static void Main()
        {
            Program program = new Program();
            program.Run();

        }

        public void Run()
        {
            //Initialize GPIO.
            InitializeGPIO();

            //Connect to wifi.
            ConnectWifi();

            //Connect the device to Azure.
            ConnectDevice();

            //Enable the alarm.
            EnableAlarm();

            int counter = 1;

            //Infinite loop...
            while (true)
            {
                //Get the current time.
                DateTime currentTime = DateTime.UtcNow;

                //Read the value of the tilt sensor.
                PinValue tiltValue = tilt.Read();

 
                //If the current time is greater than the last time we sent telemetry by the frequency of sends (ex. 5 seconds), send the latest status.
                if (currentTime.AddSeconds(-Configuration.TELEMETRY_FREQUENCY_SECS) > lastTelemetrySendTime)
                {
                    Console.WriteLine("Sending telemetry...");
                    
                    //Update the lastTelemetrySend time.
                    lastTelemetrySendTime = currentTime;


                    Console.WriteLine($"Sending heartbeat: {counter}");

                    string message = "{\"LidState\":\"closed\"}";
                         
                    if (tiltValue.Equals(PinValue.High))
                    {
                        message = "{\"LidState\":\"open\"}";
                    }
                    //Send the telemetry to azure.
                    var isReceived = device.SendMessage(message);


                    counter++;
                }

                

                //If the sensor is high, ie, the box is opened, and our alarm is enabled, sound the alarm!
                if (tiltValue.Equals(PinValue.High) && ALARM_ENABLED)
                {   
                    buzzer.PlayTone(440, 1000); // Play tone with frequency 440 hertz for one second.
                }
                //If the sensor is low, ie the box is closed, alarm is disabled, and it's been more than 10 seconds since we disabled the
                //alarm, turn it back on.  The 10 seconds is there so that when the alarm gets disabled, there is a 10 second buffer before
                //it gets renabled, allowing the person to open the box.
                else if (tiltValue.Equals(PinValue.Low) && !ALARM_ENABLED && currentTime.AddSeconds(-10) > alarmDisabledTime)
                {
                    EnableAlarm();
                }



            }
        }

        private void InitializeGPIO()
        {
            Console.WriteLine("Initializing GPIO...");
            //Create an instance of the Gpio controller.
            _GpioController = new GpioController();

            //Set our pins for the leds.
            led = _GpioController.OpenPin(Configuration.BUILT_IN_LED_PIN, PinMode.Output);
            led.Write(PinValue.Low); //Make sure the built in LED is turned off.
            redLed = _GpioController.OpenPin(Configuration.RED_LED_PIN, PinMode.Output);
            greenLed = _GpioController.OpenPin(Configuration.GREEN_LED_PIN, PinMode.Output);
            tilt = _GpioController.OpenPin(Configuration.TILT_PIN, PinMode.Input);

            //Set the buzzer pin to use pulse width modulation channel 1. and create an instance of the buzzer.
            nanoFramework.Hardware.Esp32.Configuration.SetPinFunction(Configuration.BUZZER_PIN, DeviceFunction.PWM1);
            buzzer = new Buzzer(Configuration.BUZZER_PIN);
            buzzer.StopPlaying();

            Console.WriteLine("GPIO initialized!");
        }

        private void ConnectWifi()
        {
            Console.WriteLine("Connecting to Wifi...");
            // Give 60 seconds to the wifi join to happen
            CancellationTokenSource cs = new(60000);
            var success = WiFiNetworkHelper.ConnectDhcp(Configuration.WIFI_SSID, Configuration.WIFI_PASSWORD, requiresDateTime: true, token: cs.Token);
            if (!success)
            {
                // Something went wrong, you can get details with the ConnectionError property:
                Console.WriteLine($"Can't connect to the network, error: {WiFiNetworkHelper.Status}");
                if (WiFiNetworkHelper.HelperException != null)
                {
                    Console.WriteLine($"ex: {WiFiNetworkHelper.HelperException}");
                }
                return;
            }
            else
            {
                //Turn on the built in LED so we know wifi is connected.
                led.Write(PinValue.High);
                Console.WriteLine("Wifi connected!");
            }
        }

        private void ConnectDevice()
        {
            Console.WriteLine("Connecting Device...");
            
            Console.WriteLine("Registering device with DPS.");
            var provisioning = ProvisioningDeviceClient.Create(Configuration.DPS_ADDRESS, Configuration.DPS_SCOPE, Configuration.DPS_REGISTRATION_ID, Configuration.DPS_SAS_KEY, new X509Certificate(Configuration.AZURE_ROOT_CA));
            var myDevice = provisioning.Register(new CancellationTokenSource(60000).Token);

            if (myDevice.Status != ProvisioningRegistrationStatusType.Assigned)
            {
                Console.WriteLine($"Registration is not assigned: {myDevice.Status}, error message: {myDevice.ErrorMessage}");
                return;
            }

            Console.WriteLine("Connecting device to iot hub.");
            // You can then create the device
            device = new DeviceClient(myDevice.AssignedHub, myDevice.DeviceId, Configuration.DPS_SAS_KEY, azureCert: new X509Certificate(Configuration.AZURE_ROOT_CA) );//nanoFramework.M2Mqtt.Messages.MqttQoSLevel.AtMostOnce

            Console.WriteLine("Adding device callback method.");
            //Add the device callback to disable the alarm.
            device.AddMethodCallback(DisableAlarm);

            Console.WriteLine("Connecting...");
            // Open it and continue like for the previous sections
            var res = device.Open();
            if (!res)
            {
                Console.WriteLine($"can't open the device");
            }
            Console.WriteLine("Connected!");
        }

        private string DisableAlarm(int rid, string payload)
        { 
            //This is the callback method Azure.
            Console.WriteLine($"DisableAlarm called: rid={rid}, payload={payload}");

            //Store the alarm status.
            ALARM_ENABLED = false;

            //Set the light to green.
            redLed.Write(0);
            greenLed.Write(255);

            //Set the alarm disabled time.  We use this to give the person X seconds to open the box before it gets automatically enabled.
            alarmDisabledTime = DateTime.UtcNow;

            //Return the result of the method call back to azure.
            return "{\"Status\":\"Disabled\"}";
        }

        private void EnableAlarm()
        {
            Console.WriteLine("Alarm enabled!");

            //Store the alarm status.
            ALARM_ENABLED = true;

            //Set the light to red.
            redLed.Write(255);
            greenLed.Write(0);

            //Reset the alarm disable time.
            alarmDisabledTime = DateTime.MinValue;
        }
    }
}
