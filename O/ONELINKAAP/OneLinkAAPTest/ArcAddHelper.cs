using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using static Uheaa.Common.Scripts.ReflectionInterface;
using Uheaa.Common.Scripts;



namespace OneLinkAAPTEST
{
    
    public class ArcAddHelper
    {
        private IReflectionInterface ri;

        public ArcAddHelper(IReflectionInterface RI)
        {
            this.ri = RI;
        }

        public ArcHelperResults AddLP50Comment(string account, string actionCode, string activityType, string activityContactType, string comment, string scriptId)
        {
            ArcHelperResults results = new ArcHelperResults();
            ri.FastPath("LP50A");
            ri.Hit(ReflectionInterface.Key.EndKey);
            if (account != null && account.Length > 3 && account.Substring(0, 3).ToUpper().Contains("RF@"))
            {
                ri.PutText(3, 13, "", true);
                ri.PutText(3, 48, account);
            }
            else
                ri.PutText(3, 13, account);

            ri.PutText(9, 20, actionCode, ReflectionInterface.Key.Enter);

            if (!ri.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY"))
                results.CommentAdded = false;

            if (ri.MessageCode == "01527")
            {
                results.ErrorMessage = "Can't find borrower on system " + account;
                results.CommentAdded = false;
            }
            else
            {
                results.ErrorMessage = "Borrower processed " + account;
                results.CommentAdded = true;
            }

            ri.PutText(7, 2, activityType);
            ri.PutText(7, 5, activityContactType);


            if (comment.Length + scriptId.Length < 70)
            {
                comment = string.Format("{0}. {{ {1} }}", comment, scriptId);
                ri.PutText(13, 2, comment, ReflectionInterface.Key.F6);
            }
            else
            {
                if (comment.Length + scriptId.Length > 585)
                    throw new Exception("The requested comment will not fit on LP50");
            }
            return  results;
        }

        public ArcHelperResults ProcessCompass(ARCADDPROCESSING.ArcRecord arc)
        {
            ArcHelperResults ahr = new ArcHelperResults();

            if (String.IsNullOrEmpty(arc.ActivityType))
                ahr.CommentAdded = true;    
            else
                ahr.CommentAdded = false;
        return ahr;
        }

        internal static void ThrowsExceptionIfCommentTooLarge(string comment)
        {
            if(comment.Length > 33)
                throw new InvalidOperationException();

        }

    }



    public class ArcHelperResults
    {
        public bool CommentAdded { get; internal set; }
        public string ErrorMessage { get; internal set; }
    }
}
