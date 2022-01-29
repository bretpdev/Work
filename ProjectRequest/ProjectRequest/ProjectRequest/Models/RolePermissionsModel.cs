namespace ProjectRequest.Models
{
    public class RolePermissions
    {
        public string Role { get; set; }
        public bool Read { get; set; }
        public bool Create { get; set; }
        public bool Score { get; set; }
        public bool ScoreFinance { get; set; }
        public bool ScoreRequestor { get; set; }
        public bool ScoreUrgency { get; set; }
        public bool ScoreResources { get; set; }
        public bool ScoreRisk { get; set; }
        public bool Archive { get; set; }
        public bool Admin { get; set; }
    }
}