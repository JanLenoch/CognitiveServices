using JanLenoch.CognitiveServices.Http;
using JanLenoch.CognitiveServices.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace JanLenoch.CognitiveServices.Ocr
{
    public partial class OcrClient : Client
    {
        public const string SUBSCRIPTION_KEY_NAME = "Ocp-Apim-Subscription-Key";
        protected const string URI_STUB = "api.cognitive.microsoft.com/vision/v1.0/ocr";

        protected readonly OcrRegions _region;
        protected readonly string _subscriptionKey;

        #region "Constructors"

        public OcrClient(string subscriptionKey, OcrRegions region)
        {
            _subscriptionKey = subscriptionKey;

            if (region != OcrRegions.NotSet)
            {
                _region = region;
            }
            else
            {
                throw GetRegionException();
            }
        }

        #endregion

        #region "Public methods"

        /// <summary>
        /// Recognizes text in an image that's available via a public URI.
        /// </summary>
        /// <param name="imageUri">A <see cref="Uri"/> of the image</param>
        /// <param name="imageFormat">A <see cref="OcrImageFormats"/> format</param>
        /// <param name="language">A <see cref="OcrLanguages"/> language</param>
        /// <param name="detectOrientation">Explicitly requests to detect image orientation</param>
        /// <returns>The <see cref="OcrResponse"/> with the structured text</returns>
        public async Task<OcrResponse> RecognizeAsync(Uri imageUri, OcrImageFormats imageFormat = OcrImageFormats.NotSet, OcrLanguages language = OcrLanguages.NotSet, bool? detectOrientation = null)
        {
            PrepareQueryString(language, detectOrientation);

            var urlContent = new UrlContent
            {
                Url = imageUri.AbsoluteUri
            };

            using (var content = new StringContent(await Json.SerializeAsync(urlContent)))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                return await GetResponse(content);
            }
        }

        /// <summary>
        /// Recognizes text in a <see cref="byte[]"/> array of an image.
        /// </summary>
        /// <param name="imageBytes">A <see cref="byte[]"/> of an image</param>
        /// <param name="language">A <see cref="OcrLanguages"/> language</param>
        /// <param name="detectOrientation">Explicitly requests to detect image orientation</param>
        /// <returns>The <see cref="OcrResponse"/> with the structured text</returns>
        public async Task<OcrResponse> RecognizeAsync(byte[] imageBytes, OcrLanguages language = OcrLanguages.NotSet, bool? detectOrientation = null)
        {
            PrepareQueryString(language, detectOrientation);

            using (var content = new ByteArrayContent(imageBytes))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                return await GetResponse(content);
            }
        }

        /// <summary>
        /// Gets the <see cref="Uri"/> of a select <see cref="OcrRegions"/> region.
        /// </summary>
        /// <param name="region">A <see cref="OcrRegions"/> region</param>
        /// <returns>The base <see cref="Uri"/> of the region</returns>
        public Uri GetRegionalUri(OcrRegions region)
        {
            if (region != OcrRegions.NotSet)
            {
                return new Uri($"https://{region.ToString().ToLower()}.{URI_STUB}");
            }
            else
            {
                throw GetRegionException();
            }
        }

        #endregion

        #region "Protected methods"

        protected virtual void PrepareQueryString(OcrLanguages language, bool? detectOrientation = null)
        {
            if (language != OcrLanguages.NotSet)
            {
                QueryStringValues.Add("language", Globalization.Languages.FirstOrDefault((KeyValuePair<string, string> l) => l.Value == language.ToString()).Key);
            }

            if (detectOrientation.HasValue)
            {
                QueryStringValues.Add("detectOrientation", detectOrientation.Value.ToString());
            }
        }

        protected virtual async Task<OcrResponse> GetResponse(HttpContent content)
        {
            string url = GetRegionalUri(_region).ToString();
            var response = await Post<OcrResponse>(url, content, SUBSCRIPTION_KEY_NAME, _subscriptionKey);
            ErrorCode errorCode;

            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                Enum.TryParse<ErrorCode>(response.ErrorCode, out errorCode);

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    switch (errorCode)
                    {
                        case ErrorCode.InvalidImageUrl:
                            throw new Exception(GetExceptionMessage("Image URL is badly formatted or not accessible.", response.ErrorMessage));
                        case ErrorCode.InvalidImageFormat:
                            throw new FormatException(GetExceptionMessage("Input data is not a valid image.", response.ErrorMessage));
                        case ErrorCode.InvalidImageSize:
                            throw new Exception(GetExceptionMessage("Input image is too large.", response.ErrorMessage));
                        case ErrorCode.NotSupportedLanguage:
                            throw new Exception(GetExceptionMessage("Specified language is not supported.", response.ErrorMessage));
                        default:
                            throw GetUnknownErrorException();
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.UnsupportedMediaType)
                {
                    throw new Exception(GetExceptionMessage("The Content-Type request header is not valid.", response.ErrorMessage));
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    switch (errorCode)
                    {
                        case ErrorCode.FailedToProcess:
                            throw new Exception(GetExceptionMessage("Failed to process the image.", response.ErrorMessage));
                        case ErrorCode.Timeout:
                            throw new Exception(GetExceptionMessage("Image processing time out.", response.ErrorCode));
                        case ErrorCode.InternalServerError:
                            throw new Exception(GetExceptionMessage("Internal server error.", response.ErrorMessage));
                        default:
                            throw GetUnknownErrorException();
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new Exception(GetExceptionMessage("Unauthorized.", response.ErrorMessage));
                }
                else
                {
                    throw GetUnknownErrorException();
                }
            }
        }

        protected static string GetExceptionMessage(string mainMessage, params string[] originalMessages)
        {
            return $"{mainMessage}\r\nOriginal messages:\r\n{string.Join("\r\n", originalMessages)}";
        }

        protected static ArgumentOutOfRangeException GetRegionException()
        {
            return new ArgumentOutOfRangeException(nameof(OcrRegions.NotSet), OcrRegions.NotSet, "The region must be set to return its URI.");
        }

        protected static Exception GetUnknownErrorException()
        {
            return new Exception("An unknown error occured.");
        }
        
        #endregion
    }
}