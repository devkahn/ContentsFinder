﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;


namespace CF.Helpers
{
    public static class TextHelper
    {
        public static string RemoveHeadingSymbol(this string value)
        {
            string output = value;
            while (output.First() == '#')
            { 
                output = output.Substring(1);   
            }
            return output;
        }
        public static string RemoveEmtpy(this string value)
        {
            string output = string.Empty;
            foreach (char item in value)
            {
                if (char.IsWhiteSpace(item)) continue;
                output += item;
            }
            return output;
        }
        public static string RemoveSpecialChar(this string value)
        {
            return Regex.Replace(value, @"[^a-zA-Z0-9\s\uac00-\ud7af]", "");
        }
        public static string GetLineMark(string lineText)
        {
            string output = string.Empty;

            foreach (char c in lineText)
            {
                if (char.IsLetter(c)) break;
                output += c;
            }

            return output.Trim();
        }
        public static double CalculateSimilary(string origin, string target)
        {
            int distance = LevenshteinDistance(origin, target);
            int maxLength = Math.Max(origin.Length, target.Length);
            return (1.0 - (double)distance / maxLength) * 100; // 유사도를 퍼센트로 계산
        }
        private static int LevenshteinDistance(string origin, string target)
        {
            int n = origin.Length;
            int m = target.Length;
            int[,] d = new int[n + 1, m + 1];

            // 초기화
            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            // 편집 거리 계산
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (origin[i - 1] == target[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }

        public static bool IsTextNuberic(string text)
        {
            foreach (char c in text)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

       
        public static string HighlightMarkdown(string value)
        {

            return value;
        }
    }
}
