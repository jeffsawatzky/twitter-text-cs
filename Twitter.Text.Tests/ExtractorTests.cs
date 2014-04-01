using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Twitter.Text
{
    [TestFixture]
    public class ExtractorTests : ConformanceTests
    {
        private Extractor extractor = new Extractor();

        public ExtractorTests()
            : base("extract.yml")
        {
        }

        #region Conformance

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ExtractMentionsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<dynamic>("mentions"))
            {
                try
                {
                    List<string> actual = extractor.ExtractMentionedScreennames(test.text);
                    CollectionAssert.AreEquivalent(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(test.description + ": " + test.text);
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ExtractMentionsWithIndicesTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<dynamic>("mentions_with_indices"))
            {
                try
                {
                    List<Extractor.Entity> actual = extractor.ExtractMentionedScreennamesWithIndices(test.text);
                    for (int i = 0; i < actual.Count; i++)
                    {
                        Extractor.Entity entity = actual[i];
                        Assert.AreEqual(test.expected[i].screen_name, entity.Value);
                        Assert.AreEqual(test.expected[i].indices[0], entity.Start);
                        Assert.AreEqual(test.expected[i].indices[1], entity.End);
                    }
                }
                catch (Exception)
                {
                    failures.Add(test.description + ": " + test.text);
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ExtractMentionsOrListsWithIndicesTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<dynamic>("mentions_or_lists_with_indices"))
            {
                try
                {
                    List<Extractor.Entity> actual = extractor.ExtractMentionsOrListsWithIndices(test.text);
                    for (int i = 0; i < actual.Count; i++)
                    {
                        Extractor.Entity entity = actual[i];
                        Assert.AreEqual(test.expected[i].screen_name, entity.Value);
                        Assert.AreEqual(test.expected[i].list_slug, entity.ListSlug);
                        Assert.AreEqual(test.expected[i].indices[0], entity.Start);
                        Assert.AreEqual(test.expected[i].indices[1], entity.End);
                    }
                }
                catch (Exception)
                {
                    failures.Add(test.description + ": " + test.text);
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ExtractRepliesTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<dynamic>("replies"))
            {
                try
                {
                    string actual = extractor.ExtractReplyScreenname(test.text);
                    Assert.AreEqual(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(test.description + ": " + test.text);
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractUrlsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<dynamic>("urls"))
            {
                try
                {
                    List<string> actual = extractor.ExtractURLs(test.text);
                    CollectionAssert.AreEquivalent(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(test.description + ": " + test.text);
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractUrlsWithIndicesTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<dynamic>("urls_with_indices"))
            {
                try
                {
                    List<Extractor.Entity> actual = extractor.ExtractURLsWithIndices(test.text);
                    for (int i = 0; i < actual.Count; i++)
                    {
                        Extractor.Entity entity = actual[i];
                        Assert.AreEqual(test.expected[i].url, entity.Value);
                        Assert.AreEqual(test.expected[i].indices[0], entity.Start);
                        Assert.AreEqual(test.expected[i].indices[1], entity.End);
                    }
                }
                catch (Exception)
                {
                    failures.Add(test.description + ": " + test.text);
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractHashtagsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<dynamic>("hashtags"))
            {
                try
                {
                    List<string> actual = extractor.ExtractHashtags(test.text);
                    CollectionAssert.AreEquivalent(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(test.description + ": " + test.text);
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractHashtagsWithIndicesTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<dynamic>("hashtags_with_indices"))
            {
                try
                {
                    List<Extractor.Entity> actual = extractor.ExtractHashtagsWithIndices(test.text);
                    for (int i = 0; i < actual.Count; i++)
                    {
                        Extractor.Entity entity = actual[i];
                        Assert.AreEqual(test.expected[i].hashtag, entity.Value);
                        Assert.AreEqual(test.expected[i].indices[0], entity.Start);
                        Assert.AreEqual(test.expected[i].indices[1], entity.End);
                    }
                }
                catch (Exception)
                {
                    failures.Add(test.description + ": " + test.text);
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractCashtagsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<dynamic>("cashtags"))
            {
                try
                {
                    List<string> actual = extractor.ExtractCashtags(test.text);
                    CollectionAssert.AreEquivalent(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(test.description + ": " + test.text);
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractCashtagsWithIndicesTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<dynamic>("cashtags_with_indices"))
            {
                try
                {
                    List<Extractor.Entity> actual = extractor.ExtractCashtagsWithIndices(test.text);
                    for (int i = 0; i < actual.Count; i++)
                    {
                        Extractor.Entity entity = actual[i];
                        Assert.AreEqual(test.expected[i].cashtag, entity.Value);
                        Assert.AreEqual(test.expected[i].indices[0], entity.Start);
                        Assert.AreEqual(test.expected[i].indices[1], entity.End);
                    }
                }
                catch (Exception)
                {
                    failures.Add(test.description + ": " + test.text);
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        #endregion

        #region Additional

        #region URL Tests

        [Test]
        public void UrlWithIndicesTest()
        {
            List<Extractor.Entity> extracted = extractor.ExtractURLsWithIndices("http://t.co url https://www.twitter.com ");
            Assert.AreEqual(extracted[0].Start, 0);
            Assert.AreEqual(extracted[0].End, 11);
            Assert.AreEqual(extracted[1].Start, 16);
            Assert.AreEqual(extracted[1].End, 39);
        }

        [Test]
        public void UrlWithoutProtocolTest()
        {
            String text = "www.twitter.com, www.yahoo.co.jp, t.co/blahblah, www.poloshirts.uk.com";
            AssertList("Failed to extract URLs without protocol",
                new String[] { "www.twitter.com", "www.yahoo.co.jp", "t.co/blahblah", "www.poloshirts.uk.com" },
                extractor.ExtractURLs(text));

            List<Extractor.Entity> extracted = extractor.ExtractURLsWithIndices(text);
            Assert.AreEqual(extracted[0].Start, 0);
            Assert.AreEqual(extracted[0].End, 15);
            Assert.AreEqual(extracted[1].Start, 17);
            Assert.AreEqual(extracted[1].End, 32);
            Assert.AreEqual(extracted[2].Start, 34);
            Assert.AreEqual(extracted[2].End, 47);

            extractor.ExtractURLWithoutProtocol = false;
            Assert.True(extractor.ExtractURLs(text).Count == 0, "Should not extract URLs w/o protocol");
            extractor.ExtractURLWithoutProtocol = true;
        }

        [Test]
        public void URLFollowedByPunctuationsTest()
        {
            String text = "http://games.aarp.org/games/mahjongg-dimensions.aspx!!!!!!";
            AssertList("Failed to extract URLs followed by punctuations",
                new String[] { "http://games.aarp.org/games/mahjongg-dimensions.aspx" },
                extractor.ExtractURLs(text));
        }

        [Test]
        public void UrlWithPunctuationTest()
        {
            String[] urls = new String[] {
               "http://www.foo.com/foo/path-with-period./",
               "http://www.foo.org.za/foo/bar/688.1",
               "http://www.foo.com/bar-path/some.stm?param1=foo;param2=P1|0||P2|0",
               "http://foo.com/bar/123/foo_&_bar/",
               "http://foo.com/bar(test)bar(test)bar(test)",
               "www.foo.com/foo/path-with-period./",
               "www.foo.org.za/foo/bar/688.1",
               "www.foo.com/bar-path/some.stm?param1=foo;param2=P1|0||P2|0",
               "foo.com/bar/123/foo_&_bar/"
             };

            foreach (String url in urls)
            {
                List<string> extractedUrls = extractor.ExtractURLs(url);
                Assert.AreEqual(url, extractedUrls[0]);
            }
        }

        [Test]
        public void UrlWithSupplementaryCharactersTest()
        {
            // insert U+10400 before " http://twitter.com"
            String text = "\U00010400 http://twitter.com \U00010400 http://twitter.com";

            // count U+10400 as 2 characters (as in UTF-16)
            List<Extractor.Entity> extracted = extractor.ExtractURLsWithIndices(text);
            Assert.AreEqual(extracted.Count, 2);
            Assert.AreEqual(extracted[0].Value, "http://twitter.com");
            Assert.AreEqual(extracted[0].Start, 3);
            Assert.AreEqual(extracted[0].End, 21);
            Assert.AreEqual(extracted[1].Value, "http://twitter.com");
            Assert.AreEqual(extracted[1].Start, 25);
            Assert.AreEqual(extracted[1].End, 43);
        }

        #endregion

        #region Reply Tests

        [Test]
        public void ReplyAtTheBeginningTest()
        {
            String extracted = extractor.ExtractReplyScreenname("@user reply");
            Assert.AreEqual("user", extracted, "Failed to extract reply at the start");
        }

        [Test]
        public void ReplyWithLeadingSpaceTest()
        {
            String extracted = extractor.ExtractReplyScreenname(" @user reply");
            Assert.AreEqual("user", extracted, "Failed to extract reply with leading space");
        }

        #endregion

        #region Mention Tests

        [Test]
        public void MentionAtTheBeginningTest()
        {
            List<String> extracted = extractor.ExtractMentionedScreennames("@user mention");
            AssertList("Failed to extract mention at the beginning", new String[] { "user" }, extracted);
        }

        [Test]
        public void MentionWithLeadingSpaceTest()
        {
            List<String> extracted = extractor.ExtractMentionedScreennames(" @user mention");
            AssertList("Failed to extract mention with leading space", new String[] { "user" }, extracted);
        }

        [Test]
        public void MentionInMidTextTest()
        {
            List<String> extracted = extractor.ExtractMentionedScreennames("mention @user here");
            AssertList("Failed to extract mention in mid text", new String[] { "user" }, extracted);
        }

        [Test]
        public void MultipleMentionsTest()
        {
            List<String> extracted = extractor.ExtractMentionedScreennames("mention @user1 here and @user2 here");
            AssertList("Failed to extract multiple mentioned users", new String[] { "user1", "user2" }, extracted);
        }

        [Test]
        public void MentionWithIndicesTest()
        {
            List<Extractor.Entity> extracted = extractor.ExtractMentionedScreennamesWithIndices(" @user1 mention @user2 here @user3 ");
            Assert.AreEqual(extracted.Count(), 3);
            Assert.AreEqual(extracted[0].Start, 1);
            Assert.AreEqual(extracted[0].End, 7);
            Assert.AreEqual(extracted[1].Start, 16);
            Assert.AreEqual(extracted[1].End, 22);
            Assert.AreEqual(extracted[2].Start, 28);
            Assert.AreEqual(extracted[2].End, 34);
        }

        [Test]
        public void MentionWithSupplementaryCharactersTest()
        {
            // insert U+10400 before " @mention"
            String text = "\U00010400 @mention \U00010400 @mention";

            // count U+10400 as 2 characters (as in UTF-16)
            List<Extractor.Entity> extracted = extractor.ExtractMentionedScreennamesWithIndices(text);
            Assert.AreEqual(extracted.Count(), 2);
            Assert.AreEqual(extracted[0].Value, "mention");
            Assert.AreEqual(extracted[0].Start, 3);
            Assert.AreEqual(extracted[0].End, 11);
            Assert.AreEqual(extracted[1].Value, "mention");
            Assert.AreEqual(extracted[1].Start, 15);
            Assert.AreEqual(extracted[1].End, 23);
        }

        #endregion

        #region Hashtag Test

        [Test]
        public void HashtagAtTheBeginningTest()
        {
            List<String> extracted = extractor.ExtractHashtags("#hashtag mention");
            AssertList("Failed to extract hashtag at the beginning", new String[] { "hashtag" }, extracted);
        }

        [Test]
        public void HashtagWithLeadingSpaceTest()
        {
            List<String> extracted = extractor.ExtractHashtags(" #hashtag mention");
            AssertList("Failed to extract hashtag with leading space", new String[] { "hashtag" }, extracted);
        }

        [Test]
        public void HashtagInMidTextTest()
        {
            List<String> extracted = extractor.ExtractHashtags("mention #hashtag here");
            AssertList("Failed to extract hashtag in mid text", new String[] { "hashtag" }, extracted);
        }

        [Test]
        public void MultipleHashtagsTest()
        {
            List<String> extracted = extractor.ExtractHashtags("text #hashtag1 #hashtag2");
            AssertList("Failed to extract multiple hashtags", new String[] { "hashtag1", "hashtag2" }, extracted);
        }

        [Test]
        public void HashtagWithIndicesTest()
        {
            List<Extractor.Entity> extracted = extractor.ExtractHashtagsWithIndices(" #user1 mention #user2 here #user3 ");
            Assert.AreEqual(extracted.Count, 3);
            Assert.AreEqual(extracted[0].Start, 1);
            Assert.AreEqual(extracted[0].End, 7);
            Assert.AreEqual(extracted[1].Start, 16);
            Assert.AreEqual(extracted[1].End, 22);
            Assert.AreEqual(extracted[2].Start, 28);
            Assert.AreEqual(extracted[2].End, 34);
        }

        [Test]
        public void HashtagWithSupplementaryCharactersTest()
        {
            // insert U+10400 before " #hashtag"
            String text = "\U00010400 #hashtag \U00010400 #hashtag";

            // count U+10400 as 2 characters (as in UTF-16)
            List<Extractor.Entity> extracted = extractor.ExtractHashtagsWithIndices(text);
            Assert.AreEqual(extracted.Count, 2);
            Assert.AreEqual(extracted[0].Value, "hashtag");
            Assert.AreEqual(extracted[0].Start, 3);
            Assert.AreEqual(extracted[0].End, 11);
            Assert.AreEqual(extracted[1].Value, "hashtag");
            Assert.AreEqual(extracted[1].Start, 15);
            Assert.AreEqual(extracted[1].End, 23);
        }

        #endregion

        /// <summary>
        /// Helper method for asserting that the List of extracted Strings match the expected values.
        /// </summary>
        /// <param name="message">message to display on failure</param>
        /// <param name="expected">Array of Strings that were expected to be extracted</param>
        /// <param name="actual">List of Strings that were extracted</param>
        private void AssertList(String message, String[] expected, List<String> actual)
        {
            List<String> expectedList = new List<string>(expected);
            if (expectedList.Count() != actual.Count())
            {
                Assert.Fail(message + "\n\nExpected list and extracted list are differnt.Counts:\n" +
                "  Expected (" + expectedList.Count() + "): " + expectedList + "\n" +
                "  Actual   (" + actual.Count() + "): " + actual);
            }
            else
            {
                for (int i = 0; i < expectedList.Count(); i++)
                {
                    Assert.AreEqual(expectedList[i], actual[i]);
                }
            }
        }

        #endregion
    }
}