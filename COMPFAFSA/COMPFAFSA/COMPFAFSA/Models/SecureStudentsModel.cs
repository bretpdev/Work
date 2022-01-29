using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace COMPFAFSA.Models
{
    
    public class SecureStudentsModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Display(Name = "High School Name")]
        public string School { get; set; }
        [Display(Name = "Has Problem")]
        public string HasProblem { get; set; }
        public int? DistrictId { get; set; }
        public DateTime AddedAt { get; set; }

        public static string ToCSV(IEnumerable<SecureStudentsModel> students)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"High School Name,First Name,Last Name,DOB");
            foreach (SecureStudentsModel student in students)
            {
                sb.AppendLine(string.Join(",", new List<string>() { student.School ?? "", student.FirstName ?? "", student.LastName ?? "", student.DOB.ToShortDateString(), }));
            }
            return sb.ToString();
        }
    }
}