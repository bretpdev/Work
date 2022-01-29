using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ProjectRequest.Models
{
    public class ScoreType
    {
        public int ProjectId { get; set; }
        public int ScoreTypeId { get; set; }
        [DisplayName("Score Description")]
        public string ScoreDescription { get; set; }
        public int Score { get; set; } = 0;
        public int? ScoreId { get; set; } = null;
        [DisplayName("Score Weight")]
        public int ScoreWeight { get; set; }
        [DisplayName("Score Out Of")]
        public int ScoreOutOf { get; set; }
    }
}