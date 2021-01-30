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
        //public HttpClient modernHttpClient;
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

        public async Task<BaseResponse<LoginResponse>> PostLoginCall(CredModel request)
        {
            try
            {
                var response = await HttpPOSTCall(Config.LoginUrl, request);
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

        public async Task<long> TestCompression()
        {
            SetUp();
            compress();
            var data = new List<String>() { "I dont eat", "I dont Care", "I love Megha", "I love madrid", "NO NO NO", "ON ON NO" };
            var buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            await App.Current.MainPage.DisplayAlert("NON COMPRESSED","The Length is : " + buffer.Length.ToString(),"OK");
            httpClient.DefaultRequestHeaders.Remove("Accept-Encoding");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            var jsonContent = new JsonContent(data);
            var r = await httpClient.PostAsync(Config.LoginUrl, jsonContent);
            return 0;
        }

        private async void compress()
        {
            var data = new List<String>() { "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum." };
            BinaryFormatter bf = new BinaryFormatter();
            byte[] d;
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, data);
                d = ms.ToArray();
            }
            await App.Current.MainPage.DisplayAlert("Un comp Length", d.Length.ToString(), "ok");
            var outputStream = new MemoryStream();
            using (var gZipStream = new GZipStream(outputStream, CompressionLevel.Fastest, false)) 
            {
                await gZipStream.WriteAsync(d, 0, d.Length);
            }

            await App.Current.MainPage.DisplayAlert("RANDOM", BitConverter.ToString(outputStream.ToArray()), "ok");
            await App.Current.MainPage.DisplayAlert("RANDOM", outputStream.ToArray().Length.ToString(), "ok");
        }

        private void decompress()
        {

        }

        private async Task<HttpResponseMessage> HttpPOSTCall(string url, Object data)
        {
            HttpClient client = new HttpClient();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
            var response = await client.PostAsync(url, byteContent);
            return response;
        }

        public void SetUp()
        {
            var httpHandler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
            };
            httpClient = new HttpClient(httpHandler);
            //modernHttpClient = new HttpClient(new ModernHttpClient.NativeMessageHandler());
        }
    }

    public class JsonContent : HttpContent
    {
        private JsonSerializer serializer { get; }
        private object value { get; }
        public JsonContent(object value)
        {
            this.serializer = new JsonSerializer();
            this.value = value;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
            Headers.ContentEncoding.Add("gzip");
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var gzip = new GZipStream(stream, CompressionMode.Compress, true))
                using (var writer = new StreamWriter(gzip))
                {
                    serializer.Serialize(writer, value);
                }
            });
        }
    }
}
