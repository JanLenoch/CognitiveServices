using System.Runtime.Serialization;

namespace JanLenoch.CognitiveServices.Ocr
{
    [DataContract]
    public class UrlContent
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
