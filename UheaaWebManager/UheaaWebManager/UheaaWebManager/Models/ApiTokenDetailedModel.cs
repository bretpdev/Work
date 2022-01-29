using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UheaaWebManager.Models
{
    public class ApiTokenDetailedModel
    {
        public int? ApiTokenId { get; set; }
        [DisplayName("Token")]
        public Guid GeneratedToken { get; set; }
        public string Notes { get; set; }
        public int ControllerCount { get; set; }
        [DisplayName("Start"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DisplayName("End"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public DateTime? InactivatedAt { get; set; }

        public List<ControllerAccessModel> ControllerAccess { get; set; }
    }
}