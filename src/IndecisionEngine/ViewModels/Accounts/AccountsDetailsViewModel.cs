using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IndecisionEngine.Models;
using Microsoft.AspNet.Identity;

namespace IndecisionEngine.ViewModels.Accounts
{
    public class AccountsDetailsViewModel
    {
        public AccountsDetailsViewModel()
        {
        }

        public AccountsDetailsViewModel(ApplicationUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            DisplayName = user.DisplayName;
            EmailConfirmed = user.EmailConfirmed;
            PhoneNumber = user.PhoneNumber;
            PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            TwoFactorEnabled = user.TwoFactorEnabled;
            LockoutEnabled = user.LockoutEnabled;
            LockoutEnd = user.LockoutEnd;
        }

        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail Address")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "E-mail Confirmed")]
        public bool EmailConfirmed { get; set; }
        
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Required]
        [Display(Name = "Two-Factor Enabled")]
        public bool TwoFactorEnabled { get; set; }

        [Required]
        [Display(Name = "Lockout Enabled")]
        public bool LockoutEnabled { get; set; }
        
        [Display(Name = "Lockout End")]
        public DateTimeOffset? LockoutEnd { get; set; }

        public IEnumerable<UserLoginInfo> Logins { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
