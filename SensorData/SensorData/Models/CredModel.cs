using System;
namespace SensorData.Models
{
    public class CredModel : CustomeBaseRequest
    {
        public string userName { get; set; }
        public string passWord { get; set; }
        public string deviceId { get; set; }
    }
}
