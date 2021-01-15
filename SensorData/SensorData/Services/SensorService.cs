using System;
using System.Collections.Generic;
using SensorData.Models;
using Shiny;
using Shiny.Sensors;

namespace SensorData.Services
{
    public class SensorService : ISensorService
    {
        ICompass Compass;
        IAccelerometer Accelerometer;
        IGyroscope GyroScope;
        IProximity Proximity;
        IHeartRateMonitor HeartRate;
        List<IDisposable> availableSensors;
        Dictionary<long, CompassReading> compass;
        Dictionary<long, MotionReading> accelerometer;
        Dictionary<long, MotionReading> gyroscope;
        Dictionary<long, bool> proximity;
        Dictionary<long, ushort> heartRate;
        bool Capturing;

        public SensorService()
        {
            ResolveAllSensors();
            Capturing = false;
            availableSensors = new List<IDisposable>();
            compass = new Dictionary<long, CompassReading>();
            accelerometer = new Dictionary<long, MotionReading>();
            gyroscope = new Dictionary<long, MotionReading>();
            proximity = new Dictionary<long, bool>();
            heartRate = new Dictionary<long, ushort>();
        }

        public MasterDataModel DisposeAll()
        {
            if (!Capturing)
                return null;
            Capturing = false;
            foreach (IDisposable disposable in availableSensors)
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            Capturing = false;

            return new MasterDataModel()
            {
                CompassData = compass,
                AccelerometerData = accelerometer,
                GyroscopeData = gyroscope,
                HeartRateData = heartRate,
                ProximityData = proximity
            };
        }

        public void FlushData()
        {
            compass.Clear();
            gyroscope.Clear();
            accelerometer.Clear();
            proximity.Clear();
            heartRate.Clear();
        }

        private void ResolveAllSensors()
        {
            Compass = ShinyHost.Resolve<ICompass>();
            Accelerometer = ShinyHost.Resolve<IAccelerometer>();
            GyroScope = ShinyHost.Resolve<IGyroscope>();
            Proximity = ShinyHost.Resolve<IProximity>();
            HeartRate = ShinyHost.Resolve<IHeartRateMonitor>();
        }

        public void StartCapture()
        {
            if (Capturing)
                return;

            Capturing = true;

            if (Compass != null)
            {
                availableSensors.Add(Compass.WhenReadingTaken().Subscribe(c =>
                {
                    compass.Add(DateTime.UtcNow.Ticks, c);
                }));
            }

            if (Accelerometer != null)
            {
                availableSensors.Add(Accelerometer.WhenReadingTaken().Subscribe(a =>
                {
                    accelerometer.Add(DateTime.UtcNow.Ticks, a);
                }));
            }

            if (GyroScope != null)
            {
                availableSensors.Add(GyroScope.WhenReadingTaken().Subscribe(g =>
                {
                    gyroscope.Add(DateTime.UtcNow.Ticks, g);
                }));
            }

            if (Proximity != null)
            {
                availableSensors.Add(Proximity.WhenReadingTaken().Subscribe(p =>
                {
                    proximity.Add(DateTime.UtcNow.Ticks, p);
                }));
            }

            if (HeartRate != null)
            {
                availableSensors.Add(HeartRate.WhenReadingTaken().Subscribe(h =>
                {
                    heartRate.Add(DateTime.UtcNow.Millisecond, h);
                }));
            }
        }
    }
}
