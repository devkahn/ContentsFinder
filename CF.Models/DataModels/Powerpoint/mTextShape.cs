using System;


namespace CF.Models.DataModels
{
    public class mTextShape : mShape
    {
        public string Text { get; set; }  = string.Empty;
        public double DistanceFromOrigin { get; set; } = -1;

        public string MapContentId { get; set; }  = string.Empty;
      
    }
}
