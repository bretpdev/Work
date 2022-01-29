using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Uheaa.Common;

namespace UheaaWebManager.Models
{
    public class UserTokenDetailedModel
    {
        public int? UserTokenId { get; set; }

        [DisplayName("User")]
        public string AssociatedWindowsUsername { get; set; }

        [DisplayName("Token")]
        public Guid GeneratedToken { get; set; }
        public string Notes { get; set; }

        [DisplayName("Start"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayName("End"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public DateTime? InactivatedAt { get; set; }

        [DisplayName("Role")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public List<ActiveDirectoryUser> AvailableWindowsUsernames { get; set; }
        public List<RoleSimpleModel> AvailableRoles { get; set; }
    }
}