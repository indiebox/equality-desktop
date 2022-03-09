using System;

using Catel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Equality.Services
{
    public interface IDeserializeModels<T>
        where T : new()
    {
        /// <summary>
        /// Deserializes the JSON string to the object.
        /// </summary>
        /// <param name="data">The JSON string.</param>
        /// <returns>The deserialized object.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public T Deserialize(string data)
        {
            Argument.IsNotNullOrWhitespace(nameof(data), data);

            return JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
            });
        }

        /// <summary>
        /// Deserializes the JSON string to the array of objects.
        /// </summary>
        /// <param name="data">The JSON string.</param>
        /// <returns>The deserialized array of objects.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public T[] DeserializeRange(string data)
        {
            Argument.IsNotNullOrWhitespace(nameof(data), data);

            return JsonConvert.DeserializeObject<T[]>(data, new JsonSerializerSettings
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
        public T Deserialize(JToken data)
        {
            Argument.IsNotNull(nameof(data), data);

            return data.ToObject<T>(new()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }

        /// <summary>
        /// Deserializes the <c>JToken</c> string to the array of objects.
        /// </summary>
        /// <param name="data">The JSON string.</param>
        /// <returns>The deserialized array of objects.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public T[] DeserializeRange(JToken data)
        {
            Argument.IsNotNull(nameof(data), data);

            return data.ToObject<T[]>(new()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }
    }
}
