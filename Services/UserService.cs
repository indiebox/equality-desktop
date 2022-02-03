using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Equality.Core.ApiClient;
using Equality.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Equality.Services
{
    public class UserService : IUserService
    {
        protected IApiClient ApiClient;

        public UserService(IApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<(User user, string token)> LoginAsync(string email, string password)
        {
            Dictionary<string, object> data = new()
            {
                { "email", email },
                { "password", password },
                { "device_name", GetDeviceName() },
            };

            var response = await ApiClient.PostAsync("login", data);

            var user = Deserialize(response.Content["data"].ToString());
            string token = response.Content["token"].ToString();

            return (user, token);
        }

        public User Deserialize(string data)
        {
            var user = JsonConvert.DeserializeObject<User>(data, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
            });

            return user;
        }

        protected string GetDeviceName()
        {
            return Environment.MachineName;
        }
    }
}
