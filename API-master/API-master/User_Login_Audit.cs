//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DPTPWebAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class User_Login_Audit
    {
        public int srno { get; set; }
        public Nullable<int> userId { get; set; }
        public string IPAddress { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public string SessionID { get; set; }
        public string Activity { get; set; }
    }
}
