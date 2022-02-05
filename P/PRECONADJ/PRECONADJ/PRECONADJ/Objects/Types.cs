using Uheaa.Common;

namespace PRECONADJ
{
    public class Types
    {
        public string Type { get; set; }
        public string SubType { get; set; }

        /// <summary>
        /// Gets the Type an SubType
        /// </summary>
        public Types GetTypeSubtype(string additionalDisbursement, string cap)
        {
            Types t = new Types();
            double disbursementAmount = additionalDisbursement.ToDouble();
            if (disbursementAmount == 0)
            {
                t.Type = "10";
                t.SubType = "10";
            }
            if (cap.ToUpper() == "Y" && disbursementAmount > 0)
            {
                t.Type = null;
                t.SubType = null;
                Dialog.Error.Ok("Additional disbursement was greater than 0 and Cap was Y. Please fix the error in the spreadsheet and begin processing again");
            }
            else if (cap.ToUpper() == "Y")
            {
                t.Type = "70";
                t.SubType = "01";
            }
            else if (disbursementAmount > 0)
            {
                t.Type = "01";
                t.SubType = "01";
            }
            else
            {
                t.Type = "10";
                t.SubType = "10";
            }
            return t;
        }
    }
}