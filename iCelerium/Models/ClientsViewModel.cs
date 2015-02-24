using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCelerium.Models
{
    public class ClientsViewModel
    {
        [Display(Name = "Identifiant")]
        public string ClientId { get; set; }
        [Display(Name = "Telephone")]
        public string ClientTel { get; set; }
        [Display(Name = "Mise")]
        public double Mise { get; set; }
        [Display(Name = "Solde")]
        public double Solde { get; set; }
        [Display(Name = "Nom & prenom")]
        public string Name { get; set; }
        [Display(Name = "Sexe")]
        public string Sexe { get; set; }
    }

    public class EditClientsViewModel
    {
        [Display(Name = "Identifiant")]
        
        public string ClientId { get; set; }
        [Display(Name = "Telephone")]
        [Required]
        public string ClientTel { get; set; }
        [Display(Name = "Mise")]
        [Required]
        public double Mise { get; set; }
        [Display(Name = "Solde")]

        public double Solde { get; set; }
        [Display(Name = "Nom & prenom")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Sexe")]
        public string Sexe { get; set; }
        [Display(Name = "Agent collecteur")]
        public string AgentName { get; set; }


    }
}