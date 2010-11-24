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

        public IList<WordPart> HyphenateWord(string word)
        {
            if (word.Length < (MinimumPrefix + MinimumSuffix)) return new[] { new WordPart(0, word.Length, word) };

            string data = "." + word + ".";
            int[] points = new int[data.Length];

            for(var i=0; i<data.Length; ++i)
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

            int w;
            var output = new StringBuilder();
            var result = new List<WordPart>();
            int start = 0;
            for (w = 1; w < points.Length - 1; ++w)
            {
                if(points[w] % 2 != 0 && w > MinimumPrefix && w < (points.Length - MinimumSuffix))
                {
                    result.Add(new WordPart(start, w - 1, output.ToString()));
                    output.Clear();
                    start = w - 1;
                }
                output.Append(data[w]);
            }
            result.Add(new WordPart(start, points.Length - 2, output.ToString()));
            return result;
        }
    }
}
