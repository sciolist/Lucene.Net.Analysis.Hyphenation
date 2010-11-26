using System.Collections.Generic;
using System.Text;

namespace Lucene.Net.Analysis.Hyphenation
{
    public class Hyphenator
    {
        private HyphenationTreeNode _tree;

        public int MinimumPrefix { get; set; }
        public int MinimumSuffix { get; set; }
        public IEnumerable<HyphenatorPattern> Patterns { get; set; }
        public HyphenationTreeNode Tree { get { return _tree ?? (_tree = new HyphenationTreeNode(Patterns)); } }

        private IList<int> Score(string data)
        {
            var points = new int[data.Length];

            for (var i = 0; i < data.Length; ++i)
            {
                var node = Tree;
                var lastNode = node;
                for (var c = 0; c < data.Length - i; ++c)
                {
                    lastNode = node;
                    var nextValue = data[c + i];
                    if (!node.Children.TryGetValue(nextValue, out node)) break;
                }

                if (lastNode.Points == null) continue;
                for (var p = 0; p < lastNode.Points.Length; ++p)
                {
                    var a = points[i + p];
                    var b = lastNode.Points[p];
                    points[i + p] = a > b ? a : b;
                }
            }
            return points;
        }

        private bool InsideMinimumLength(int len, int i)
        {
            if(i <= MinimumPrefix) return true;
            if(i >= (len - MinimumSuffix)) return true;
            return false;
        }

        public IList<WordPart> HyphenateWord(string word)
        {
            if (word.Length < (MinimumPrefix + MinimumSuffix)) return new[] { new WordPart(0, word.Length, word) };

            string data = "." + word + ".";
            var points = Score(data);

            int start = 0;
            var output = new StringBuilder();
            var result = new List<WordPart>();
            for (int i = 1; i < points.Count - 1; ++i)
            {
                var skip = points[i]%2 != 0;
                if (skip && !InsideMinimumLength(points.Count, i))
                {
                    result.Add(new WordPart(start, i - 1, output.ToString()));
                    output.Clear();
                    start = i - 1;
                }
                output.Append(data[i]);
            }
            result.Add(new WordPart(start, points.Count - 2, output.ToString()));
            return result;
        }
    }
}
