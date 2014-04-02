using System.Collections.Generic;
using System.Text;

namespace Twitter.Text
{
    /// <summary>
    /// A class for adding HTML links to hashtag, username and list references in Tweet text.
    /// </summary>
    public class Autolink
    {
        /// <summary>
        /// The Extractor used to extract entities from text.
        /// </summary>
        private Extractor __Extractor;

        /// <summary>
        /// Default CSS class for auto-linked list URLs
        /// </summary>
        public const string DEFAULT_LIST_CLASS = "tweet-url list-slug";

        /// <summary>
        /// Default CSS class for auto-linked username URLs
        /// </summary>
        public const string DEFAULT_USERNAME_CLASS = "tweet-url username";

        /// <summary>
        /// Default CSS class for auto-linked hashtag URLs
        /// </summary>
        public const string DEFAULT_HASHTAG_CLASS = "tweet-url hashtag";

        /// <summary>
        /// Default CSS class for auto-linked cashtag URLs
        /// </summary>
        public const string DEFAULT_CASHTAG_CLASS = "tweet-url cashtag";

        /// <summary>
        /// Default href for username links (the username without the @ will be appended)
        /// </summary>
        public const string DEFAULT_USERNAME_URL_BASE = "https://twitter.com/";

        /// <summary>
        /// Default href for list links (the username/list without the @ will be appended)
        /// </summary>
        public const string DEFAULT_LIST_URL_BASE = "https://twitter.com/";

        /// <summary>
        /// Default href for hashtag links (the hashtag without the # will be appended)
        /// </summary>
        public const string DEFAULT_HASHTAG_URL_BASE = "https://twitter.com/#!/search?q=%23";

        /// <summary>
        /// Default href for cashtag links (the cashtag without the $ will be appended)
        /// </summary>
        public const string DEFAULT_CASHTAG_URL_BASE = "https://twitter.com/#!/search?q=%24";

        /// <summary>
        /// Default attribute for invisible span tag
        /// </summary>
        public const string DEFAULT_INVISIBLE_TAG_ATTRS = "style='position:absolute;left:-9999px;'";

        /// <summary>
        /// Gets or set the CSS class for auto-linked URLs.
        /// </summary>
        public string UrlClass { get; set; }

        /// <summary>
        /// Gets or set the CSS class for auto-linked list URLs.
        /// </summary>
        public string ListClass { get; set; }

        /// <summary>
        /// Gets or set the CSS class for auto-linked username URLs.
        /// </summary>
        public string UsernameClass { get; set; }

        /// <summary>
        /// Gets or sets the CSS class for auto-linked hashtag URLs.
        /// </summary>
        public string HashtagClass { get; set; }

        /// <summary>
        /// Gets or sets the CSS class for auto-linked cashtag URLs,
        /// </summary>
        public string CashtagClass { get; set; }

        /// <summary>
        /// Gets or sets the href value for username links (to which the username will be appended).
        /// </summary>
        public string UsernameUrlBase { get; set; }

        /// <summary>
        /// Gets or sets the href value for list links (to which the username/list will be appended).
        /// </summary>
        public string ListUrlBase { get; set; }

        /// <summary>
        /// Gets or sets the href value for hashtag links (to which the hashtag will be appended).
        /// </summary>
        public string HashtagUrlBase { get; set; }

        /// <summary>
        /// Gets or sets the href value for cashtag links (to which the cashtag will be appended).
        /// </summary>
        public string CashtagUrlBase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InvisibleTagAttrs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the current URL links will include rel="nofollow" (true by default).
        /// </summary>
        public bool NoFollow { get; set; }

        /// <summary>
        /// Sets a value indicating whether the at mark '@' should be included in the link (false by default).
        /// </summary>
        public bool UsernameIncludeSymbol { protected get; set; }

        /// <summary>
        /// Sets HTML tag to be applied around #/@/# symbols in hashtags/usernames/lists/cashtag. The tag should be without brackets e.g., "b" or "s".
        /// </summary>
        public string SymbolTag { protected get; set; }

        /// <summary>
        /// Set HTML tag to be applied around text part of hashtags/usernames/lists/cashtag. The tag should be without brackets e.g., "b" or "s".
        /// </summary>
        public string TextWithSymbolTag { protected get; set; }

        /// <summary>
        /// Sets the value of the target attribute in auto-linked URLs e.g., "_blank"
        /// </summary>
        public string UrlTarget { protected get; set; }

        /// <summary>
        /// Sets a modifier to modify attributes of a link based on an entity.
        /// </summary>
        public ILinkAttributeModifier LinkAttributeModifier { protected get; set; }

