using Uheaa.Common;
using Uheaa.Common.DataAccess;
using static Uheaa.Common.Dialog;

namespace OLDEMOS
{
    public class BwrInfo411Processor
    {
        private static BrwInfo411 Form411 { get; set; }
        private static Borrower Borrower { get; set; }
        private static bool info411Changed;

        public static bool Info411Changed { get { return info411Changed; } }

        public static void Show411Form(Borrower bor, bool userSelected = false)
        {
            Borrower = bor;
            Borrower.Info411 = "";
            Helper.RI.FastPath($"LP50I{bor.Ssn};;;;;M1411");
            if (Helper.RI.AltMessageCode == "47004" && !userSelected)
                return;
            if (Helper.RI.CheckForText(1, 58, "ACTIVITY SUMMARY SELECT"))
                Helper.RI.PutText(6, 2, "X", true);
            if (Helper.RI.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY"))
                Borrower.Info411 = $"{Helper.RI.GetText(13, 2, 75)}{Helper.RI.GetText(14, 2, 75)}{Helper.RI.GetText(15, 2, 75)}{Helper.RI.GetText(16, 2, 75)}{Helper.RI.GetText(17, 2, 75)}{Helper.RI.GetText(18, 2, 75)}{Helper.RI.GetText(19, 2, 75)}{Helper.RI.GetText(20, 2, 75)}";

            ShowForm();
        }

        private static void ShowForm()
        {
            Form411 = new BrwInfo411();
            Form411.ShowDialog(Borrower);
        }

        public static void SaveChangesToSystems(string updatedText = "")
        {
            bool results = false;

            if (updatedText.IsPopulated() && !updatedText.Contains("No Borrower Information Found"))
                Borrower.Info411 = updatedText;
            if (Borrower != null)
                results = Helper.RI.AddCommentInLP50(Borrower.Ssn, "AM", "10", "M1411", Borrower.Info411, Program.ScriptId);

            if (results == false)
                Info.Ok("For some reason an activity comment using the M1411 ARC could not be added.  Please notifiy the System Support Help Desk.", "Activity Comment Add Error");
            Close411Form();
        }

        /// <summary>
        /// closes the 411 screen
        /// </summary>
        public static void Close411Form()
        {
            if (Form411 != null)
            {
                Form411.Close();
                Form411 = null;
            }
        }
    }
}