using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace JanLenoch.CognitiveServices.Common
{
    public abstract class Client : IDisposable
    {
        const string CONTENT_TYPE_HEADER_IDENTIFIER = "Content-Type";

        private readonly HttpClient _httpClient = null;
        private Dictionary<string, string> _queryStringValues = new Dictionary<string, string>();

        protected HttpClient HttpClient => _httpClient ?? new HttpClient();

        public Dictionary<string, string> QueryStringValues
        {
            get { return _queryStringValues; }
        }

        public string ParseQueryString()
        {
            StringBuilder builder = new StringBuilder("?");
            int i = 0;

            foreach (KeyValuePair<string, string> parameter in _queryStringValues)
            {
                string keyEncoded = System.Net.WebUtility.UrlEncode(parameter.Key);
                builder.Append(keyEncoded);
                string valueEncoded = System.Net.WebUtility.UrlEncode(parameter.Value);
                builder.Append(valueEncoded);
                i++;

                if (i != _queryStringValues.Count)
                {
                    builder.Append("&");
                }
            }

            return builder.ToString();
        }

        protected async Task<TResponse> GetResponse<TResponse>(string baseUri, ByteArrayContent content)
            where TResponse : class
        {
            using (HttpResponseMessage responseMessage = await HttpClient.PostAsync(baseUri + ParseQueryString(), content))
            {
                var serializer = new DataContractJsonSerializer(typeof(TResponse));
                var stream = await responseMessage.Content.ReadAsStreamAsync();

                return serializer.ReadObject(stream) as TResponse;
            }
        }

        protected void AddContentTypeHeader(string contentType)
        {
            HttpClient.DefaultRequestHeaders.Add(CONTENT_TYPE_HEADER_IDENTIFIER, contentType);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