        /// <summary>
        /// Sets a modifier to modify text of a link based on an entity.
        /// </summary>
        public ILinkTextModifier LinkTextModifier { protected get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Autolink"/> class.
        /// </summary>
        public Autolink()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Autolink"/> class.
        /// </summary>
        /// <param name="linkURLWithoutProtocol">Whether or not to link urls without a protocol</param>
        public Autolink(bool linkURLWithoutProtocol)
        {
            UrlClass = null;
            ListClass = DEFAULT_LIST_CLASS;
            UsernameClass = DEFAULT_USERNAME_CLASS;
            HashtagClass = DEFAULT_HASHTAG_CLASS;
            CashtagClass = DEFAULT_CASHTAG_CLASS;
            UsernameUrlBase = DEFAULT_USERNAME_URL_BASE;
            ListUrlBase = DEFAULT_LIST_URL_BASE;
            HashtagUrlBase = DEFAULT_HASHTAG_URL_BASE;
            CashtagUrlBase = DEFAULT_CASHTAG_URL_BASE;
            InvisibleTagAttrs = DEFAULT_INVISIBLE_TAG_ATTRS;
            NoFollow = true;
            __Extractor = new Extractor { ExtractURLWithoutProtocol = linkURLWithoutProtocol };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string EscapeBrackets(string text)
        {
            int len = text.Length;
            if (len == 0)
                return text;

            StringBuilder sb = new StringBuilder(len + 16);
            for (int i = 0; i < len; ++i)
            {
                char c = text[i];
                if (c == '>')
                    sb.Append("&gt;");
                else if (c == '<')
                    sb.Append("&lt;");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string EscapeHTML(string text)
        {
            StringBuilder builder = new StringBuilder(text.Length * 2);
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                switch (c)
                {
                    case '&': builder.Append("&amp;"); break;
                    case '>': builder.Append("&gt;"); break;
                    case '<': builder.Append("&lt;"); break;
                    case '"': builder.Append("&quot;"); break;
                    case '\'': builder.Append("&#39;"); break;
                    default: builder.Append(c); break;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="text"></param>
        /// <param name="attributes"></param>
        /// <param name="builder"></param>
        public void LinkToText(Extractor.Entity entity, string text, IDictionary<string, string> attributes, StringBuilder builder)
        {
            if (NoFollow)
            {
                attributes["rel"] = "nofollow";
            }
            if (LinkAttributeModifier != null)
            {
                LinkAttributeModifier.Modify(entity, attributes);
            }
            if (LinkTextModifier != null)
            {
                text = LinkTextModifier.Modify(entity, text);
            }
            // append <a> tag
            builder.Append("<a");
            foreach (var entry in attributes)
            {
                builder.Append(" ").Append(EscapeHTML(entry.Key)).Append("=\"").Append(EscapeHTML(entry.Value)).Append("\"");
            }
            builder.Append(">").Append(text).Append("</a>");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="symbol"></param>
        /// <param name="text"></param>
        /// <param name="attributes"></param>
        /// <param name="builder"></param>
        public void LinkToTextWithSymbol(Extractor.Entity entity, string symbol, string text, IDictionary<string, string> attributes, StringBuilder builder)
        {
            string taggedSymbol = string.IsNullOrWhiteSpace(SymbolTag) ? symbol : string.Format("<{0}>{1}</{0}>", SymbolTag, symbol);
            text = EscapeHTML(text);
            string taggedText = string.IsNullOrWhiteSpace(TextWithSymbolTag) ? text : string.Format("<{0}>{1}</{0}>", TextWithSymbolTag, text);
            bool includeSymbol = UsernameIncludeSymbol || !Regex.AT_SIGNS.IsMatch(symbol);

            if (includeSymbol)
            {
                LinkToText(entity, taggedSymbol.ToString() + taggedText, attributes, builder);
            }
            else
            {
                builder.Append(taggedSymbol);
                LinkToText(entity, taggedText, attributes, builder);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="text"></param>
        /// <param name="builder"></param>
        public void LinkToHashtag(Extractor.Entity entity, string text, StringBuilder builder)
        {
            // Get the original hash char from text as it could be a full-width char.
            string hashChar = text.Substring(entity.Start, 1);
            string hashtag = entity.Value;

            IDictionary<string, string> attrs = new Dictionary<string, string>();
            attrs["href"] = HashtagUrlBase + hashtag;
            attrs["title"] = "#" + hashtag;

            if (Regex.RTL_CHARACTERS.IsMatch(text))
            {
                attrs["class"] = HashtagClass + " rtl";
            }
            else
            {
                attrs["class"] = HashtagClass;
            }

            LinkToTextWithSymbol(entity, hashChar, hashtag, attrs, builder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="text"></param>
        /// <param name="builder"></param>
        public void LinkToCashtag(Extractor.Entity entity, string text, StringBuilder builder)
        {
            string cashtag = entity.Value;

            IDictionary<string, string> attrs = new Dictionary<string, string>();
            attrs["href"] = CashtagUrlBase + cashtag;
            attrs["title"] = "$" + cashtag;
            attrs["class"] = CashtagClass;

            LinkToTextWithSymbol(entity, "$", cashtag, attrs, builder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="text"></param>
        /// <param name="builder"></param>
        public void LinkToMentionAndList(Extractor.Entity entity, string text, StringBuilder builder)
        {
            string mention = entity.Value;
            // Get the original at char from text as it could be a full-width char.
            string atChar = text.Substring(entity.Start, 1);

            IDictionary<string, string> attrs = new Dictionary<string, string>();
            if (entity.ListSlug != null)
            {
                mention += entity.ListSlug;
                attrs["class"] = ListClass;
                attrs["href"] = ListUrlBase + mention;
            }
            else
            {
                attrs["class"] = UsernameClass;
                attrs["href"] = UsernameUrlBase + mention;
            }

            LinkToTextWithSymbol(entity, atChar, mention, attrs, builder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="text"></param>
        /// <param name="builder"></param>
        public void LinkToURL(Extractor.Entity entity, string text, StringBuilder builder)
        {
            string url = entity.Value;
            string linkText = EscapeHTML(url);

            if (entity.DisplayURL != null && entity.ExpandedURL != null)
            {
                // Goal: If a user copies and pastes a tweet containing t.co'ed link, the resulting paste
                // should contain the full original URL (expanded_url), not the display URL.
                //
                // Method: Whenever possible, we actually emit HTML that contains expanded_url, and use
                // font-size:0 to hide those parts that should not be displayed (because they are not part of display_url).
                // Elements with font-size:0 get copied even though they are not visible.
                // Note that display:none doesn't work here. Elements with display:none don't get copied.
                //
                // Additionally, we want to *display* ellipses, but we don't want them copied.  To make this happen we
                // wrap the ellipses in a tco-ellipsis class and provide an onCopy handler that sets display:none on
                // everything with the tco-ellipsis class.
                //
                // As an example: The user tweets "hi http://longdomainname.com/foo"
                // This gets shortened to "hi http://t.co/xyzabc", with display_url = "…nname.com/foo"
                // This will get rendered as:
                // <span class='tco-ellipsis'> <!-- This stuff should get displayed but not copied -->
                //   …
                //   <!-- There's a chance the onCopy event handler might not fire. In case that happens,
                //        we include an &nbsp; here so that the … doesn't bump up against the URL and ruin it.
                //        The &nbsp; is inside the tco-ellipsis span so that when the onCopy handler *does*
                //        fire, it doesn't get copied.  Otherwise the copied text would have two spaces in a row,
                //        e.g. "hi  http://longdomainname.com/foo".
                //   <span style='font-size:0'>&nbsp;</span>
                // </span>
                // <span style='font-size:0'>  <!-- This stuff should get copied but not displayed -->
                //   http://longdomai
                // </span>
                // <span class='js-display-url'> <!-- This stuff should get displayed *and* copied -->
                //   nname.com/foo
                // </span>
                // <span class='tco-ellipsis'> <!-- This stuff should get displayed but not copied -->
                //   <span style='font-size:0'>&nbsp;</span>
                //   …
                // </span>
                //
                // Exception: pic.twitter.com images, for which expandedUrl = "https://twitter.com/#!/username/status/1234/photo/1
                // For those URLs, display_url is not a substring of expanded_url, so we don't do anything special to render the elided parts.
                // For a pic.twitter.com URL, the only elided part will be the "https://", so this is fine.
                string displayURLSansEllipses = entity.DisplayURL.Replace("…", "");
                int diplayURLIndexInExpandedURL = entity.ExpandedURL.IndexOf(displayURLSansEllipses);
                if (diplayURLIndexInExpandedURL != -1)
                {
                    string beforeDisplayURL = entity.ExpandedURL.Substring(0, diplayURLIndexInExpandedURL);
                    string afterDisplayURL = entity.ExpandedURL.Substring(diplayURLIndexInExpandedURL + displayURLSansEllipses.Length);
                    string precedingEllipsis = entity.DisplayURL.StartsWith("…") ? "…" : "";
                    string followingEllipsis = entity.DisplayURL.EndsWith("…") ? "…" : "";
                    string invisibleSpan = "<span " + InvisibleTagAttrs + ">";

                    StringBuilder sb = new StringBuilder("<span class='tco-ellipsis'>");
                    sb.Append(precedingEllipsis);
                    sb.Append(invisibleSpan).Append("&nbsp;</span></span>");
                    sb.Append(invisibleSpan).Append(EscapeHTML(beforeDisplayURL)).Append("</span>");
                    sb.Append("<span class='js-display-url'>").Append(EscapeHTML(displayURLSansEllipses)).Append("</span>");
                    sb.Append(invisibleSpan).Append(EscapeHTML(afterDisplayURL)).Append("</span>");
                    sb.Append("<span class='tco-ellipsis'>").Append(invisibleSpan).Append("&nbsp;</span>").Append(followingEllipsis).Append("</span>");

                    linkText = sb.ToString();
                }
                else
                {
                    linkText = entity.DisplayURL;
                }
            }

            IDictionary<string, string> attrs = new Dictionary<string, string>();
            attrs["href"] = url;

            if (!string.IsNullOrWhiteSpace(entity.DisplayURL) && !string.IsNullOrWhiteSpace(entity.ExpandedURL))
            {
                attrs["title"] = entity.ExpandedURL;
            }

            if (!string.IsNullOrWhiteSpace(UrlClass))
            {
                attrs["class"] = UrlClass;
            }

            if (!string.IsNullOrWhiteSpace(UrlTarget))
            {
                attrs["target"] = UrlTarget;
            }

            LinkToText(entity, linkText, attrs, builder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public string AutoLinkEntities(string text, List<Extractor.Entity> entities)
        {
            StringBuilder builder = new StringBuilder(text.Length * 2);
            int beginIndex = 0;

            foreach (Extractor.Entity entity in entities)
            {
                builder.Append(text.Substring(beginIndex, entity.Start - beginIndex));

                switch (entity.Type)
                {
                    case Extractor.EntityType.Url:
                        LinkToURL(entity, text, builder);
                        break;
                    case Extractor.EntityType.Hashtag:
                        LinkToHashtag(entity, text, builder);
                        break;
                    case Extractor.EntityType.Mention:
                        LinkToMentionAndList(entity, text, builder);
                        break;
                    case Extractor.EntityType.Cashtag:
                        LinkToCashtag(entity, text, builder);
                        break;
                }
                beginIndex = entity.End;
            }
            builder.Append(text.Substring(beginIndex, text.Length - beginIndex));

            return builder.ToString();
        }

        /// <summary>
        /// Auto-link hashtags, URLs, usernames and lists.
        /// </summary>
        /// <param name="text">text of the Tweet to auto-link</param>
        /// <returns>text with auto-link HTML added</returns>
        public string AutoLink(string text)
        {
            text = EscapeBrackets(text);

            // extract entities
            List<Extractor.Entity> entities = __Extractor.ExtractEntitiesWithIndices(text);
            return AutoLinkEntities(text, entities);
        }

        /// <summary>
        /// Auto-link the @username and @username/list references in the provided text. 
        /// Links to @username references will  have the usernameClass CSS classes added. 
        /// Links to @username/list references will have the listClass CSS class  added.
        /// </summary>
        /// <param name="text">text of the Tweet to auto-link</param>
        /// <returns>text with auto-link HTML added</returns>
        public string AutoLinkUsernamesAndLists(string text)
        {
            return AutoLinkEntities(text, __Extractor.ExtractMentionsOrListsWithIndices(text));
        }

        /// <summary>
        /// Auto-link #hashtag references in the provided Tweet text. The #hashtag links will have the hashtagClass CSS class added.
        /// </summary>
        /// <param name="text">text of the Tweet to auto-link</param>
        /// <returns>text with auto-link HTML added</returns>
        public string AutoLinkHashtags(string text)
        {
            return AutoLinkEntities(text, __Extractor.ExtractHashtagsWithIndices(text));
        }

        /// <summary>
        /// Auto-link URLs in the Tweet text provided. This only auto-links URLs with protocol.
        /// </summary>
        /// <param name="text">text of the Tweet to auto-link</param>
        /// <returns>text with auto-link HTML added</returns>
        public string AutoLinkURLs(string text)
        {
            return AutoLinkEntities(text, __Extractor.ExtractURLsWithIndices(text));
        }

        /// <summary>
        /// Auto-link $cashtag references in the provided Tweet text. The $cashtag links will have the cashtagClass CSS class added.
        /// </summary>
        /// <param name="text">text of the Tweet to auto-link</param>
        /// <returns>text with auto-link HTML added.</returns>
        public string AutoLinkCashtags(string text)
        {
            return AutoLinkEntities(text, __Extractor.ExtractCashtagsWithIndices(text));
        }

        /// <summary>
        /// 
        /// </summary>
        public interface ILinkAttributeModifier
        {
            void Modify(Extractor.Entity entity, IDictionary<string, string> attributes);
        };

        /// <summary>
        /// 
        /// </summary>
        public interface ILinkTextModifier
        {
            string Modify(Extractor.Entity entity, string text);
        }
    }
}