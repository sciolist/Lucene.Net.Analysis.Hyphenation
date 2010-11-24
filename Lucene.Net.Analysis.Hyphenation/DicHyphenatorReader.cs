using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lucene.Net.Analysis.Hyphenation
{
    public class DicHyphenatorReader : HyphenatorReaderBase
    {
        public override Hyphenator Load(TextReader source)
        {
            if (source == null) throw new ArgumentNullException("source");
            string line;
            bool started = false;
            var patterns = new List<HyphenatorPattern>();
            while((line = source.ReadLine()) != null)
            {
                if (!started && line.StartsWith("charset ")) continue; // no support for charset for now.
                if(string.IsNullOrWhiteSpace(line)) continue;
                started = true;
                patterns.Add(new HyphenatorPattern(line));
            }

            var hyphenator = new Hyphenator();
            hyphenator.MinimumPrefix = 2;
            hyphenator.MinimumSuffix = 2;
            hyphenator.Patterns = patterns;
            return hyphenator;
        }
    }
}
