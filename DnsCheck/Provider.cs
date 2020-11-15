using System.Collections.Generic;

namespace DnsCheck
{
    public class Provider
    {
        public string Name { get; set; }
        public List<Parser> Parsers { get; set; }
    }

    public class Parser
    {
        public string Word { get; set; }
        public ParsingAlgorithm Algorithm { get; set; }
    }

    public enum ParsingAlgorithm
    {
        Full,
        StartWidth,
        Contains,
        EndWidth,
        Regex // TODO
    }
}