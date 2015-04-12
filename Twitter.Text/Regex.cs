using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Pattern = System.Text.RegularExpressions.Regex;

namespace Twitter.Text
{
    /// <summary>
    /// Patterns and regular expressions used by the twitter text methods.
    /// </summary>
    internal static class Regex
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static Regex()
        {
            //
            // Create the equivalent of Java's \p{Alpha}
            // See http://docs.oracle.com/javase/7/docs/api/java/util/regex/Pattern.html
            //
            // \p{Alpha}    An alphabetic character:[\p{Lower}\p{Upper}]
            // \p{Lower}    A lower-case alphabetic character: [a-z]
            // \p{Upper}    An upper-case alphabetic character:[A-Z]
            //
            // Note: It is meant to be used in a character group
            //
            String ALPHA_CHARS = "a-zA-Z";

            //
            // Create the equivalent of Java's \p{Digit}
            // See http://docs.oracle.com/javase/7/docs/api/java/util/regex/Pattern.html
            //
            // \p{Digit}    A decimal digit: [0-9]
            //
            // Note: It is meant to be used in a character group
            //
            String NUM_CHARS = "0-9";

            //
            // Create the equivalent of Java's \p{Alnum}
            // See http://docs.oracle.com/javase/7/docs/api/java/util/regex/Pattern.html
            //
            // \p{Alnum}    An alphanumeric character:[\p{Alpha}\p{Digit}]
            //
            // Note: It is meant to be used in a character group
            //
            String ALNUM_CHARS = ALPHA_CHARS + NUM_CHARS;

            //
            // Create the quivalent of Java's \p{Punct}
            // See http://docs.oracle.com/javase/7/docs/api/java/util/regex/Pattern.html
            //
            // \p{Punct}    Punctuation: One of !"#$%&'()*+,-./:;<=>?@[\]^_`{|}~
            //
            // Note: It is meant to be used in a character group
            //
            String PUNCT_CHARS = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~".Replace(@"\", @"\\").Replace(@"]", @"\]").Replace(@"-", @"\-");

            String URL_VALID_GTLD = "(?:(?:" + string.Join("|", TldLib.Generic) + ")(?=[^" + ALNUM_CHARS + "@]|$))";

            String URL_VALID_CCTLD = "(?:(?:" + string.Join("|", TldLib.Country) + ")(?=[^" + ALNUM_CHARS + "@]|$))";

            //
            // Space is more than %20, U+3000 for example is the full-width space used with Kanji. Provide a short-hand
            // to access both the list of characters and a pattern suitible for use with String#split
            // Taken from: ActiveSupport::Multibyte::Handlers::UTF8Handler::UNICODE_WHITESPACE
            //
            String UNICODE_SPACES = "[" +
                    "\u0009-\u000d" +     // White_Space # Cc [5]    <control-0009>..<control-000D>
                    "\u0020" +            // White_Space # Zs        SPACE
                    "\u0085" +            // White_Space # Cc        <control-0085>
                    "\u00a0" +            // White_Space # Zs        NO-BREAK SPACE
                    "\u1680" +            // White_Space # Zs        OGHAM SPACE MARK
                    "\u180E" +            // White_Space # Zs        MONGOLIAN VOWEL SEPARATOR
                    "\u2000-\u200a" +     // White_Space # Zs [11]   EN QUAD..HAIR SPACE
                    "\u2028" +            // White_Space # Zl        LINE SEPARATOR
                    "\u2029" +            // White_Space # Zp        PARAGRAPH SEPARATOR
                    "\u202F" +            // White_Space # Zs        NARROW NO-BREAK SPACE
                    "\u205F" +            // White_Space # Zs        MEDIUM MATHEMATICAL SPACE
                    "\u3000" +            // White_Space # Zs        IDEOGRAPHIC SPACE
                "]";

            // Character not allowed in Tweets
            String INVALID_CONTROL_CHARS = "[" +
                    "\ufffe\ufeff" +    // BOM
                    "\uffff" +          // Special
                    "\u202a-\u202e" +   // Directional change
                "]";

            //
            // Latin accented characters
            // Excludes 0xd7 from the range (the multiplication sign, confusable with "x").
            // Also excludes 0xf7, the division sign
            //
            String LATIN_ACCENTS_CHARS =
                "\u00c0-\u00d6\u00d8-\u00f6\u00f8-\u00ff" +                                     // Latin-1
                "\u0100-\u024f" +                                                               // Latin Extended A and B
                "\u0253\u0254\u0256\u0257\u0259\u025b\u0263\u0268\u026f\u0272\u0289\u028b" +    // IPA Extensions
                "\u02bb" +                                                                      // Hawaiian
                "\u0300-\u036f" +                                                               // Combining diacritics
                "\u1e00-\u1eff";                                                                // Latin Extended Additional (mostly for Vietnamese)

            String RTL_CHARS =
                "\u0600-\u06FF" +
                "\u0750-\u077F" +
                "\u0590-\u05FF" +
                "\uFE70-\uFEFF";

            //
            // Hashtag related patterns
            //

            String HASHTAG_LETTERS = "\\p{L}\\p{M}";
            String HASHTAG_NUMERALS = "\\p{Nd}";
            String HASHTAG_SPECIAL_CHARS = "_" + //underscore
                                           "\\u200c" + // ZERO WIDTH NON-JOINER (ZWNJ)
                                           "\\u200d" + // ZERO WIDTH JOINER (ZWJ)
                                           "\\ua67e" + // CYRILLIC KAVYKA
                                           "\\u05be" + // HEBREW PUNCTUATION MAQAF
                                           "\\u05f3" + // HEBREW PUNCTUATION GERESH
                                           "\\u05f4" + // HEBREW PUNCTUATION GERSHAYIM
                                           "\\u309b" + // KATAKANA-HIRAGANA VOICED SOUND MARK
                                           "\\u309c" + // KATAKANA-HIRAGANA SEMI-VOICED SOUND MARK
                                           "\\u30a0" + // KATAKANA-HIRAGANA DOUBLE HYPHEN
                                           "\\u30fb" + // KATAKANA MIDDLE DOT
                                           "\\u3003" + // DITTO MARK
                                           "\\u0f0b" + // TIBETAN MARK INTERSYLLABIC TSHEG
                                           "\\u0f0c" + // TIBETAN MARK DELIMITER TSHEG BSTAR
                                           "\\u0f0d";  // TIBETAN MARK SHAD
            String HASHTAG_LETTERS_NUMERALS = HASHTAG_LETTERS + HASHTAG_NUMERALS + HASHTAG_SPECIAL_CHARS;

            String HASHTAG_LETTERS_SET = "[" + HASHTAG_LETTERS + "]";

            String HASHTAG_LETTERS_NUMERALS_SET = "[" + HASHTAG_LETTERS_NUMERALS + "]";

            String VALID_HASHTAG_STRING = "(^|[^&" + HASHTAG_LETTERS_NUMERALS + "])(#|\uFF03)(" + HASHTAG_LETTERS_NUMERALS_SET + "*" + HASHTAG_LETTERS_SET + HASHTAG_LETTERS_NUMERALS_SET + "*)";

            //
            // URL related patterns
            //

            String URL_VALID_PRECEEDING_CHARS = "(?:[^A-Z0-9@＠$#＃\u202A-\u202E]|^)";

            String URL_VALID_CHARS = ALNUM_CHARS + LATIN_ACCENTS_CHARS;

            String URL_VALID_SUBDOMAIN = "(?>(?:[" + URL_VALID_CHARS + "][" + URL_VALID_CHARS + "\\-_]*)?[" + URL_VALID_CHARS + "]\\.)";

            String URL_VALID_DOMAIN_NAME = "(?:(?:[" + URL_VALID_CHARS + "][" + URL_VALID_CHARS + "\\-]*)?[" + URL_VALID_CHARS + "]\\.)";

            // Any non-space, non-punctuation characters. \p{Z} = any kind of whitespace or invisible separator.
            String URL_VALID_UNICODE_CHARS = "(?:\\.|[^" + PUNCT_CHARS + "\\s\\p{Z}\\p{IsGeneralPunctuation}])";

            String URL_PUNYCODE = "(?:xn--[0-9a-z]+)";

            String SPECIAL_URL_VALID_CCTLD = "(?:(?:" + "co|tv" + ")(?=[^" + ALNUM_CHARS + "@]|$))";

            String URL_VALID_DOMAIN =
                "(?:" +                                                   // subdomains + domain + TLD
                    URL_VALID_SUBDOMAIN + "+" + URL_VALID_DOMAIN_NAME +   // e.g. www.twitter.com, foo.co.jp, bar.co.uk
                    "(?:" + URL_VALID_GTLD + "|" + URL_VALID_CCTLD + "|" + URL_PUNYCODE + ")" +
                    ")" +
                "|(?:" +                                                  // domain + gTLD + some ccTLD
                    URL_VALID_DOMAIN_NAME +                                 // e.g. twitter.com
                    "(?:" + URL_VALID_GTLD + "|" + URL_PUNYCODE + "|" + SPECIAL_URL_VALID_CCTLD + ")" +
                ")" +
                "|(?:" + "(?<=https?://)" +
                    "(?:" +
                    "(?:" + URL_VALID_DOMAIN_NAME + URL_VALID_CCTLD + ")" +  // protocol + domain + ccTLD
                    "|(?:" +
                        URL_VALID_UNICODE_CHARS + "+\\." +                     // protocol + unicode domain + TLD
                        "(?:" + URL_VALID_GTLD + "|" + URL_VALID_CCTLD + ")" +
                    ")" +
                    ")" +
                ")" +
                "|(?:" +                                                  // domain + ccTLD + '/'
                    URL_VALID_DOMAIN_NAME + URL_VALID_CCTLD + "(?=/)" +     // e.g. t.co/
                ")";

            String URL_VALID_PORT_NUMBER = "(?>[0-9]+)";

            String URL_VALID_GENERAL_PATH_CHARS = "[a-z0-9!\\*';:=\\+,.\\$/%#\\[\\]\\-_~\\|&@" + LATIN_ACCENTS_CHARS + "]";

            //
            // Allow URL paths to contain up to two nested levels of balanced parens
            //  1. Used in Wikipedia URLs like /Primer_(film)
            //  2. Used in IIS sessions like /S(dfd346)/
            //  3. Used in Rdio URLs like /track/We_Up_(Album_Version_(Edited))/
            //
            String URL_BALANCED_PARENS =
                "\\(" +
                    "(?:" +
                        URL_VALID_GENERAL_PATH_CHARS + "+" +
                        "|" +
                        // allow one nested level of balanced parentheses
                        "(?:" +
                            URL_VALID_GENERAL_PATH_CHARS + "*" +
                            "\\(" +
                                URL_VALID_GENERAL_PATH_CHARS + "+" +
                            "\\)" +
                            URL_VALID_GENERAL_PATH_CHARS + "*" +
                        ")" +
                    ")" +
                "\\)";

            //
            // Valid end-of-path characters (so /foo. does not gobble the period).
            //   1. Allow =&# for empty URL parameters and other URL-join artifacts
            //
            String URL_VALID_PATH_ENDING_CHARS = "[a-z0-9=_#/\\-\\+" + LATIN_ACCENTS_CHARS + "]|(?:" + URL_BALANCED_PARENS + ")";

            String URL_VALID_PATH =
                "(?:" +
                    "(?:" +
                        URL_VALID_GENERAL_PATH_CHARS + "*" +
                        "(?:" + URL_BALANCED_PARENS + URL_VALID_GENERAL_PATH_CHARS + "*)*" +
                        URL_VALID_PATH_ENDING_CHARS +
                    ")|(?:@" + URL_VALID_GENERAL_PATH_CHARS + "+/)" +
                ")";

            String URL_VALID_URL_QUERY_CHARS = "[a-z0-9!?\\*'\\(\\);:&=\\+\\$/%#\\[\\]\\-_\\.,~\\|@]";

            String URL_VALID_URL_QUERY_ENDING_CHARS = "[a-z0-9_&=#/]";

            String VALID_URL_PATTERN_STRING =
                "(" +                                                   //  $1 total match
                    "(" + URL_VALID_PRECEEDING_CHARS + ")" +            //  $2 Preceeding chracter
                    "(" +                                               //  $3 URL
                        "(https?://)?" +                                //  $4 Protocol (optional)
                        "(" + URL_VALID_DOMAIN + ")" +                  //  $5 Domain(s)
                        "(?::(" + URL_VALID_PORT_NUMBER + "))?" +       //  $6 Port number (optional)
                        "(/" +
                            "(?>" + URL_VALID_PATH + "*)" +
                        ")?" +                                          //  $7 URL Path and anchor
                        "(\\?" + URL_VALID_URL_QUERY_CHARS + "*" +      //  $8 Query String 
                            URL_VALID_URL_QUERY_ENDING_CHARS + ")?" +
                    ")" +
                ")";

            String AT_SIGNS_CHARS = "@\uFF20";

            String DOLLAR_SIGN_CHAR = "\\$";

            //
            // Cashtag related patterns
            //

            String CASHTAG = "[a-z]{1,6}(?:[._][a-z]{1,2})?";

            //
            // Begin public constants
            //

            INVALID_CHARACTERS = new Pattern(INVALID_CONTROL_CHARS, RegexOptions.IgnoreCase);

            VALID_HASHTAG = new Pattern(VALID_HASHTAG_STRING, RegexOptions.IgnoreCase);

            INVALID_HASHTAG_MATCH_END = new Pattern("^(?:[#＃]|://)");

            RTL_CHARACTERS = new Pattern("[" + RTL_CHARS + "]");

            AT_SIGNS = new Pattern("[" + AT_SIGNS_CHARS + "]");

            VALID_MENTION_OR_LIST = new Pattern("([^a-z0-9_!#$%&*" + AT_SIGNS_CHARS + "]|^|RT:?)(" + AT_SIGNS + "+)([a-z0-9_]{1,20})(/[a-z][a-z0-9_\\-]{0,24})?", RegexOptions.IgnoreCase);

            VALID_REPLY = new Pattern("^(?:" + UNICODE_SPACES + ")*" + AT_SIGNS + "([a-z0-9_]{1,20})", RegexOptions.IgnoreCase);

            INVALID_MENTION_MATCH_END = new Pattern("^(?:[" + AT_SIGNS_CHARS + LATIN_ACCENTS_CHARS + "]|://)");

            VALID_URL = new Pattern(VALID_URL_PATTERN_STRING, RegexOptions.IgnoreCase);

            VALID_TCO_URL = new Pattern("^https?:\\/\\/t\\.co\\/[a-z0-9]+", RegexOptions.IgnoreCase);

            INVALID_URL_WITHOUT_PROTOCOL_MATCH_BEGIN = new Pattern("[-_./]$");

            VALID_CASHTAG = new Pattern("(^|" + UNICODE_SPACES + ")(" + DOLLAR_SIGN_CHAR + ")(" + CASHTAG + ")" + "(?=$|\\s|[" + PUNCT_CHARS + "])", RegexOptions.IgnoreCase);
        }

        public static readonly Pattern INVALID_CHARACTERS;

        public static readonly Pattern VALID_HASHTAG;
        public const int VALID_HASHTAG_GROUP_BEFORE = 1;
        public const int VALID_HASHTAG_GROUP_HASH = 2;
        public const int VALID_HASHTAG_GROUP_TAG = 3;

        public static readonly Pattern INVALID_HASHTAG_MATCH_END;

        public static readonly Pattern RTL_CHARACTERS;

        public static readonly Pattern AT_SIGNS;

        public static readonly Pattern VALID_MENTION_OR_LIST;
        public const int VALID_MENTION_OR_LIST_GROUP_BEFORE = 1;
        public const int VALID_MENTION_OR_LIST_GROUP_AT = 2;
        public const int VALID_MENTION_OR_LIST_GROUP_USERNAME = 3;
        public const int VALID_MENTION_OR_LIST_GROUP_LIST = 4;

        public static readonly Pattern VALID_REPLY;
        public const int VALID_REPLY_GROUP_USERNAME = 1;

        public static readonly Pattern INVALID_MENTION_MATCH_END;

        public static readonly Pattern VALID_URL;
        public const int VALID_URL_GROUP_ALL = 1;
        public const int VALID_URL_GROUP_BEFORE = 2;
        public const int VALID_URL_GROUP_URL = 3;
        public const int VALID_URL_GROUP_PROTOCOL = 4;
        public const int VALID_URL_GROUP_DOMAIN = 5;
        public const int VALID_URL_GROUP_PORT = 6;
        public const int VALID_URL_GROUP_PATH = 7;
        public const int VALID_URL_GROUP_QUERY_STRING = 8;

        public static readonly Pattern VALID_TCO_URL;

        public static readonly Pattern INVALID_URL_WITHOUT_PROTOCOL_MATCH_BEGIN;

        public static readonly Pattern VALID_CASHTAG;
        public const int VALID_CASHTAG_GROUP_BEFORE = 1;
        public const int VALID_CASHTAG_GROUP_DOLLAR = 2;
        public const int VALID_CASHTAG_GROUP_CASHTAG = 3;
    }
}