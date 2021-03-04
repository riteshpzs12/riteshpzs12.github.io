using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SensorData.Models;

namespace SensorData.Services
{
    public class WebHelper : IWebHelper
    {
        public HttpClient httpClient;
        private ICache cache;

        /// <summary>
        /// Constructor
        /// </summary>
        public WebHelper(ICache _cache)
        {
            cache = _cache;
        }
        /// <summary>
        /// The Login process, TODO update the login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<LoginResponse>> PostLoginCall(CredModel request)
        {
            try
            {
                Dictionary<string, string> header = new Dictionary<string, string>();
                var token = await Xamarin.Essentials.SecureStorage.GetAsync(Config.InstallationToken);
                header.Add("InstallToken", token);
                var response = await HttpPOSTCall(Config.LoginUrl, request, header);
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

        /// <summary>
        /// Send The sensor data to the backend
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="sensorTypeEnum"></param>
        /// <returns></returns>
        public async Task<bool> SendSensorData<T>(Dictionary<long, T> data, SensorTypeEnum sensorTypeEnum)
        {
            SetUp();
            var d = await CompressDataAsync(data);
            httpClient.DefaultRequestHeaders.Remove("Accept-Encoding");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            httpClient.DefaultRequestHeaders.Add("DeviceId", App.DeviceId);
            var sessionId = cache.Get<string>(Config.SessionDataKey);
            httpClient.DefaultRequestHeaders.Add("SessionId", sessionId);
            var byteContent = new ByteArrayContent(d.ToArray());
            var t = d.ToArray().Length;

            //Commented just for development purpose
            var response = await httpClient.PostAsync(Config.DataPushUrl + sensorTypeEnum.ToString(), byteContent);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        private async Task<HttpResponseMessage> HttpPOSTCall(string url, Object data, Dictionary<string, string> header = null)
        {
            HttpClient client = new HttpClient();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
            if (header != null)
                if (header.Count > 0)
                    foreach (KeyValuePair<string, string> k in header)
                        client.DefaultRequestHeaders.Add(k.Key, k.Value);
            var response = await client.PostAsync(url, byteContent);
            return response;
        }

        private void SetUp()
        {
            var httpHandler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
            httpClient = new HttpClient(httpHandler);
        }

        private async Task<MemoryStream> CompressDataAsync<T>(Dictionary<long, T> data)
        {
            int len = 0;
            string details = JsonConvert.SerializeObject(data);
            BinaryFormatter bf = new BinaryFormatter();
            byte[] d;
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, details);
                d = ms.ToArray();
                len = d.Length;
            }
            var outputStream = new MemoryStream();
            using (var gZipStream = new GZipStream(outputStream, CompressionLevel.Optimal, false))
            {
                await gZipStream.WriteAsync(d, 0, d.Length);
            }

            return outputStream;
        }

        public async Task<BaseResponse<RegistrationResponse>> PostRegister(RegisterModel register)
        {
            try
            {
                var response = await HttpPOSTCall(Config.RegisterUrl, register);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return new BaseResponse<RegistrationResponse>.Success
                    {
                        data = JsonConvert.DeserializeObject<RegistrationResponse>(result)
                    };
                }
                return JsonConvert.DeserializeObject<BaseResponse<RegistrationResponse>.Error>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                return new BaseResponse<RegistrationResponse>.Error
                {
                    statusCode = System.Net.HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }
        }
    }

    /// <summary>
    /// Not required class as of now cause the basic compression class is working
    /// </summary>
    //public class JsonContent : HttpContent
    //{
    //    private JsonSerializer serializer { get; }
    //    private object value { get; }
    //    public JsonContent(object value)
    //    {
    //        this.serializer = new JsonSerializer();
    //        this.value = value;
    //        Headers.ContentType = new MediaTypeHeaderValue("application/json");
    //        Headers.ContentEncoding.Add("gzip");
    //    }

    //    protected override bool TryComputeLength(out long length)
    //    {
    //        length = -1;
    //        return false;
    //    }

    //    protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
    //    {
    //        return Task.Factory.StartNew(() =>
    //        {
    //            using (var gzip = new GZipStream(stream, CompressionMode.Compress, true))
    //            using (var writer = new StreamWriter(gzip))
    //            {
    //                serializer.Serialize(writer, value);
    //            }
    //        });
    //    }
    //}
}
