using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CSHRCPTFED.Infrastructure;

namespace CSHRCPTFED.ViewModels
{
    public class CashReceiptModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an Account Number or SSN")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Digits only.")]
        [MaxLength(10, ErrorMessage = "Must be 9 or 10 digits.")]
        [MinLength(9, ErrorMessage = "Must be 9 or 10 digits.")]
        [Display(Name = "Account Number/SSN")]
        public string AccountIdentifier { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter or lookup the Payee's Name")]
        [Display(Name = "Name")]
        public string BorrowerName { get; set; }

        [MaxLength(15, ErrorMessage = "No more than 15 characters.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Check #")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Check number cannot contain special characters.")]
        [Display(Name = "Check #")]
        public string CheckId { get; set; }

        [Required]
        [RegularExpression(@"^(?!0*(\.0+)?$)(\d+|\d*\.\d+) *$", ErrorMessage = "Please enter a valid amount.")]
        [Range(0.0, 1000000, ErrorMessage = "Please enter a valid amount.")]
        public string Amount { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] //fix for chrome date display
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Check Date")]
        [Display(Name = "Date Received")]
        public DateTime CheckDate { get; set; } = DateTime.Now;

        public Payee Payee { get; set; }
    }
}