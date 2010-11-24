namespace Lucene.Net.Analysis.Hyphenation
{
    public class WordPart
    {
        public WordPart(int start, int end, string text)
        {
            Start = start;
            End = end;
            Text = text;
        }

        public int Start { get; set; }
        public int End { get; set; }
        public string Text { get; set; }
    }
}