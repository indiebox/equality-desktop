using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Catel.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Equality.Models
{
    public class ValidationError : Exception
    {
        public readonly int StatusCode;
        public readonly JObject Errors;
        public ValidationError(int statusCode, string statusText) : base(statusText)
        {
            StatusCode = statusCode;
            JObject statusTextJson = JObject.Parse(statusText);
            Errors = statusTextJson["errors"] as JObject;
        }
    }
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
            string strParams = JsonConvert.SerializeObject(@params);
            Debug.Print(strParams);
            HttpContent content = new StringContent(strParams, Encoding.UTF8, accept);

            HttpResponseMessage response = await client.PostAsync(url, content);
            result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) {
                return result;
            } else if ((int)response.StatusCode == 422) {
                throw new ValidationError((int)response.StatusCode, result);
            } else {
                throw new Exception(result);
            }

        }

        public static async Task<string> Login(string email, string password, string deviceName)
        {
            Dictionary<string, string> Params = new Dictionary<string, string>();
            Params.Add("email", email);
            Params.Add("password", password);
            Params.Add("device_name", deviceName);
            return await Request("http://equality/api/v1/login", Params, "application/json");
        }
    }
}