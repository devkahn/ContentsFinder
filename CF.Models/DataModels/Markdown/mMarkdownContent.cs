using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Models.DataModels.Markdown
{
    public class mMarkdownContent : mBase
    {
        public int CheckStatusCode { get; set; } = -1;
        public int Page { get; set; } = -1;
        public int StartLineNumber { get; set; } = -1;
        public int EndLineNumber { get; set; } = -1;
        public List<string> LineIds { get; set; } = new List<string>();
    }
}
