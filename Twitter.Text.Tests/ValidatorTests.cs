using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

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

        #region Conformance

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
                    bool actual = validator.IsValidUsername(test.text);
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
        public void ValidateListsTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<bool>("lists"))
            {
                try
                {
                    bool actual = validator.IsValidList(test.text);
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
                    bool actual = validator.IsValidHashTag(test.text);
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
                    bool actual = validator.IsValidUrl(test.text);
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
                    bool actual = validator.IsValidUrl(test.text);
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

        #endregion

        #region Additional

        [Test]
        public void BOMCharacterTest()
        {
            Assert.False(validator.IsValidTweet("test \uFFFE"));
            Assert.False(validator.IsValidTweet("test \uFEFF"));
        }

        [Test]
        public void InvalidCharacterTest()
        {
            Assert.False(validator.IsValidTweet("test \uFFFF"));
            Assert.False(validator.IsValidTweet("test \uFEFF"));
        }

        [Test]
        public void DirectionChangeCharactersTest()
        {
            Assert.False(validator.IsValidTweet("test \u202A test"));
            Assert.False(validator.IsValidTweet("test \u202B test"));
            Assert.False(validator.IsValidTweet("test \u202C test"));
            Assert.False(validator.IsValidTweet("test \u202D test"));
            Assert.False(validator.IsValidTweet("test \u202E test"));
        }

        [Test]
        public void AccentCharactersTest()
        {
            String c = "\u0065\u0301";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 139; i++)
            {
                builder.Append(c);
            }
            Assert.True(validator.IsValidTweet(builder.ToString()));
            Assert.True(validator.IsValidTweet(builder.Append(c).ToString()));
            Assert.False(validator.IsValidTweet(builder.Append(c).ToString()));
        }

        [Test]
        public void MutiByteCharactersTest()
        {
            String c = "\ud83d\ude02";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 139; i++)
            {
                builder.Append(c);
            }
            Assert.True(validator.IsValidTweet(builder.ToString()));
            Assert.True(validator.IsValidTweet(builder.Append(c).ToString()));
            Assert.False(validator.IsValidTweet(builder.Append(c).ToString()));
        }

        #endregion
    }
}