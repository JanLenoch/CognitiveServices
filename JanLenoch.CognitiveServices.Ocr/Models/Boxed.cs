using System.Runtime.Serialization;

namespace JanLenoch.CognitiveServices.Ocr
{
    [DataContract]
    public abstract class Boxed
    {
        private BoundingBox _boundingBox;

        [DataMember(Name = "boundingBox")]
        private string BoundingBoxText { get; set; }

        private void GetBoundingBox()
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
                if (_boundingBox.Left == default(int))
                {
                    GetBoundingBox();
                }

                return _boundingBox;
            }
        }
    }

}
