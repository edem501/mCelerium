using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using iCelerium.Models.Interfaces;

namespace iCelerium.Models.BodyClasses
{
    public class TransactionsViewModel:IvTransaction
    {
        [Display(Name = "Date Operation")]
       public System.DateTime Date { get; set; }

        [Display(Name = "Agent Collecteur")]
        public string Nom_Commercial { get; set; }

        [Display(Name = "Membre")]
        public string Nom_Client { get; set; }

        [Display(Name = "Solde Ouverture")]
        [DataType(DataType.Currency)]
        public double Solde_Ouverture { get; set; }

        [Display(Name = "Debit")]
        [DataType(DataType.Currency)]
        public double Debit { get; set; }
        [Display(Name = "Credit")]
        [DataType(DataType.Currency)]
        public double Credit { get; set; }

        [Display(Name = "Solde Fermeture")]
        [DataType(DataType.Currency)]
        public double Solde_Fermeture { get; set; }

        [ScaffoldColumn(false)]
        public int id { get; set; }

        [ScaffoldColumn(false)]
        public string AgentId { get; set; }
    }
}