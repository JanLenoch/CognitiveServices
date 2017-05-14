using JanLenoch.CognitiveServices.Http;
using JanLenoch.CognitiveServices.Ocr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace JanLenoch.CognitiveServices.Tests
{
    public class OcrTests
    {
        // TODO Do not commit the key to SC
        private const string API_KEY = "";
        private const OcrRegions REGION = OcrRegions.WestUs;

        [Fact]
        public void UnsetUriThrows()
        {
            // Arrange
            OcrClient client = new OcrClient(API_KEY, REGION);

            // Act

            // Assert
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => client.GetRegionalUri(OcrRegions.NotSet));
        }

        [Fact]
        public void ParsesEmptyQueryString()
        {
            // Arrange
            OcrClient client = new OcrClient(API_KEY, REGION);

            // Act
            string parsedString = client.ParseQueryString();

            // Assert
            Assert.NotNull(parsedString);
            Assert.Equal(string.Empty, parsedString);
        }

        [Fact]
        public void ParsesQueryString()
        {
            // Arrange
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "parameter1", "parameter1value" },
                { "parameter2", "Parameter2ValueWithCapitals" },
                { "parameter3", "parameter3valuewithspecialcharacters@&#να" }
            };

            OcrClient client = new OcrClient(API_KEY, REGION);

            foreach (var parameter in parameters)
            {
                client.QueryStringValues.Add(parameter.Key, parameter.Value);
            }

            // Act
            string parsedString = client.ParseQueryString();

            // Assert
            Assert.NotNull(parsedString);
            Assert.Equal("?parameter1=parameter1value&parameter2=Parameter2ValueWithCapitals&parameter3=parameter3valuewithspecialcharacters%40%26%23%C5%A1%C3%AD%C3%A1", parsedString);
        }

        [Fact]
        public void CastsPostResponseToType()
        {
            // Arrange
            byte[] contentBytes = File.ReadAllBytes(@"..\..\..\OcrTests\AboutUs.png");
            OcrClient client = new OcrClient(API_KEY, REGION);
            string url = client.GetRegionalUri(REGION).ToString();
            object response;

            // Act
            using (ByteArrayContent content = new ByteArrayContent(contentBytes))
            {
                response = Task.Run(() => client.Post<OcrResponse>(url, content, "Ocp-Apim-Subscription-Key", API_KEY)).Result;
            }

            // Assert
            Assert.NotNull(response);
            Assert.IsType(typeof(OcrResponse), response);
        }

        public void WrongImageFormatThrows()
        {

        }

        [Theory]
        [InlineData(OcrLanguages.AutoDetect)]
        [InlineData(OcrLanguages.English)]
        public void RecognizesWithLanguage(OcrLanguages language)
        {
            // Arrange
            OcrClient client = new OcrClient(API_KEY, REGION);
            OcrResponse response;

            // Act
            response = Task.Run(() => client.RecognizeAsync(new Uri(@"https://oxfordportal.blob.core.windows.net/vision/doc-vision-overview-ocr01.png"), language: language)).Result;

            // Assert
            Assert.NotNull(response);
            Assert.Null(response.ErrorCode);
            Assert.Equal(response.Language, "en");
            Assert.NotNull(response.Regions);
            Assert.NotEmpty(response.Regions);
            Assert.NotEqual(response.Regions.First().BoundingBox.Left, default(int));
            Assert.NotEqual(response.Regions.First().BoundingBox.Top, default(int));
            Assert.NotEqual(response.Regions.First().BoundingBox.Width, default(int));
            Assert.NotEqual(response.Regions.First().BoundingBox.Height, default(int));
            Assert.NotNull(response.Regions.First().Lines);
            Assert.NotEmpty(response.Regions.First().Lines);
            Assert.NotNull(response.Regions.First().Lines.First().Words);
        }

        [Theory]
        [InlineData("AboutUs.png")]
        [InlineData("Products.png")]
        public void RecognizesWithByteArray(string imageFileName)
        {
            // Arrange
            byte[] contentBytes = File.ReadAllBytes(@"..\..\..\OcrTests\" + imageFileName);
            OcrClient client = new OcrClient(API_KEY, REGION);
            OcrResponse response;

            // Act
            response = Task.Run(() => client.RecognizeAsync(contentBytes)).Result;

            // Assert
            Assert.NotNull(response);
            Assert.Null(response.ErrorCode);
            Assert.NotNull(response.Regions);
            Assert.NotEmpty(response.Regions);
            Assert.NotEqual(response.Regions.First().BoundingBox.Left, default(int));
            Assert.NotEqual(response.Regions.First().BoundingBox.Top, default(int));
            Assert.NotEqual(response.Regions.First().BoundingBox.Width, default(int));
            Assert.NotEqual(response.Regions.First().BoundingBox.Height, default(int));
            Assert.NotNull(response.Regions.First().Lines);
            Assert.NotEmpty(response.Regions.First().Lines);
            Assert.NotNull(response.Regions.First().Lines.First().Words);
        }
    }
}
