using System;
using System.IO;

namespace Lucene.Net.Analysis.Hyphenation
{
    public abstract class HyphenatorReaderBase
    {
        public Hyphenator Parse(string data)
        {
            if (data == null) throw new ArgumentNullException("data");
            using (var reader = new StringReader(data))
                return Load(reader);
        }

        public Hyphenator Load(string fileName)
        {
            using (var file = File.OpenRead(fileName))
            using (var reader = new StreamReader(file))
                return Load(reader);
        }

        public abstract Hyphenator Load(TextReader source);
    }
}