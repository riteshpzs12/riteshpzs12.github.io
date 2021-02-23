using System.Collections.Generic;

namespace SensorData.Models
{
    public class SensorDataDisplayModel
    {
        public string SensorName { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string AdditionalField { get; set; }
    }

    public class MasterDataModel
    {
        public string SessionId { get; set; }
        public Dictionary<long, Xamarin.Essentials.CompassData> CompassData { get; set; }
        public Dictionary<long, Xamarin.Essentials.AccelerometerData> AccelerometerData { get; set; }
        public Dictionary<long, Xamarin.Essentials.GyroscopeData> GyroscopeData { get; set; }
        public Dictionary<long, Xamarin.Essentials.OrientationSensorData> OrientationSensorData { get; set; }
        public Dictionary<long, Xamarin.Essentials.MagnetometerData> MagnetometerData { get; set; }
    }

    public class SensorDataResponse : BaseResponse<SensorDataResponse>
    {
        //[JsonProperty("userId")]
        //public int UID { get; set; }
    }
}
