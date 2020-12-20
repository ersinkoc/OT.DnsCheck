using System;
using System.Collections.Generic;
using System.Text;

namespace DnsCheck
{
    public class CheckResult
    {
        public string Domain { get; set; }
        public string ControlTime { get; set; }
        public string MailProvider { get; set; }
    }
}
