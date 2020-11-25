using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{
   
    public class TagOperation
    {
        public Header header { get; set; }
        public TagOperationPayload payload { get; set; }
    }

    

    public class TagOperationPayload
    {
        public string tagId { get; set; }
        public string operationId { get; set; }
        public string walletId { get; set; }
    }

}
