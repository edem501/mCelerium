using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace iCelerium.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class EditAccountViewModel
    {
        public EditAccountViewModel()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.AspNetRoles = new HashSet<AspNetRole>();
        }
    
        public string Id { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
         [Required]
         [Display(Name = "Full Name")]
        public string FullName { get; set; }
         [Required]
         public int RegionID { get; set; }
          [Required]
         public int PollingStationID { get; set; }
         [Required] 
         public int DistrictID { get; set; }
        [Required]
         public int ContituencyID { get; set; }
        [Display(Name = "Is Enabled")]
         public bool IsEnable { get; set; }
    
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
         [Required]
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
    }
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must at least {2} characteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "new password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "Le Nouveau mot de passe et la confirmation sont differents")]
        public string ConfirmPassword { get; set; }
    }
   
    public class LoginViewModel
    {
         [Required]
        public string UserName { get; set; }
         [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class RolesViewModel
    {
        //public IEnumerable<SelectListItem> RoleName { get; set; }
        public IEnumerable<string> ListRoleName { get; set; }
    
    }

    
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

    }
}
