using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCelerium.Models
{
    public class AuditViewModel
    {
        public System.Guid AuditID { get; set; }
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }
        [Display(Name = "Uri Accessed")]
        public string AreaAccessed { get; set; }
        [Display(Name = "Timestamp")]
        public System.DateTime Timestamp { get; set; }
    }
}