// Copyright (c) Microsoft. All rights reserved.
namespace SenseHatModule
{
    using System;
    using System.IO;
    using System.Net;
    using System.Drawing;
    using System.Runtime.Loader;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;
    using Newtonsoft.Json;
    using Iot.Device.Common;
    using Iot.Device.SenseHat;
    using UnitsNet;


    class Program
    {
        public static ModuleClient ioTHubModuleClient;
        public static SenseHat senseHat;

        public static int xPixel = 0, yPixel = 0;

        public static void Main(string[] args)
        {
            senseHat = new SenseHat();
            senseHat.Fill();

            Init().Wait();

            // Wait until the app unloads or is cancelled
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();

            SendEvents(cts).Wait();
            WhenCancelled(cts.Token).Wait();
        }

        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        static async Task Init()
        {
            senseHat.Fill(Color.Yellow);
            // set this to the current sea level pressure in the area for correct altitude readings
            var defaultSeaLevelPressure = WeatherHelper.MeanSeaLevel;
            

            MqttTransportSettings mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            ITransportSettings[] settings = { mqttSetting };

            // Open a connection to the Edge runtime
            ioTHubModuleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
            await ioTHubModuleClient.OpenAsync();

            senseHat.Fill(Color.Green);

            Console.WriteLine("SenseHat module initialized!");
        }

        static async Task SendEvents(CancellationTokenSource cts)
        {
            var defaultSeaLevelPressure = WeatherHelper.MeanSeaLevel;

            while (!cts.Token.IsCancellationRequested)
            {
                //Clear the sensehat leds.
                senseHat.Fill();
                //Set the next pixel to green.
                senseHat.SetPixel(xPixel,yPixel,Color.Green);

                //Increment the pixel positions for the next time.
                xPixel++;
                if (xPixel == 8) //We're at the end of the X axis.
                {
                    xPixel = 0; //Reset the X position to 0.
                    yPixel++; //Increment the y position.
                    if (yPixel == 8) //We're at the bottom of the Y axis.
                    {
                        yPixel = 0;
                    }
                }

                var message = new MessageBody(senseHat);
                
                string dataBuffer = JsonConvert.SerializeObject(message);
                var eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer));
                eventMessage.ContentEncoding = "utf-8";
                eventMessage.ContentType = "application/json";
                //eventMessage.Properties.Add("sequenceNumber", count.ToString());
                //eventMessage.Properties.Add("batchId", BatchId.ToString());
                Console.WriteLine($"\t{DateTime.Now.ToLocalTime()}> Body: [{dataBuffer}]");
                await ioTHubModuleClient.SendEventAsync("senseHatOutput", eventMessage);
                

                await Task.Delay(1000, cts.Token);
            }

            senseHat.Fill();
        }
    }
}