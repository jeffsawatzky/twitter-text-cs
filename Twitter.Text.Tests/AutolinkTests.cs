using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Twitter.Text;
using Pattern = System.Text.RegularExpressions.Regex;

namespace Twitter.Text
{
    [TestFixture]
    public class AutolinkTests : ConformanceTests
    {
        private static Autolink autolink;

        public AutolinkTests()
            : base("autolink.yml")
        {
        }

        [SetUp]
        public void SetUp()
        {
            autolink = new Autolink();
            autolink.NoFollow = false;
        }

        #region Conformance

        [Test]
        public void AutolinkUsernamesTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<string>("usernames"))
            {
                string actual = string.Empty;
                try
                {

                    actual = autolink.AutoLinkUsernamesAndLists(test.text);
                    Assert.AreEqual(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(string.Format("\n{0}: {1}\n\tExpected: {2}\n\t  Actual: {3}", test.description, test.text, test.expected, actual));
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkListsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<string>("lists"))
            {
                string actual = string.Empty;
                try
                {
                    actual = autolink.AutoLinkUsernamesAndLists(test.text);
                    Assert.AreEqual(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(string.Format("\n{0}: {1}\n\tExpected: {2}\n\t  Actual: {3}", test.description, test.text, test.expected, actual));
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkHashtagsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<string>("hashtags"))
            {
                string actual = string.Empty;
                try
                {
                    actual = autolink.AutoLinkHashtags(test.text);
                    Assert.AreEqual(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(string.Format("\n{0}: {1}\n\tExpected: {2}\n\t  Actual: {3}", test.description, test.text, test.expected, actual));
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkUrlsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<string>("urls"))
            {
                string actual = string.Empty;
                try
                {
                    actual = autolink.AutoLinkURLs(test.text);
                    Assert.AreEqual(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(string.Format("\n{0}: {1}\n\tExpected: {2}\n\t  Actual: {3}", test.description, test.text, test.expected, actual));
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkCashtagsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<string>("cashtags"))
            {
                string actual = string.Empty;
                try
                {
                    actual = autolink.AutoLinkCashtags(test.text);
                    Assert.AreEqual(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(string.Format("\n{0}: {1}\n\tExpected: {2}\n\t  Actual: {3}", test.description, test.text, test.expected, actual));
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkAllTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<string>("all"))
            {
                string actual = string.Empty;
                try
                {
                    actual = autolink.AutoLink(test.text);
                    Assert.AreEqual(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(string.Format("\n{0}: {1}\n\tExpected: {2}\n\t  Actual: {3}", test.description, test.text, test.expected, actual));
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkJsonTest()
        {
            Extractor extractor = new Extractor();
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<string>("json"))
            {
                string actual = string.Empty;
                try
                {
                    List<Extractor.Entity> entities = extractor.ExtractEntitiesWithIndices(test.text);
                    foreach (Extractor.Entity entity in entities)
                    {
                        if (entity.Type == Extractor.EntityType.Url)
                        {
                            entity.DisplayURL = "twitter.com";
                            entity.ExpandedURL = "http://twitter.com/";
                        }
                    }
                    actual = autolink.AutoLinkEntities(test.text, entities);
                    Assert.AreEqual(test.expected, actual);
                }
                catch (Exception)
                {
                    failures.Add(string.Format("\n{0}: {1}\n\tExpected: {2}\n\t  Actual: {3}", test.description, test.text, test.expected, actual));
                }
            }
            if (failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        #endregion

        #region Additional

        [Test]
        public void NoFollowByDefaultTest()
        {
            autolink = new Autolink();
            String tweet = "This has a #hashtag";
            String expected = "This has a <a href=\"https://twitter.com/#!/search?q=%23hashtag\" title=\"#hashtag\" class=\"tweet-url hashtag\" rel=\"nofollow\">#hashtag</a>";
            AssertAutolink(expected, autolink.AutoLinkHashtags(tweet));
        }

        [Test]
        public void NoFollowDisabledTest()
        {
            String tweet = "This has a #hashtag";
            String expected = "This has a <a href=\"https://twitter.com/#!/search?q=%23hashtag\" title=\"#hashtag\" class=\"tweet-url hashtag\">#hashtag</a>";
            AssertAutolink(expected, autolink.AutoLinkHashtags(tweet));
        }

        /** See Also: http://github.com/mzsanford/twitter-text-rb/issues#issue/5 */
        [Test]
        public void BlogspotWithDashTest()
        {
            String tweet = "Url: http://samsoum-us.blogspot.com/2010/05/la-censure-nuit-limage-de-notre-pays.html";
            String expected = "Url: <a href=\"http://samsoum-us.blogspot.com/2010/05/la-censure-nuit-limage-de-notre-pays.html\">http://samsoum-us.blogspot.com/2010/05/la-censure-nuit-limage-de-notre-pays.html</a>";
            AssertAutolink(expected, autolink.AutoLinkURLs(tweet));
        }

        /** See also: https://github.com/mzsanford/twitter-text-java/issues/8 */
        [Test]
        public void URLWithDollarThatLooksLikeARegexTest()
        {
            String tweet = "Url: http://example.com/$ABC";
            String expected = "Url: <a href=\"http://example.com/$ABC\">http://example.com/$ABC</a>";
            AssertAutolink(expected, autolink.AutoLinkURLs(tweet));
        }

        [Test]
        public void URLWithoutProtocolTest()
        {
            String tweet = "Url: www.twitter.com http://www.twitter.com";
            String expected = "Url: www.twitter.com <a href=\"http://www.twitter.com\">http://www.twitter.com</a>";
            AssertAutolink(expected, autolink.AutoLinkURLs(tweet));
        }

        [Test]
        public void URLEntitiesTest()
        {
            autolink.NoFollow = true;
            Extractor.Entity entity = new Extractor.Entity(0, 19, "http://t.co/0JG5Mcq", Extractor.EntityType.Url);
            entity.DisplayURL = "blog.twitter.com/2011/05/twitte…";
            entity.ExpandedURL = "http://blog.twitter.com/2011/05/twitter-for-mac-update.html";
            List<Extractor.Entity> entities = new List<Extractor.Entity>();
            entities.Add(entity);
            String tweet = "http://t.co/0JG5Mcq";
            String expected = "<a href=\"http://t.co/0JG5Mcq\" title=\"http://blog.twitter.com/2011/05/twitter-for-mac-update.html\" rel=\"nofollow\"><span class='tco-ellipsis'><span style='position:absolute;left:-9999px;'>&nbsp;</span></span><span style='position:absolute;left:-9999px;'>http://</span><span class='js-display-url'>blog.twitter.com/2011/05/twitte</span><span style='position:absolute;left:-9999px;'>r-for-mac-update.html</span><span class='tco-ellipsis'><span style='position:absolute;left:-9999px;'>&nbsp;</span>…</span></a>";

            AssertAutolink(expected, autolink.AutoLinkEntities(tweet, entities));
        }

        [Test]
        public void WithAngleBracketsTest()
        {
            String tweet = "(Debugging) <3 #idol2011";
            String expected = "(Debugging) &lt;3 <a href=\"https://twitter.com/#!/search?q=%23idol2011\" title=\"#idol2011\" class=\"tweet-url hashtag\">#idol2011</a>";
            AssertAutolink(expected, autolink.AutoLink(tweet));

            tweet = "<link rel='true'>http://example.com</link>";
            expected = "<link rel='true'><a href=\"http://example.com\">http://example.com</a></link>";
            AssertAutolink(expected, autolink.AutoLinkURLs(tweet));
        }

        [Test]
        public void UsernameIncludeSymbolTest()
        {
            autolink.NoFollow = true;
            autolink.UsernameIncludeSymbol = true;
            String tweet = "Testing @mention and @mention/list";
            String expected = "Testing <a class=\"tweet-url username\" href=\"https://twitter.com/mention\" rel=\"nofollow\">@mention</a> and <a class=\"tweet-url list-slug\" href=\"https://twitter.com/mention/list\" rel=\"nofollow\">@mention/list</a>";
            AssertAutolink(expected, autolink.AutoLink(tweet));
        }

        [Test]
        public void UrlClassTest()
        {
            String tweet = "http://twitter.com";
            String expected = "<a href=\"http://twitter.com\">http://twitter.com</a>";
            AssertAutolink(expected, autolink.AutoLink(tweet));

            autolink.UrlClass = "testClass";
            expected = "<a href=\"http://twitter.com\" class=\"testClass\">http://twitter.com</a>";
            AssertAutolink(expected, autolink.AutoLink(tweet));

            tweet = "#hash @tw";
            String result = autolink.AutoLink(tweet);
            Assert.True(result.Contains("class=\"" + Autolink.DEFAULT_HASHTAG_CLASS + "\""));
            Assert.True(result.Contains("class=\"" + Autolink.DEFAULT_USERNAME_CLASS + "\""));
            Assert.False(result.Contains("class=\"testClass\""));
        }

        [Test]
        public void SymbolTagTest()
        {
            autolink.SymbolTag = "s";
            autolink.TextWithSymbolTag = "b";

            String tweet = "#hash";
            String expected = "<a href=\"https://twitter.com/#!/search?q=%23hash\" title=\"#hash\" class=\"tweet-url hashtag\"><s>#</s><b>hash</b></a>";
            AssertAutolink(expected, autolink.AutoLink(tweet));

            tweet = "@mention";
            expected = "<s>@</s><a class=\"tweet-url username\" href=\"https://twitter.com/mention\"><b>mention</b></a>";
            AssertAutolink(expected, autolink.AutoLink(tweet));

            autolink.UsernameIncludeSymbol = true;
            expected = "<a class=\"tweet-url username\" href=\"https://twitter.com/mention\"><s>@</s><b>mention</b></a>";
            AssertAutolink(expected, autolink.AutoLink(tweet));
        }

        [Test]
        public void UrlTargetTest()
        {
            autolink.UrlTarget = "_blank";

            String tweet = "http://test.com";
            String expected = "<a href=\"http://test.com\" target=\"_blank\">http://test.com</a>";
            AssertAutolink(expected, autolink.AutoLink(tweet));
        }

        private void AssertAutolink(String expected, String linked)
        {
            Assert.AreEqual(expected, linked, "Autolinked text should equal the input");
        }

        #endregion
    }
}