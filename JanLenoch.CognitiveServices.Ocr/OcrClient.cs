using JanLenoch.CognitiveServices.Common;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace JanLenoch.CognitiveServices.Ocr
{
    public class OcrClient : Client
    {
        public enum SupportedLanguages
        {
            NotSet,
            AutoDetect,
            ChineseSimplified,
            ChineseTraditional,
            Czech,
            Danish,
            Dutch,
            English,
            Finnish,
            French,
            German,
            Greek,
            Hungarian,
            Italian,
            Japanese,
            Korean,
            Norwegian,
            Polish,
            Portuguese,
            Russian,
            Spanish,
            Swedish,
            Turkish
        }

        public enum SupportedImageFormats
        {
            NotSet,
            Jpeg,
            Png,
            Gif,
            Bmp
        }

        private const string OCR_URI = @"https://westus.api.cognitive.microsoft.com/vision/v1.0/ocr";

        private readonly string _subscriptionKey;

        public OcrClient(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }


        public async Task<Response> Submit(Uri imageUri, SupportedImageFormats imageFormat = SupportedImageFormats.NotSet, SupportedLanguages language = SupportedLanguages.NotSet, bool? detectOrientation = null)
        {
            PrepareQueryString(language, detectOrientation);

            throw new NotImplementedException();
        }


        public async Task<Response> Submit(byte[] imageBytes, SupportedLanguages language = SupportedLanguages.NotSet, bool? detectOrientation = null)
        {
            PrepareQueryString(language, detectOrientation);
            string contentType;

            using (SKData skData = SKData.CreateCopy(imageBytes))
            {
                using (SKCodec codec = SKCodec.Create(skData))
                {

                    switch (codec.EncodedFormat)
                    {
                        case SKEncodedImageFormat.Jpeg:
                            contentType = "image/jpeg";
                            break;
                        case SKEncodedImageFormat.Png:
                            contentType = "image/png";
                            break;
                        case SKEncodedImageFormat.Gif:
                            contentType = "image/gif";
                            break;
                        case SKEncodedImageFormat.Bmp:
                            contentType = "image/bmp";
                            break;
                        default:
                            contentType = string.Empty;
                            break;
                    }
                }
            }

            if (contentType == string.Empty)
            {
                throw new FormatException("The format of the image is not supported.");
            }

            AddContentTypeHeader(contentType);

            using (var content = new ByteArrayContent(imageBytes))
            {
                // TODO 'application/octet-stream' or 'multipart/form-data' ?
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                return await GetResponse<Response>(OCR_URI, content);
            }
        }

        private void PrepareQueryString(SupportedLanguages language, bool? detectOrientation)
        {
            if (!string.IsNullOrEmpty(_subscriptionKey))
            {
                QueryStringValues.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            }

            if (language != OcrClient.SupportedLanguages.NotSet)
            {
                QueryStringValues.Add("language", Globalization.Languages.First((KeyValuePair<string, string> l) => l.Value == language.ToString()).Key);
            }

            if (detectOrientation.HasValue)
            {
                QueryStringValues.Add("detectOrientation", detectOrientation.Value.ToString());
            }
        }
    }
}
