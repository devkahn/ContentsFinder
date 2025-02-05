using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Models.DataModels
{
    public class mMarkdownLine : mBase
    {
        public int Num { get; set; } = -1;
        public int LineTypeCode { get; set; } = -1;
        public string Mark { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;


        public int Page { get; set; } = -1;
    }
}
