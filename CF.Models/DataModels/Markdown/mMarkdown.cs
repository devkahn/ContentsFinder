using System.Collections.Generic;

namespace CF.Models.DataModels.Markdown
{
    public class mMarkdown : mBase
    {
        public List<mMarkdownLine> Lines { get; set; } = new List<mMarkdownLine>();
    }
}
