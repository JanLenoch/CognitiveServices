using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace JanLenoch.CognitiveServices.Utils
{
    public static class Json
    {
        public static async Task<string> SerializeAsync<TInput>(TInput input)
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TInput));
            serializer.WriteObject(stream, input);
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);

            return await sr.ReadToEndAsync();
        }

        public static TOutput Deserialize<TOutput>(Stream stream)
            where TOutput : class
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TOutput));
            
            return serializer.ReadObject(stream) as TOutput;
        }
    }
}
