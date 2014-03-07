using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Twitter.Text
{
    [TestFixture]
    public class HitHighlighterTests : ConformanceTests
    {
        private HitHighlighter highlighter = new HitHighlighter();

        public HitHighlighterTests()
            : base("hit_highlighting.yml")
        {
        }

        [Test]
        public void HighlightPlainTextTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<string>("plain_text"))
            {
                string actual = string.Empty;
                try
                {
                    actual = highlighter.Highlight(test.text, test.hits);
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
        public void HighlightWithLinksTest()
        {
            List<string> failures = new List<string>();
            foreach (dynamic test in LoadTestSection<string>("with_links"))
            {
                string actual = string.Empty;
                try
                {
                    actual = highlighter.Highlight(test.text, test.hits);
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