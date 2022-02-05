using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHRPT.Models
{
    public class Recipient
    {
        public int? RecipientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public int SchoolCount { get; set; }
    }
}