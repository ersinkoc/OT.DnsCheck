using System;
using System.Collections.Generic;
using System.Text;

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
        public CheckAlgorith FindAt { get; set; }
    }


    public enum CheckAlgorith
    {
        Full,
        StartWidth,
        Contains,
        EndWidth
    }
}
