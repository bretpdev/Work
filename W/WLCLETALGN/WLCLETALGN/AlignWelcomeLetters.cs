using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.Scripts;

namespace WLCLETALGN
{
    public partial class AlignWelcomeLetters : BatchScript
    {
        private string Password { get; set; }
        public string FileName { get; set; }
        public ProcessingForm Processing { get; set; }
        public List<BorrowerData> Borrowers { get; set; }
        public bool IsPrinting = false;
        public bool IsCommenting = false;
        public List<string> RecoveryValues { get; set; }
        public object Lock = new object();
        public RecoveryLog Rec { get; set; }
        public string RecoveryFile { get; set; }
        public EndOfJobReport EndOfJob { get; set; }
        public ErrorReport ErrReport { get; set; }
        public static string ERR_InvalidFIle = "The file selected is in an invalid format";
        public static string ERR_AddingArc = "There was an error adding comments";
        public static string ERR_Printing = "There was an error printing the letters";
        public static string EOJ_Total = "Total number of borrowers in file";
        public static string EOJ_TotalArcProcessed = "Total number of comments added";
        public List<string> EOJ_Headers = new List<string>() { ERR_InvalidFIle, ERR_AddingArc, ERR_Printing, EOJ_Total, EOJ_TotalArcProcessed };

        public AlignWelcomeLetters(ReflectionInterface ri)
            : base(ri, "WLCLETALGN", "ERR_BU35", "EOJ_BU35", new List<string>(), Uheaa.Common.DataAccess.DataAccessHelper.Region.Uheaa)
        {
            RecoveryFile = "WLCLETALGN_" + UserId;
            Password = DataAccess.GetPassword(UserId);
            RecoveryValues = new List<string>() { "", "", "", "" }; //Keep the empty strings for indexing purposes
        }

        public override void Main()
        {
            DisplayForm();
        }

        private void DisplayForm()
        {
            Processing = new ProcessingForm(UserId, this);
            Processing.ShowDialog();
        }
    }
}
