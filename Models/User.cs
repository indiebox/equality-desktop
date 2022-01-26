using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
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

        private static async Task<string> Request(string url, string method, Dictionary<string, string> Params, string accept = "application/json")
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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.Accept = accept;
                NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
                byte[] byteArray = Encoding.UTF8.GetBytes(HttpUtility.UrlEncode("email=test@mail.ru password=123456 device_name=Desktop-XXX"));
                using (Stream dataStream = request.GetRequestStream()) {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                request.ContentLength = byteArray.Length;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string responseText;
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding)) {
                    responseText = reader.ReadToEnd();
                }
                response.Close();
                return responseText;

            } catch (WebException e) {
                string responseText;

                var encoding = ASCIIEncoding.ASCII;
                HttpWebResponse response = (HttpWebResponse)e.Response;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding)) {
                    responseText = reader.ReadToEnd();
                }
                return responseText;
            }
        }

        public static async Task<string> Login(string email, SecureString password, string deviceName)
        {
            Dictionary<string, string> Params = new Dictionary<string, string>();
            Params.Add("email", email);
            Params.Add("password", password.ToString());
            Params.Add("device_name", deviceName);
            return await Request("http://equality/api/v1/login", "POST", Params, "Accept:application/json");
        }
    }
}