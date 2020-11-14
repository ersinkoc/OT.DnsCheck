using System.Collections.Generic;

namespace DnsCheck
{
    public class Provider
    {
        public string Name { get; set; }
        public List<SearchPhrase> SearchPhrases { get; set; }
    }

    public class SearchPhrase
    {
        public string Phrase { get; set; }
        public WhereIs FindAt { get; set; }
    }

    public enum WhereIs
    {
        Full,
        StartWidth,
        Contains,
        EndWidth
    }
}