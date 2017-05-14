using JanLenoch.CognitiveServices.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace JanLenoch.CognitiveServices.Ocr
{
    [DataContract]
    public class OcrResponse : Response
    {
        [DataMember(Name = "language")]
        public string Language { get; set; }

        [DataMember(Name = "textAngle")]
        public float TextAngle { get; set; }

        [DataMember(Name = "orientation")]
        public string Orientation { get; set; }

        [DataMember(Name = "regions")]
        public Region[] Regions { get; set; }
    }
}
