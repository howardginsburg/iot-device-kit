using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;

namespace MeadowApplication
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        
              

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            IAnalogInputPort analogIn = Device.CreateAnalogInputPort(Device.Pins.A00);

            while (true)
            {
                Voltage volt = analogIn.Read().Result;
                Console.WriteLine("Current Value: " + volt.Volts);

                Thread.Sleep(1000);
            }
            

        }
    }
}