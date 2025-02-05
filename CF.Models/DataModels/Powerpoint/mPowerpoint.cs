using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Models.DataModels.Powerpoint
{
    public class mPowerpoint : mBase
    {
        public List<mSlide> Slides { get; set; } = new List<mSlide>();
    }
}
