using Q;

namespace PUTSUSPCOM
{
    public abstract class OptionProcessorBase : ScriptSessionBase
    {

        protected Suspense _systemSuspenseData;
        protected string SCRIPT_ID = "PUTSUSPCOM";

        public string Comments { get; set; }
        private string _userID = string.Empty;
        private string UserID
        {
            get
            {
                if (_userID == string.Empty)
                {
                    //if user ID not gathered yet then get it
                    _userID = GetUserIDFromLP40();
                }
                return _userID;
            }
        }

        public OptionProcessorBase(ReflectionInterface ri, Suspense systemSuspenseData)
            : base(ri)
        {
            _systemSuspenseData = systemSuspenseData;
        }

        /// <summary>
        /// Process option
        /// </summary>
        public abstract void Process();

        //add comments on TD22, TD37 or LP50
        protected virtual void AddComments(string comments)
        {
            if (ATD22FirstLoanOnly(_systemSuspenseData.BorrowerDemos.SSN, "SUSDE", comments) == false)
            {
                if (ATD37FirstLoan(_systemSuspenseData.BorrowerDemos.SSN, "SUSDE", comments, SCRIPT_ID, false) != Common.CompassCommentScreenResults.CommentAddedSuccessfully)
                {
                    AddCommentInLP50(_systemSuspenseData.BorrowerDemos.SSN, "SUSDE", SCRIPT_ID, "MS", "16", comments);
                }
            }
        }

        private bool ATD22FirstLoanOnly(string ssn, string arc, string comment)
        {
            Coordinate coord = null;
            string userID = UserID;

            FastPath("TX3Z/ATD22" + ssn);
            if (Check4Text(1, 72, "TDX23") == false)
            {
                return false;
            }
            //find the ARC
            while (coord == null)
            {
                coord = FindText(arc);
                if (coord == null)
                {
                    Hit(ReflectionInterface.Key.F8);
                    if (Check4Text(23, 2, "90007"))
                    {
                        return false;
                    }
                }
            }
            //select the ARC
            PutText(coord.Row, coord.Column - 5, "01", ReflectionInterface.Key.Enter);
            //exit the function if the selection screen is not displayed
            if (Check4Text(1, 72, "TDX24") == false)
            {
                return false;
            }
            //select first loan
            PutText(11, 3, "X");
            //enter short comments
            if (comment.Length < 132)
            {
                PutText(21, 2, comment + string.Format("  {0}{1}{2} /{3}", "{", SCRIPT_ID, "}", userID));
                Hit(ReflectionInterface.Key.Enter);
                if (Check4Text(23, 2, "02860") == false)
                {
                    return false;
                }
            }
            else //long comments
            {
                //fill the first screen
                PutText(21, 2, comment.SafeSubstring(0, 154), ReflectionInterface.Key.Enter);
                if (Check4Text(23, 2, "02860") == false)
                {
                    return false;
                }
                Hit(ReflectionInterface.Key.F4);
                //enter the rest on the expanded comments screen
                for (int k = 154; k < comment.Length; k = k + 260)
                {
                    EnterText(comment.SafeSubstring(k, 260));
                }
                EnterText("  {" + SCRIPT_ID + "} /" + userID);
                Hit(ReflectionInterface.Key.Enter);
                if (Check4Text(23, 2, "02114") == false)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
