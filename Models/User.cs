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

    internal class User : ModelBase
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
    }
}