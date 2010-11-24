using System.Collections.Generic;
using Lucene.Net.Analysis.Tokenattributes;

namespace Lucene.Net.Analysis.Hyphenation
{
    public class HyphenationTokenFilter : TokenFilter
    {
        private readonly Hyphenator _hyphenator;
        private readonly TermAttribute _termAtt;
        private readonly TypeAttribute _typeAtt;
        private readonly OffsetAttribute _ofsAtt;

        public HyphenationTokenFilter(TokenStream input, Hyphenator hyphenator)
            : base(input)
        {
            _hyphenator = hyphenator;
            _termAtt = (TermAttribute)AddAttribute(typeof(TermAttribute));
            _typeAtt = (TypeAttribute)AddAttribute(typeof(TypeAttribute));
            _ofsAtt = (OffsetAttribute)AddAttribute(typeof(OffsetAttribute));
        }

        private readonly Queue<WordPart> _terms = new Queue<WordPart>();
        private int _startOfs;

        private bool SetNextTerm()
        {
            if (_terms.Count == 0) return false;
            var nextTerm = _terms.Dequeue();
            _termAtt.SetTermLength(nextTerm.Text.Length);
            _termAtt.SetTermBuffer(nextTerm.Text);
            _ofsAtt.SetOffset(_startOfs + nextTerm.Start, _startOfs + nextTerm.End);
            _typeAtt.SetType("<ALPHANUM>");
            return true;
        }

        public override bool IncrementToken()
        {
            if (SetNextTerm()) return true;
            if (!input.IncrementToken()) return false;

            _startOfs = _ofsAtt.StartOffset();
            var term = _termAtt.Term();
            foreach (var newWord in _hyphenator.HyphenateWord(term))
            {
                _terms.Enqueue(newWord);
            }
            return true;
        }
    }
}