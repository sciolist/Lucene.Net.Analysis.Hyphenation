using Lucene.Net.Analysis.Hyphenation;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class HyphenatorPatternTests
    {
        [Test]
        public void Can_read_raw_string()
        {
            var result = new HyphenatorPattern("test");
            Assert.That(result.Text, Is.EqualTo("test"));
            Assert.That(result.Points, Is.EqualTo(new[] { 0, 0, 0, 0, 0 }));
        }

        [Test]
        public void Can_read_vote_strings()
        {
            var result = new HyphenatorPattern("te1st2");
            Assert.That(result.Text, Is.EqualTo("test"));
            Assert.That(result.Points, Is.EqualTo(new[] { 0, 0, 1, 0, 2 }));
        }

        [Test]
        public void Can_replace_hexadecimal_characters()
        {
            var result = new HyphenatorPattern("t2^^f63st");
            Assert.That(result.Text, Is.EqualTo("töst"));
            Assert.That(result.Points, Is.EqualTo(new[] { 0, 2, 3, 0, 0 }));
        }
    }
}