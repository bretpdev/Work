using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectRequest.Models
{
    public class ProjectScoring
    {
        public ProjectScoring(int projectId, int scoreTypeId)
        {
            ProjectId = projectId;
            ScoreTypeId = scoreTypeId;
        }

        public ProjectScoring()
        {

        }

        public int ProjectId { get; set; }
        public int ScoreTypeId { get; set; }
        public int? ScoreId { get; set; } = null;
        [DisplayName("Scoring Department")]
        public string ScoringDepartment { get; set; } = null;
        [Required]
        [Range(0,5)]
        public int Score { get; set; } = 0;
    }
}