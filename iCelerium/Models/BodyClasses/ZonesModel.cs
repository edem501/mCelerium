using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCelerium.Models.BodyClasses
{
    public class ZonesModel
    {
        public int Id { get; set; }
        [Display(Name = "Numero de zone")]
        public string ZoneID { get; set; }
        [Display(Name = "Zone")]
        public string ZoneName { get; set; }
    }
    public class ClientsList
    {
        public Boolean selected { get; set; }
        public String ClientID { get; set; }
        public String FullName { get; set; }
    }
    public class CreateZoneModel
    {
        [Display(Name = "Zone")]
        [Required(ErrorMessage = "Le {0} est obligatoire")]
        public string ZoneName { get; set; }
    }

}