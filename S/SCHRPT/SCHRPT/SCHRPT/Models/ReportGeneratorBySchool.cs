using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHRPT.Models
{
    public class ReportGeneratorBySchool
    {
        public List<School> Schools { get; set; } = new List<School>();
        public List<ReportType> Reports { get; set; } = new List<ReportType>();
        public List<int> SelectedSchoolIds { get; set; } = new List<int>();
    }
}