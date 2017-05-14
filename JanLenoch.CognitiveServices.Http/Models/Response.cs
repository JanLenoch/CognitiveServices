using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace JanLenoch.CognitiveServices.Http
{
    [DataContract]
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }

        [DataMember(Name = "code")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "requestId")]
        public Guid? RequestId { get; set; }

        [DataMember(Name = "message")]
        public string ErrorMessage { get; set; }
    }
}
