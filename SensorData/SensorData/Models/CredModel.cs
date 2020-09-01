using System;
using System.Net;
using Newtonsoft.Json;

namespace SensorData.Models
{
    public class CredModel : CustomeBaseRequest
    {
        public string userName { get; set; }
        public string passWord { get; set; }
        public string deviceId { get; set; }
    }

    public abstract class BaseResponse<T>
    {
        public sealed class Success : BaseResponse<T>
        {
            public T data { get; set; }
        }

        public sealed class Error : BaseResponse<T>
        {
            [JsonProperty("message")]
            public string message { get; set; }

            [JsonProperty("statusCode")]
            public HttpStatusCode statusCode { get; set; }
        }
    }

    public class LoginResponse : BaseResponse<LoginResponse>
    {
        [JsonProperty("userId")]
        public int UID { get;set; }
    }
}
