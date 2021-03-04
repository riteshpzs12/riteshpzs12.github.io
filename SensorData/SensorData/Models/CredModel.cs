using System;
using System.Net;
using Newtonsoft.Json;

namespace SensorData.Models
{
    public class CredModel : CustomeBaseRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string deviceId { get; set; }
    }

//  "Name": "Misti Sebyer",
//  "Contact": "msebyer0@nba.com",
//  "Password": "apBzd8",
//  "DeviceId": "706bf546-2665-40c7-99bb-edd804f16076",
//  "Model": "Devbug",
//  "Consent":"1111"

    public class RegisterModel
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceType { get; set; }
        public string Consent { get; set; }
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

        [JsonProperty("sessionId")]
        public string sessionId { get; set; }

        [JsonProperty("SessionStart")]
        public string CreatedOn { get; set; }

        [JsonProperty("CustomMessage")]
        public string Greet { get; set; }
    }

    public class RegistrationResponse : BaseResponse<RegistrationResponse>
    {
        [JsonProperty("userId")]
        public int UID { get; set; }

        [JsonProperty("InstallationToken")]
        public string InstallToken { get; set; }
    }
}
