using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Models.DataModels
{
    public class mSlide : mBase
    {
        public int CheckStatus { get; set; } = -1;
        public int Index { get; set; } = 0;
        public List<mTextShape> TextShapes { get; set; } = new List<mTextShape>();
    }
}
