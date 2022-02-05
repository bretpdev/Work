using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COMPFAFSA.Models
{

    public class RequestModel
    {
        [Display(Name="First Name")]
        [MaxLength(200)]
        [Required(ErrorMessage = "You must provide a first name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [MaxLength(200)]
        [Required(ErrorMessage = "You must provide a last name")]
        public string LastName { get; set; }
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage="Invalid email address")]
        [Required(ErrorMessage = "You must provide a email")]
        [MaxLength(1000)]
        public string EmailAddress { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        [MaxLength(20)]
        [Phone]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "School")]
        [Required(ErrorMessage = "You must select a school")]
        public string School { get; set; }
        [Required(ErrorMessage = "You must select one or more classes")]
        public List<string> Class { get; set; }
        [Required(ErrorMessage = "You must select a type of data you wish to request")]
        public List<string> DataType { get; set; }
        [Display(Name = "Additional Information")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [MaxLength(2000)]
        public string AdditionalInformation { get; set; }
    }

    public class RequestSSMS
    {
        public RequestSSMS()
        {

        }

        public RequestSSMS(RequestModel request)
        {
            FirstName = request.FirstName;
            LastName = request.LastName;
            EmailAddress = request.EmailAddress;
            PhoneNumber = request.PhoneNumber;
            School = request.School;  
            Classes = string.Join(",", request.Class);
            DataType = string.Join(",", request.DataType);
            AdditionalInformation = request.AdditionalInformation;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string School { get; set; }
        public string Classes { get; set; }
        public string DataType { get; set; }
        public string AdditionalInformation { get; set; }
    }
}