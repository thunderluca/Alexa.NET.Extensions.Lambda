using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace Alexa.NET.Lambda.Serialization
{
    public class Serializer : ILambdaSerializer
    {
        private readonly JsonSerializer InternalSerializer;

        public Serializer()
        {
            this.InternalSerializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            this.InternalSerializer.Converters.Add(new StringEnumConverter
            {
                AllowIntegerValues = false
            });
        }

        public void Serialize<T>(T response, Stream responseStream)
        {
            var writer = new StreamWriter(responseStream);
            this.InternalSerializer.Serialize(writer, response);
            writer.Flush();
        }

        public T Deserialize<T>(Stream requestStream)
        {
            using (var reader = new StreamReader(requestStream))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return this.InternalSerializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}
