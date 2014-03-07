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
    }
}