using System;
using System.Collections.Generic;

namespace Lucene.Net.Analysis.Hyphenation
{
    public class HyphenationTreeNode
    {
        public HyphenationTreeNode()
        {
            Children = new Dictionary<char, HyphenationTreeNode>();
        }

        public HyphenationTreeNode(IEnumerable<HyphenatorPattern> patterns) : this()
        {
            if (patterns == null) throw new ArgumentNullException("patterns");
            foreach(var pattern in patterns) Append(pattern);
        }

        public IDictionary<char, HyphenationTreeNode> Children { get; set; }
        public int[] Points { get; set; }


        public void Append(HyphenatorPattern pattern)
        {
            if (pattern == null) throw new ArgumentNullException("pattern");
            if (string.IsNullOrEmpty(pattern.Text)) return;
            var node = this;
            foreach(var character in pattern.Text)
            {
                HyphenationTreeNode next;
                if(!node.Children.TryGetValue(character, out next))
                {
                    next = new HyphenationTreeNode();
                    node.Children.Add(character, next);
                }
                node = next;
            }
            node.Points = pattern.Points;
        }
    }
}