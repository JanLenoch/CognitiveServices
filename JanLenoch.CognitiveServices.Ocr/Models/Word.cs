using System.Runtime.Serialization;

namespace JanLenoch.CognitiveServices.Ocr
{
    [DataContract]
    public class Word : Boxed
    {
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

}
