using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace JanLenoch.CognitiveServices.Ocr
{
    public class Response
    {
        [DataMember(Name = "language")]
        public string Language { get; set; }

        [DataMember(Name = "textAngle")]
        public float TextAngle { get; set; }

        [DataMember(Name = "orientation")]
        public string Orientation { get; set; }

        [DataMember(Name = "regions")]
        public Region[] Regions { get; set; }

        [DataMember(Name = "code")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "requestId")]
        public Guid? RequestId { get; set; }

        [DataMember(Name = "message")]
        public string ErrorMessage { get; set; }
    }

    public struct BoundingBox
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public abstract class Boxed
    {
        private readonly BoundingBox _boundingBox;

        [DataMember(Name = "boundingBox")]
        private string BoundingBoxText { get; set; }

        public Boxed()
        {
            string[] split = BoundingBoxText.Split(",".ToCharArray());
            int left;
            int top;
            int width;
            int height;
            int.TryParse(split[0], out left);
            int.TryParse(split[1], out top);
            int.TryParse(split[2], out width);
            int.TryParse(split[3], out height);

            _boundingBox = new BoundingBox
            {
                Left = left,
                Top = top,
                Width = width,
                Height = height
            };
        }

        public BoundingBox BoundingBox
        {
            get
            {
                return _boundingBox;
            }
        }
    }

    public class Region : Boxed
    {
        [DataMember(Name = "lines")]
        public Line[] Lines { get; set; }
    }

    public class Line : Boxed
    {
        [DataMember(Name = "words")]
        public Word[] Words { get; set; }
    }

    public class Word : Boxed
    {
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

}
