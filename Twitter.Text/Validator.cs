using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Twitter.Text
{
    /// <summary>
    /// A class for validating Tweet texts.
    /// </summary>
    public class Validator
    {
        protected const int SHORT_URL_LENGTH = 23;
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
        /// <param name="text">Target of string. Should be normalized 'NormalizationForm.FormC'</param>
        /// <returns></returns>
        public int GetTweetLength(String text)
        {
            try
            {
                //text = text.Normalize(NormalizationForm.FormC);
            }
            catch { }

            int length = new StringInfo(text).LengthInTextElements;
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

            if (Regex.INVALID_CHARACTERS.IsMatch(text))
            {
                return false;
            }

            return this.GetTweetLength(text) <= MAX_TWEET_LENGTH;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool IsValidUsername(String text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            // Must match whole string
            Match match = Regex.VALID_REPLY.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidList(String text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            // Must match, have nothing before, and contain a list
            Match match = Regex.VALID_MENTION_OR_LIST.Match(text);
            if (match.Success &&
                match.Length == text.Length &&
                String.IsNullOrEmpty(match.Groups[Regex.VALID_MENTION_OR_LIST_GROUP_BEFORE].Value) &&
                !String.IsNullOrEmpty(match.Groups[Regex.VALID_MENTION_OR_LIST_GROUP_LIST].Value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidHashTag(String text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            // Must match, have nothing before, and contain a list
            Match match = Regex.VALID_HASHTAG.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidUrl(String text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            // Must match, have nothing before, and contain a list
            Match match = Regex.VALID_URL.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}