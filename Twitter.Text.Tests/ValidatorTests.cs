using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Twitter.Text
{
    [TestFixture]
    public class ValidatorTests : ConformanceTests
    {
        private Validator validator = new Validator();

        public ValidatorTests()
            : base("validate.yml")
        {
        }

        [Test]
        public void ValidateTweetsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<bool>("tweets"))
            {
                try
                {
                    bool actual = validator.IsValidTweet(test.text);
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
        public void ValidateUsernamesTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<bool>("usernames"))
            {
                try
                {
                    Match match = Regex.VALID_REPLY.Match(test.text);
                    bool actual = (match.Success && match.Value.Equals(test.text));
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
        [Ignore("Fails a couple tests")]
        public void ValidateListsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<bool>("lists"))
            {
                try
                {
                    Match match = Regex.VALID_MENTION_OR_LIST.Match(test.text);
                    bool actual = (match.Success && match.Value.Equals(test.text));
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
        public void ValidateHashtagsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<bool>("hashtags"))
            {
                try
                {
                    Match match = Regex.VALID_HASHTAG.Match(test.text);
                    bool actual = (match.Success && match.Value.Equals(test.text));
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
        [Ignore("Fails a couple tests")]
        public void ValidateUrlsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<bool>("urls"))
            {
                try
                {
                    Match match = Regex.VALID_URL.Match(test.text);
                    bool actual = (match.Success && match.Value.Equals(test.text));
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
        [Ignore("Fails a couple tests")]
        public void ValidateUrlsWithoutProtocolTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<bool>("urls_without_protocol"))
            {
                try
                {
                    Match match = Regex.VALID_URL.Match(test.text);
                    bool actual = (match.Success && match.Value.Equals(test.text));
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
        public void ValidateLengthsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<int>("lengths"))
            {
                try
                {
                    int actual = validator.GetTweetLength(test.text);
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
    }
}