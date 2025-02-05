using System;
using System.IO;
using System.Linq;
using System.Xml;
using CF.Commons;
using CF.Models.DataModels;
using CF.Models.DataModels.Markdown;
using CF.Models.ViewModels.MarkDownSetting;


namespace CF.Helpers
{
    public static class MarkdownHelper
    {
        public static bool IsLoaded(vmMarkdown markdown)
        {
            return markdown != null;

        }
        private static eLineType GetLineType(string text)
        {
            string target = text.Trim();
            if(target.StartsWith("#"))
            {
                if (target.StartsWith("#######")) return eLineType.Heading7;
                if (target.StartsWith("######")) return eLineType.Heading6;
                if (target.StartsWith("#####")) return eLineType.Heading5;
                if (target.StartsWith("####")) return eLineType.Heading4;
                if (target.StartsWith("###")) return eLineType.Heading3;
                if (target.StartsWith("##")) return eLineType.Heading2;
                if (target.StartsWith("#")) return eLineType.Heading1;
            }
            else if(string.IsNullOrEmpty(text)|| string.IsNullOrWhiteSpace(text))
            {
                return eLineType.Empty;
            }
            else
            {
                return eLineType.NormalText;
            }

            return eLineType.None;
        }
     
        public static vmMarkdown LoadMarkdonw(vmMarkdown markdown)
        {
            vmMarkdown output = null;

            bool isMarkdownLoaded = IsLoaded(markdown);
            if (isMarkdownLoaded)
            {
                output = markdown;
            }
            else
            {
                bool isLoaded = OpenMarkdown(output);
            }

            return output;
        }
        public static bool OpenMarkdown(vmMarkdown markdown)
        {
            string caption = "파일에서 Markdown 데이터 불러오기";

            FileInfo fInfo = FileHelper.OpenFile(caption);
            if (fInfo == null) return false;
            string mdOrigin = File.ReadAllText(fInfo.FullName);

            mMarkdown newMD = new mMarkdown();
            string[] lines = mdOrigin.Split('\n');
            for (int i = 0; i < lines.Count(); i++)
            {
                string ln = lines[i];
                mMarkdownLine newLine = new mMarkdownLine();
                newLine.Num = i + 1;
                newLine.LineTypeCode = GetLineType(ln).GetHashCode();

                string mark = TextHelper.GetLineMark(ln);
                if(string.IsNullOrEmpty(mark))
                {
                    newLine.Text = ln.Trim();
                }
                else
                {
                    newLine.Mark = mark;
                    newLine.Text = ln.Substring(mark.Length).Trim();
                }

                newMD.Lines.Add(newLine);
            }

            markdown = new vmMarkdown(newMD);
            return true;
        }


        //public static mHeading GetParentHeading(mHeading value)
        //{
        //    switch ((eLineType)value.HeadingTypeCode)
        //    {
        //        case eLineType.Heading1:
        //            return null;
        //        case eLineType.Heading2:
        //            return ProgramValues.CurrentMaterial.Markdown.Headings.Where(x => x.Id == value.ParentId).FirstOrDefault();
        //        case eLineType.Heading3:
        //            return ProgramValues.CurrentMaterial.Markdown.Headings.Where(x => x.Id == value.ParentId).FirstOrDefault();
        //        case eLineType.Heading4:
        //            return ProgramValues.CurrentMaterial.Markdown.Headings.Where(x => x.Id == value.ParentId).FirstOrDefault();
        //        case eLineType.Heading5:
        //            return ProgramValues.CurrentMaterial.Markdown.Headings.Where(x => x.Id == value.ParentId).FirstOrDefault();
        //        case eLineType.Heading6:
        //            return ProgramValues.CurrentMaterial.Markdown.Headings.Where(x => x.Id == value.ParentId).FirstOrDefault();
        //        case eLineType.Heading7:
        //        default:
        //            return null;
        //    }
        //}

    }
}
