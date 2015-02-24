//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iCelerium.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Agent
    {
        public string UserID { get; set; }
        public int RegionID { get; set; }
        public int DistrictID { get; set; }
        public int ContituencyID { get; set; }
        public bool IsEnable { get; set; }
        public int PollingStationID { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Constituency Constituency { get; set; }
        public virtual District District { get; set; }
        public virtual PollingStation PollingStation { get; set; }
        public virtual Region Region { get; set; }
    }
}