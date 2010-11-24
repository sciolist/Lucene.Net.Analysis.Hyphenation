using System.Linq;
using Lucene.Net.Analysis.Hyphenation;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class XmlHyphenatorTests
    {
        private const string TestHyphenationDefinition = @"<?xml version=""1.0"" encoding=""utf-8""?>
<hyphenation-info>
  <hyphen-min before=""2"" after=""2""/>
  <patterns>.hy1p ph1e h2e1n io1n.</patterns>
</hyphenation-info>";

        [Test]
        public void Can_create_hyphenator()
        {
            var reader = new XmlHyphenatorReader();
            var hyphenator = reader.Parse(TestHyphenationDefinition);
            Assert.That(hyphenator, Is.Not.Null);
        }

        [Test]
        public void Hyphenator_reads_patterns()
        {
            var reader = new XmlHyphenatorReader();
            var hyphenator = reader.Parse(TestHyphenationDefinition);
            Assert.That(hyphenator.Patterns.Count(), Is.EqualTo(4));
        }

        [Test]
        public void Hyphenator_can_hyphenate_word()
        {
            var reader = new XmlHyphenatorReader();
            var hyphenator = reader.Parse(@"<?xml version=""1.0"" encoding=""utf-8""?>
<hyphenation-info>
  <hyphen-min before=""2"" after=""2""/>
  <patterns>p3h1e</patterns>
</hyphenation-info>");

            var text = hyphenator.HyphenateWord("zzphezz").Select(p => p.Text).ToArray();
            Assert.That(text, Is.EqualTo(new[] { "zzp", "h", "ezz" }));
        }

        [Test]
        public void Hyphenator_wont_hyphenate_word_inside_minimum_prefix()
        {
            var reader = new XmlHyphenatorReader();
            var hyphenator = reader.Parse(@"<?xml version=""1.0"" encoding=""utf-8""?>
<hyphenation-info>
  <hyphen-min before=""2"" after=""2""/>
  <patterns>p1i1z</patterns>
</hyphenation-info>");

            var text = hyphenator.HyphenateWord("pizza").Select(p => p.Text).ToArray();
            Assert.That(text, Is.EqualTo(new[] { "pi", "zza" }));
        }

        [Test]
        public void Hyphenator_wont_hyphenate_word_inside_minimum_suffix()
        {
            var reader = new XmlHyphenatorReader();
            var hyphenator = reader.Parse(@"<?xml version=""1.0"" encoding=""utf-8""?>
<hyphenation-info>
  <hyphen-min before=""2"" after=""2""/>
  <patterns>z1z1a</patterns>
</hyphenation-info>");

            var text = hyphenator.HyphenateWord("pizza").Select(p => p.Text).ToArray();
            Assert.That(text, Is.EqualTo(new[] { "piz", "za" }));
        }
    }
}
