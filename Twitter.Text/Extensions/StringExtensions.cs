using System;

namespace Twitter.Text.Extensions
{
    public static class StringExtensions
    {
        public static string Slice(this string source, int startIndex)
        {
            return Slice(source, startIndex, source.Length);
        }

        public static string Slice(this string source, int startIndex, int endIndex)
        {
            if (startIndex < 0)
            {
                startIndex = source.Length + startIndex;
            }
            if (endIndex < 0)
            {
                endIndex = source.Length + endIndex;
            }
            int length = endIndex - startIndex;
            return source.Substring(startIndex, length);
        }
    }
}