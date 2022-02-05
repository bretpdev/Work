namespace REFCALL
{
    public class CostCenterCode
    {
        public string Ccc { get; set; }
        public string AcsPartCode { get; set; }

        public CostCenterCode(string ccc, string acsPartCode)
        {
            this.Ccc = ccc;
            this.AcsPartCode = acsPartCode;
        }

        public static readonly CostCenterCode ERROR = new CostCenterCode("ERROR", "ERROR");
    }
}