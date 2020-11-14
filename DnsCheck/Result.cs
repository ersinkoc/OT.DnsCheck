using System.Collections.Generic;

namespace DnsCheck
{
    partial class Program
    {
        public class ReturnJson
        {
            public string ProcessResponseTime { get; set; }
            public string Domain { get; set; }
            public string RequestType { get; set; }
            public List<string> Warnings { get; set; }
        }

        public class ReturnJsonMX : ReturnJson
        {
            public List<ResultMx> Results { get; set; }
        }

        public class ReturnJsonNS : ReturnJson
        {
            public List<ResultNs> Results { get; set; }
        }

        public class ResultMx
        {
            public int Reference { get; set; }
            public string Exchange { get; set; }
        }

        public class ResultNs
        {
            public string nameServer { get; set; }
        }
    }
}