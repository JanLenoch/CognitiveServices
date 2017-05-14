using System.Runtime.Serialization;

namespace JanLenoch.CognitiveServices.Ocr
{
    [DataContract]
    public class Line : Boxed
    {
        [DataMember(Name = "words")]
        public Word[] Words { get; set; }
    }

}
