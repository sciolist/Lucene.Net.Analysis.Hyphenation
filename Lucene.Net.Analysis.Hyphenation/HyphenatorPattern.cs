using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lucene.Net.Analysis.Hyphenation
{
    public class HyphenatorPattern
    {
        private static readonly Regex PointRegex = new Regex(@"[\D]");
        private static readonly Regex HexFinder = new Regex(@"\^\^([0-9a-f]{2})", RegexOptions.IgnoreCase);

        public string Text { get; protected set; }
        public int[] Points { get; protected set; }

        public HyphenatorPattern(string raw)
        {
            raw = HexFinder.Replace(raw, (m) => new string((char)Convert.ToInt16(m.Groups[1].Value, 16), 1));
            Text = new string(raw.Where(c => !char.IsDigit(c)).ToArray());
            Points = PointRegex.Split(raw).Select(c => c.Length > 0 ? int.Parse(c) : 0).ToArray();
        }
    }
}