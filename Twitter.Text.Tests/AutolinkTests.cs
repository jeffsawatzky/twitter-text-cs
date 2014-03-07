using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Twitter.Text
{
    [TestFixture]
    public class AutolinkTests : ConformanceTests
    {
        private static Autolink autolink = new Autolink();

        public AutolinkTests()
            : base("autolink.yml")
        {
        }

        [SetUp]
        public void SetUp()
        {
            autolink.NoFollow = false;
        }

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
                        if (entity.Type == Extractor.EntityType.Url) {
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
    }
}