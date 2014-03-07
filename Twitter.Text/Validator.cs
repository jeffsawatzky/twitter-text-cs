using System;
using System.Text;

namespace Twitter.Text
{
    /// <summary>
    /// A class for validating Tweet texts.
    /// </summary>
    public class Validator
    {
        protected const int SHORT_URL_LENGTH = 22;
        protected const int SHORT_URL_LENGTH_HTTPS = 23;
        public const int MAX_TWEET_LENGTH = 140;

        private Extractor __Extractor = new Extractor();

        /// <summary>
        /// 
        /// </summary>
        public int ShortUrlLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ShortUrlLengthHttps { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Validator()
        {
            this.ShortUrlLength = SHORT_URL_LENGTH;
            this.ShortUrlLengthHttps = SHORT_URL_LENGTH_HTTPS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int GetTweetLength(String text)
        {
            try
            {
                text = text.Normalize(NormalizationForm.FormC);
            }
            catch { }

            int length = text.Length;
            foreach (Extractor.Entity urlEntity in __Extractor.ExtractURLsWithIndices(text))
            {
                // Subtract the length of the original URL
                length -= (urlEntity.End - urlEntity.Start);

                // Add `ShortUrlLengthHttps` characters for URL starting with https:// Otherwise add `ShortUrlLength` characters
                length += urlEntity.Value.ToLower().StartsWith("https://") ? ShortUrlLengthHttps : ShortUrlLength;
            }
            return length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool IsValidTweet(String text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            foreach (char c in text)
            {
                if (
                    c == '\uFFFE' || c == '\uFEFF' ||   // BOM
                    c == '\uFFFF' ||                    // Special
                    (c >= '\u202A' && c <= '\u202E')    // Direction change
                )
                {
                    return false;
                }
            }

            return this.GetTweetLength(text) <= MAX_TWEET_LENGTH;
        }
    }
}