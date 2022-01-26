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
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

using Catel.Data;

using Newtonsoft.Json;

namespace Equality.Models
{
    class User : ModelBase
    {
        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        private static async Task<string> Request(string url, Dictionary<string, string> @params, string accept = "application/json")
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            string result;
            try {
                string strParams = JsonConvert.SerializeObject(@params);
                Debug.Print(strParams);
                HttpContent content = new StringContent(strParams, Encoding.UTF8, accept);

                HttpResponseMessage response = await client.PostAsync(url, content);
                result = await response.Content.ReadAsStringAsync();

            } catch (HttpRequestException e) {
                result = e.Message.ToString();
            }
            return result;
        }

        public static async Task<string> Login(string email, SecureString password, string deviceName)
        {
            Dictionary<string, string> Params = new Dictionary<string, string>();
            Params.Add("email", email);
            Params.Add("password", password.ToString());
            Params.Add("device_name", deviceName);
            return await Request("http://equality/api/v1/login", Params, "application/json");
        }
    }
}