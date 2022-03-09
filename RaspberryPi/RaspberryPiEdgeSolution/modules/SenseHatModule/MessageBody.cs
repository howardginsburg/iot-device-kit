// Copyright (c) Microsoft. All rights reserved.
namespace SenseHatModule
{
    using System;
    using System.Numerics;
    using Iot.Device.Common;
    using Iot.Device.SenseHat;
    using Newtonsoft.Json;

    class MessageBody
    {
        public MessageBody(SenseHat senseHat)
        {
            ReadHat(senseHat);
        }

        [JsonProperty(PropertyName = "joystickState")]
        public string JoystickState { get; set;}

        [JsonProperty(PropertyName = "temperature")]
        public double Temperature { get; set;}

        [JsonProperty(PropertyName = "pressure")]
        public double Pressure { get; set;}

        [JsonProperty(PropertyName = "altitude")]
        public double Altitude { get; set;}

        [JsonProperty(PropertyName = "acceleration")]
        public VectorBody Acceleration { get; set; }

        [JsonProperty(PropertyName = "angularRate")]
        public VectorBody AngularRate {get; set;}

        [JsonProperty(PropertyName = "magneticInduction")]
        public VectorBody MagneticInduction {get; set;}

        [JsonProperty(PropertyName = "humidity")]
        public double Humidity {get; set;}

        [JsonProperty(PropertyName = "heatIndex")]
        public double HeatIndex {get; set;}

        [JsonProperty(PropertyName = "dewPoint")]
        public double DewPoint {get; set;}

        [JsonProperty(PropertyName = "timeCreated")]
        public DateTime TimeCreated { get; set; }

        public class VectorBody
        {
            public VectorBody(double X, double Y, double Z)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
            }
            [JsonProperty(PropertyName = "x")]
            public double X { get; set; }

            [JsonProperty(PropertyName = "y")]
            public double Y { get; set; }

            [JsonProperty(PropertyName = "z")]
            public double Z { get; set; }
        }

        private void ReadHat(SenseHat senseHat)
        {
            Temperature = senseHat.Temperature.DegreesCelsius;
            Pressure = senseHat.Pressure.Kilopascals;
            Humidity = senseHat.Humidity.Percent;
            VectorBody accel = new VectorBody(senseHat.Acceleration.X,senseHat.Acceleration.Y, senseHat.Acceleration.Z);
            Acceleration = accel;
            //Acceleration = senseHat.Acceleration;
            VectorBody angular = new VectorBody(senseHat.AngularRate.X,senseHat.AngularRate.Y,senseHat.AngularRate.Z);
            AngularRate = angular;
            //AngularRate = senseHat.AngularRate;
            VectorBody magnetic = new VectorBody(senseHat.MagneticInduction.X,senseHat.MagneticInduction.Y,senseHat.MagneticInduction.Z);
            MagneticInduction = magnetic;
            //MagneticInduction = senseHat.MagneticInduction;
            Altitude = (WeatherHelper.CalculateAltitude(senseHat.Pressure, WeatherHelper.MeanSeaLevel, senseHat.Temperature)).Meters;
            HeatIndex = WeatherHelper.CalculateHeatIndex(senseHat.Temperature, senseHat.Humidity).DegreesCelsius;
            DewPoint = WeatherHelper.CalculateDewPoint(senseHat.Temperature, senseHat.Humidity).DegreesCelsius;

            senseHat.ReadJoystickState();

            if (senseHat.HoldingUp)
            {
                JoystickState = "Up";
            }
            else if (senseHat.HoldingDown)
            {
                JoystickState =  "Down";
            }
            else if (senseHat.HoldingLeft)
            {
                JoystickState =  "Left";
            }
            else if (senseHat.HoldingRight)
            {
                JoystickState =  "Right";
            }
            else if (senseHat.HoldingButton)
            {
                JoystickState =  "Pressed";
            }
            else
            {
                JoystickState =  "Center";
            }

            TimeCreated = DateTime.UtcNow;
        }

        
        
    }
}