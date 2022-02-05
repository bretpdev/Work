using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LSLETTERSU.Models
{
    public class ProjectData
    {
        public bool IsLoaded { get; set; }
        public List<LetterData> Letters { get; set; }
        public List<string> LetterTypes { get; set; }
        public List<string> LetterOptions { get; set; }
        public List<string> LetterChoices1 { get; set; }
        public List<string> LetterChoices2 { get; set; }
        public List<string> LetterChoices3 { get; set; }
        public List<string> LetterChoices4 { get; set; }
        public List<string> LetterChoices5 { get; set; }
        public List<string> ManualLetters { get; set; }

        [Required]
        public string DischargeAmount { get; set; }

        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "You must provide 9 digits with no alpha characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Account Number is required")]
        public string AccountIdentifier { get; set; }

        [Required]
        public string SchoolName { get; set; }

        [Required(ErrorMessage= "The field Last Date of Attendance must be a date ex: 01/01/2000")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Last date attended school")]
        public DateTime LastDate { get; set; }

        [Required(ErrorMessage = "The field School Closure Date must be a date ex: 01/01/2000")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "School closure date")]
        public DateTime SchoolClosure { get; set; }

        [Required]
        public List<string> DefForb { get; set; }

        [Required(ErrorMessage = "The field Deferment/Forbearance date must be a date ex: 01/01/2000")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Deferment/Forbearance date")]
        public DateTime DefForbDate { get; set; }

        [Required(ErrorMessage = "The field Loan Term End Date must be a date ex: 01/01/2000")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Loan Term end date")]
        public DateTime LoanTermEndDate { get; set; }
        
        public string ManualDenialReason { get; set; }
        [Required]
        public string BeginYear { get; set; }
        [Required]
        public string EndYear { get; set; }

        public ProjectData()
        {
            Letters = new List<LetterData>();
            LetterTypes = new List<string>();
            LetterOptions = new List<string>();
            LetterChoices1 = new List<string>();
            LetterChoices2 = new List<string>();
            LetterChoices3 = new List<string>();
            LetterChoices4 = new List<string>();
            LetterChoices5 = new List<string>();
            ManualLetters = new List<string>();
            DefForb = new List<string>();
            DischargeAmount = "";
            SchoolName = "";
            BeginYear = "";
            EndYear = "";
        }
    }
}