using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Catel.Data;

namespace Equality.Models
{
    class User : ModelBase
    {

        //private static readonly HttpClient client = new HttpClient();

        //private static async Task Request()
        //{
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        //    client.DefaultRequestHeaders.Add("Accept", "application/json");
        //    var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");

        //    var msg = await stringTask;
        //    Console.Write(msg);
        //}

        private static async Task<int> Request(string url, string method, string data, string headers = "Accept:application/json")
        {
            try {
                //WebRequest request = WebRequest.Create(url);
                //request.Method = method;
                //request.ContentType = "application/json";
                //byte[] byteArray = Encoding.UTF8.GetBytes(HttpUtility.UrlEncode("email=test@mail.ru password=123456 device_name=Desktop-XXX"));
                //request.Headers.Add("Accept: application/json");
                //request.ContentLength = byteArray.Length;

                //using (Stream dataStream = request.GetRequestStream()) {
                //    dataStream.Write(byteArray, 0, byteArray.Length);
                //}
                //string responseResult;
                //WebResponse response = await request.GetResponseAsync();
                //using (Stream stream = response.GetResponseStream()) {
                //    using (StreamReader reader = new StreamReader(stream)) {
                //        responseResult = reader.ReadToEnd();
                //    }
                //}
                //response.Close();
                int statusCode;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Head;
                request.Accept = "application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                statusCode = (int)response.StatusCode;
                response.Close();
                return statusCode;

            } catch (WebException e) {
                return (int)((HttpWebResponse)e.Response).StatusCode;
            }
        }

        public static async Task<int> Login(string email, string password, string deviceName)
        {
            return await Request("http://equality/api/v1/login", "POST", string.Format("email={0} password={1} device_name={2}", email, password, deviceName), "Accept:application/json");
        }
    }
}