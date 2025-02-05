using System.Linq;
using CF.Models.ViewModels.PowerPointSetting;

namespace CF.Helpers
{
    public static class SlideHelper
    {
        public static vmSlide GetNextSlide(vmSlide current)
        {
            vmSlide output = null;

            int curIndex = current.ParentPowerpoint.Slides.IndexOf(current);
            if (curIndex < current.ParentPowerpoint.Slides.Count - 1)
            {
                output = current.ParentPowerpoint.Slides[curIndex + 1];
            }

            return output;
        }
    }
}
