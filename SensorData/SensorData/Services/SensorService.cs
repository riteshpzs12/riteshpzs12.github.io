using System.Collections.Generic;
using SensorData.Models;
using SensorData.ShinySensor.Sensors_XamEssential;

//Will deprecate this as shinysensor will be removed
namespace SensorData.Services
{
    public class SensorService : ISensorService
    {
        public List<ISenseors> senseors;
        private bool Capturing;
        public SensorService()
        {
            ResolveAllSensors();
            Capturing = false;
        }

        /// <summary>
        /// Disposes the Sensor instances
        /// </summary>
        /// <returns>MasterDataModel</returns>
        public void DisposeAll()
        {
            if (!Capturing)
                return;
            foreach (ISenseors senseors in senseors)
            {
                senseors.ControlSunscribe(false);
            }
            Capturing = false;
        }

        /// <summary>
        /// Clears the cpatured data of the dictionaries
        /// </summary>
        public void FlushData()
        {
            foreach (ISenseors senseors in senseors)
            {
                senseors.Dispose();
            }
        }

        /// <summary>
        /// Returns the Data captured by the sensors.
        /// </summary>
        /// <returns></returns>
        public MasterDataModel GetData()
        {
            MasterDataModel masterDataModel = new MasterDataModel();
            masterDataModel.AccelerometerData = ((AccelerometerCapture)senseors[0]).AccelerometerDataReading;
            masterDataModel.GyroscopeData = ((GyroscopeCapture)senseors[1]).GyroscopeDataReading;
            masterDataModel.OrientationSensorData = ((OrientationCapture)senseors[2]).OrientationDataReading;
            masterDataModel.CompassData = ((CompassCapture)senseors[3]).CompassDataReading;
            masterDataModel.MagnetometerData = ((MagnetometerCapture)senseors[4]).MagnetometerDataReading;
            return masterDataModel;
        }

        /// <summary>
        /// Creates each sensors instance, ready to capure data
        /// </summary>
        private void ResolveAllSensors()
        {
            senseors = new List<ISenseors>();
            senseors.Add(new AccelerometerCapture());
            senseors.Add(new GyroscopeCapture());
            senseors.Add(new OrientationCapture());
            senseors.Add(new CompassCapture());
            senseors.Add(new MagnetometerCapture());
        }

        /// <summary>
        /// Captures the sensor reading into the dictionaries
        /// </summary>
        public void StartCapture()
        {
            if (Capturing)
                return;

            Capturing = true;
            foreach(ISenseors senseors in senseors)
            {
                senseors.ControlSunscribe(true);
            }
        }
    }
}
