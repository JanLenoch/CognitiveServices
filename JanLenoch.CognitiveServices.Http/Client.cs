using JanLenoch.CognitiveServices.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JanLenoch.CognitiveServices.Http
{
    public abstract class Client
    {
        protected const string CONTENT_TYPE_HEADER_IDENTIFIER = "Content-Type";

        protected readonly Dictionary<string, string> _queryStringValues = new Dictionary<string, string>();

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
                builder.Append($"={valueEncoded}");
                i++;

                if (i != _queryStringValues.Count)
                {
                    builder.Append("&");
                }
            }

            return _queryStringValues.Count > 0 ? builder.ToString() : string.Empty;
        }

        public async Task<TResponse> Post<TResponse>(string baseUri, HttpContent content, string apiKeyName = null, string apiKeyValue = null)
            where TResponse : Response
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, baseUri + ParseQueryString()))
                {
                    requestMessage.Content = content;

                    if (!string.IsNullOrEmpty(apiKeyName))
                    {
                        requestMessage.Headers.Add(apiKeyName, apiKeyValue);
                    }

                    string contentType = content.Headers?.ContentType?.ToString();

                    if (!string.IsNullOrEmpty(contentType))
                    {
                        // The endpoints require such explicit request header.
                        requestMessage.Headers.TryAddWithoutValidation("Content-Type", contentType);
                    }

                    using (HttpResponseMessage responseMessage = await client.SendAsync(requestMessage))
                    {
                        var response = Json.Deserialize<TResponse>(await responseMessage.Content.ReadAsStreamAsync());
                        response.StatusCode = responseMessage.StatusCode;
                        response.IsSuccessStatusCode = responseMessage.IsSuccessStatusCode;

                        return response;
                    }
                }
            }
        }
    }
}
