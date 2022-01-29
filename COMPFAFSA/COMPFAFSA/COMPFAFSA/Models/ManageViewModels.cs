using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COMPFAFSA.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
    }

    public class ManageUser
    {
        public int UserId { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Locked Out")]
        public bool LockedOut { get; set; }
        public bool Admin { get; set; }
    }

    public class DeleteUser
    {
        public int UserId { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
    }

    public class UnlockUser
    {
        public int UserId { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
    }

    public class CreateUser
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "You must provide a email")]
        [MaxLength(1000)]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}", ErrorMessage = "Password must contain 8 characters, a lower and uppercase character, a number, and a special character.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool Admin { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}", ErrorMessage = "Password must contain 8 characters, a lower and uppercase character, a number, and a special character.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}", ErrorMessage = "Password must contain 8 characters, a lower and uppercase character, a number, and a special character.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserSchool
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int SchoolId { get; set; }
        [Required]
        public string School { get; set; }
    }

    public class User
    {
        [Required]
        public int UserId { get; set; }
    }

}