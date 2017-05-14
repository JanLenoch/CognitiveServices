using System.Runtime.Serialization;

namespace JanLenoch.CognitiveServices.Ocr
{
    [DataContract]
    public class Region : Boxed
    {
        [DataMember(Name = "lines")]
        public Line[] Lines { get; set; }
    }

}
