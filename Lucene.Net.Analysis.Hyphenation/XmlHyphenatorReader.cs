using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Lucene.Net.Analysis.Hyphenation
{
    public class XmlHyphenatorReader : HyphenatorReaderBase
    {
        private static readonly Regex WhiteSpaceRegex = new Regex(@"\s");

        public override Hyphenator Load(TextReader source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return Read(XDocument.Load(source));
        }

        public Hyphenator Read(XDocument document)
        {
            if (document == null || document.Root == null) throw new ArgumentException("Invalid document supplied.", "document");
            var hyphenator = new Hyphenator();

            var minMax = document.Root.Element("hyphen-min");
            hyphenator.MinimumPrefix = int.Parse(GetValue(minMax, "before", "2"));
            hyphenator.MinimumSuffix = int.Parse(GetValue(minMax, "after", "2"));
            hyphenator.Patterns = ReadPatterns(document.Root.Element("patterns")).ToList();
            return hyphenator;
        }

        private static IEnumerable<HyphenatorPattern> ReadPatterns(XElement source)
        {
            if (source == null) yield break;
            using (var reader = new StringReader(source.Value))
            {
                foreach (var word in WhiteSpaceRegex.Split((reader.ReadToEnd() ?? "").Trim()))
                {
                    yield return new HyphenatorPattern(word);
                }
            }
        }

        private static string GetValue(XElement node, string attribute, string defaultValue)
        {
            if (node == null) return defaultValue;
            var attr = node.Attribute(attribute);
            if (attr == null || string.IsNullOrWhiteSpace(attr.Value)) return defaultValue;
            return attr.Value;
        }
    }
}