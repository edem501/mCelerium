using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iCelerium.Models.BodyClasses
{
    public class Votes
    {
        public string ConstituencyID { get; set; }
    }
    public class VoteCollationModelView
    {
        [Required]
        public int NDC { get; set; }
        [Required]
        public int NPP { get; set; }
        [Required]
        public int  CPP { get; set; }
        [Required]
        public int PPP { get; set; }
        [Required]
        public int OTHERS { get; set; }
        [Required]
        public int REJECTED { get; set; }
        [Required]
        public string ConstituencyID { get; set; }
        [Required]
        public int PollingStationID { get; set; }
    }

    public class CollationModelView
    {
        [Required]
        public int NDC { get; set; }
        [Required]
        public int NPP { get; set; }
        [Required]
        public int CPP { get; set; }
        [Required]
        public int PPP { get; set; }
        [Required]
        public int OTHERS { get; set; }
        [Required]
        public int REJECTED { get; set; }
        [Required]
        public string ConstituencyID { get; set; }
        [Display(Name="TOTAL VOTES")]
        public int TOTAL { get; set; }

    }
    public class VotesPerRegionModelView
    {
        [Required]
        public int NDC { get; set; }
        [Required]
        public int NPP { get; set; }
        [Required]
        public int CPP { get; set; }
        [Required]
        public int PPP { get; set; }
        [Required]
        public int OTHERS { get; set; }
        [Required]
        public int REJECTED { get; set; }
        [Required]
        [Display(Name = "# OF COLLATED CTCY")]
        public int? NoConstituency{ get; set; }
        public string Region { get; set; }
        [Display(Name = "TOTAL VOTES")]
        public int TOTAL { get; set; }

    }
}