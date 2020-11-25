using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{
    public class Header
    {
        public string requestId { get; set; }
        public string channelId { get; set; }
        public string checksum { get; set; }
    }
    public class Subheader
    {
        public string requestUUID { get; set; }
        public string serviceRequestId { get; set; }
        public string serviceRequestVersion { get; set; }
        public string channelId { get; set; }
    }
}
