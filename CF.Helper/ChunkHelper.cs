using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using CF.Models.DataModels.ChunkData;

namespace CF.Helpers
{
    public static class ChunkHelper
    {
        public static List<string[]> GetImageDescription(string inputText)
        {
            List<string[]> output = new List<string[]>();


            string pattern = @"<<\[이미지설명\]\s*제목\s*[:：]?\s*([^<]+)\s*설명\s*[:：]?\s*([^<]+)>>";
            Regex regex = new Regex(pattern);

            foreach (Match match in regex.Matches(inputText))
            {
                string title = match.Groups[1].Value.Trim();  // 제목
                string description = match.Groups[2].Value.Trim();  // 설명
                output.Add(new string[] { title, description });
            }

            return output;
        }

        public static List<string[]> GetImageUrl(string inputText)
        {
            List<string[]> output = new List<string[]>();

            string pattern = @"\[(.*?)\]\((.*?)\)";
            Regex regex = new Regex(pattern);

            foreach (Match match in regex.Matches(inputText))
            {
                string title = match.Groups[1].Value.Trim();  // 대괄호 안의 텍스트 (제목)
                string url = match.Groups[2].Value.Trim();    // 소괄호 안의 텍스트 (URL)
                output.Add(new string[] { title, url });
            }

            return output;
        }

        public static List<mChunkImage> GetImageItems(mChunkData chunk)
        {
            List<mChunkImage> output = new List<mChunkImage>();


            List<string[]> descriptionSet = GetImageDescription(chunk.GenerationContents);
            List<string[]> urlSet = GetImageUrl(chunk.ReferenceContents);
            List<string[]> linkSet = chunk.ImageList;

            foreach (string[] link in linkSet)
            {
                string nameString = link[0].Trim();
                string urlString = link[1].Trim();
                string titleString = string.Empty;
                string descriptionString = string.Empty;

                foreach (string[] url in urlSet)
                {
                    if (url[1].Contains(urlString))
                    {
                        titleString = url[0];
                        break;
                    }
                }
                foreach (string[] description in descriptionSet)
                {
                    if (description[0].Contains(titleString))
                    {
                        descriptionString = description[1];
                        break;
                    }
                }



                mChunkImage newImage = new mChunkImage();
                newImage.Name = nameString;
                newImage.URL = urlString;
                newImage.Title = titleString;
                newImage.Description = descriptionString; 

                output.Add(newImage);
            }





            return output;
        }

    }
}
