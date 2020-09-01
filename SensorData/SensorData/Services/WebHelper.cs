using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SensorData.Models;

namespace SensorData.Services
{
    public class WebHelper : IWebHelper
    {
        public WebHelper()
        {
        }

        //public async Task<bool> GetCall(CustomeBaseRequest data)
        //{
        //    try
        //    {
        //        HttpClient client = new HttpClient();

        //        var uri = new Uri("http://ec2-3-94-134-168.compute-1.amazonaws.com:8080/unlock-sensor");
        //        TestBody testBody = new TestBody()
        //        {
        //            gyroscope = "Gyro sensor data",
        //            accelerometer = "Accelerometer sensor data"
        //        };
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testBody));
        //        //var buffer = System.Text.Encoding.UTF8.GetBytes(Compass.Text+"///"+Gyro.Text+"///"+Accel.Text+"///"+Proximity.Text+"////");
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");

        //        var response = await client.PostAsync(uri, byteContent);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = response.Content.ReadAsStringAsync().Result;
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public async Task<BaseResponse<LoginResponse>> PostCall(CustomeBaseRequest request)
        {
            try
            {
                HttpClient client = new HttpClient();
                CredModel c = (CredModel)request;
                var buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(c));
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var response = await client.PostAsync(Config.LoginUrl, byteContent);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return new BaseResponse<LoginResponse>.Success
                    {
                        data = JsonConvert.DeserializeObject<LoginResponse>(result)
                    };
                }
                return JsonConvert.DeserializeObject<BaseResponse<LoginResponse>.Error>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                return new BaseResponse<LoginResponse>.Error
                {
                    statusCode = System.Net.HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }
        }
    }
}
