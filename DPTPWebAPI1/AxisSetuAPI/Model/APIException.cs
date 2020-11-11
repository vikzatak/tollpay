
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPTPWebAPI.AxisSetuAPI.Model
{
   
    public class APIException
    {
        public int httpCode { get; set; }
        public string httpMessage { get; set; }
        public string errorCode { get; set; }
        public string moreInformation { get; set; }
    }

}