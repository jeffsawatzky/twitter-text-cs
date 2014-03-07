using System;
using System.Collections.Generic;
using System.Text;
using Twitter.Text.Extensions;

namespace Twitter.Text
{
    /// <summary>
    /// A class for adding HTML highlighting in Tweet text (such as would be returned from a Search)
    /// </summary>
    public class HitHighlighter
    {
        /// <summary>
        /// Default HTML tag for highlight hits
        /// </summary>
        public const String DEFAULT_HIGHLIGHT_TAG = "em";

        /// <summary>
        /// Get/set the current HTML tag used for phrase highlighting.
        /// </summary>
        public String HighlightTag { get; set; }

        /// <summary>
        /// Create a new HitHighlighter object.
        /// </summary>
        public HitHighlighter()
        {
            HighlightTag = DEFAULT_HIGHLIGHT_TAG;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<string> SplitTags(string text)
        {
            List<string> rv = new List<string>();
            string[] firstSplits = text.Split('<');
            string[] secondSplits;

            for (int i = 0; i < firstSplits.Length; ++i)
            {
                string split = firstSplits[i];
                if (string.IsNullOrEmpty(split))
                {
                    rv.Add(string.Empty);
                }
                else
                {
                    secondSplits = split.Split('>');
                    for (int j = 0; j < secondSplits.Length; ++j)
                    {
                        rv.Add(secondSplits[j]);
                    }
                }
            }

            return rv;
        }

        /// <summary>
        /// Surround the <code>hits</code> in the provided <code>text</code> with an HTML tag. This is used with offsets
        /// from the search API to support the highlighting of query terms.
        /// </summary>
        /// <param name="text">text of the Tweet to highlight</param>
        /// <param name="hits">A List of highlighting offsets (themselves lists of two elements)</param>
        /// <returns>text with highlight HTML added</returns>
        public String Highlight(String text, List<List<int>> hits)
        {
            if (string.IsNullOrWhiteSpace(text) || hits.Count == 0)
            {
                return text;
            }

            StringBuilder result = new StringBuilder(text.Length);

            string[] tags = new string[] { "<" + HighlightTag + ">", "</" + HighlightTag + ">" };
            List<string> chunks = SplitTags(text);
            int chunkIndex = 0;
            string chunk = chunks[chunkIndex];
            int prevChunksLen = 0;
            int chunkCursor = 0;
            bool startInChunk = false;

            List<int> flatHits = new List<int>();
            for (int i = 0; i < hits.Count; ++i)
            {
                for (int j = 0; j < hits[i].Count; ++j)
                {
                    flatHits.Add(hits[i][j]);
                }
            }

            for (int index = 0; index < flatHits.Count; ++index)
            {
                int hit = flatHits[index];
                string tag = tags[index % 2];
                bool placed = false;

                while (chunk != null && hit >= prevChunksLen + chunk.Length)
                {
                    result.Append(chunk.Slice(chunkCursor));
                    if (startInChunk && hit == prevChunksLen + chunk.Length)
                    {
                        result.Append(tag);
                        placed = true;
                    }

                    if (chunkIndex + 1 < chunks.Count && !string.IsNullOrEmpty(chunks[chunkIndex + 1]))
                    {
                        result.Append("<" + chunks[chunkIndex + 1] + ">");
                    }

                    prevChunksLen += chunk.Length;
                    chunkCursor = 0;
                    chunkIndex += 2;
                    chunk = (chunkIndex < chunks.Count ? chunks[chunkIndex] : null);
                    startInChunk = false;
                }

                if (!placed && chunk != null)
                {
                    int hitSpot = hit - prevChunksLen;
                    result.Append(chunk.Slice(chunkCursor, hitSpot) + tag);
                    chunkCursor = hitSpot;
                    if (index % 2 == 0)
                    {
                        startInChunk = true;
                    }
                    else
                    {
                        startInChunk = false;
                    }
                }
                else if (!placed)
                {
                    placed = true;
                    result.Append(tag);
                }
            }

            if (chunk != null)
            {
                if (chunkCursor < chunk.Length)
                {
                    result.Append(chunk.Slice(chunkCursor));
                }
                for (int index = chunkIndex + 1; index < chunks.Count; ++index)
                {
                    result.Append(index % 2 == 0 ? chunks[index] : "<" + chunks[index] + ">");
                }
            }

            return result.ToString();
        }
    }
}