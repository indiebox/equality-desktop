using System;

using Catel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Equality.Data
{
    public static class Json
    {
        /// <summary>
        /// Deserializes the JSON string to the object.
        /// </summary>
        /// <param name="data">The JSON string.</param>
        /// <returns>The deserialized object.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public static TModel Deserialize<TModel>(string data)
        {
            Argument.IsNotNullOrWhitespace(nameof(data), data);

            return JsonConvert.DeserializeObject<TModel>(data, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
            });
        }

        /// <summary>
        /// Deserializes the <c>JToken</c> to the object.
        /// </summary>
        /// <param name="data">The JToken.</param>
        /// <returns>The deserialized object.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public static TModel Deserialize<TModel>(JToken data)
        {
            Argument.IsNotNull(nameof(data), data);

            return data.ToObject<TModel>(new()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }

        /// <summary>
        /// Serializes the <c>object</c> to the string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The serialized string.</returns>
        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }
    }
}
