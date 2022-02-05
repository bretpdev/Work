using System.Text;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Dialog;
using static Uheaa.Common.Scripts.Screen;

namespace OLDEMOS
{
    public class LoginHelper
    {

        #region Singleton
        private LoginHelper() { }
        private static LoginHelper helper = new LoginHelper();
        public static LoginHelper Instance { get { return helper; } }
        #endregion

        public bool IsLoggedIn { get; private set; }

        static ReflectionInterface ses = Helper.RI;
        public bool Login(LoginForm login, IWin32Window owner = null)
        {
            try
            {
                if (!ses.Check(FirstScreen))
                    ses.FastPath("LOGOUT");
            }
            catch (System.Exception)
            {
                //If the session was closed, it will create a new one and needs to be assigned again.
                ses = Helper.RI;
                if (!ses.Check(FirstScreen))
                    ses.FastPath("LOGOUT");
            }
            ses.Wait(FirstScreen);
            if (DataAccessHelper.TestMode)
                ses.PutText(16, 12, "QTOR", ReflectionInterface.Key.Enter, false);
            else
                ses.PutText(16, 12, "PHEAA", ReflectionInterface.Key.Enter, false);

            ses.Wait(LoginScreen);
            ses.PutText(20, 18, login.UtIdBox.UtId);
            ses.PutText(20, 40, login.PasswordBox.Text, ReflectionInterface.Key.Enter);

            //check for all of the error situations
            if (ses.MessageCode == "ON007" || //userid/password wrong
                ses.MessageCode == "ON006") //USERID NOT DEFINED TO RACF (whatever that means)
            {
                Error.Ok("The given ID/Password combo was invalid, please enter your information again.", "Invalid ID or Password");
                return false;
            }
            else if (ses.CheckForText(23, 2, "ON002: YOUR USERID IS REVOKED; CONTACT CLIENT SUPPORT SERVICES"))
            {
                Helper.UI.CreateAndShowDialog<PasswordRevokedForm>(null, owner);
                return false;
            }
            else if (ses.CheckForText(23, 2, "ON009: SECURITY VIOLATION: YOU ARE ALREADY LOGGED IN AT TERMINAL"))
            {
                Error.Ok("You may already be logged on.  Please make sure you are logged off of any Reflection session.  Wait a few minutes and try again.  If the problem persists and you are logged off, please contact the Systems Support Help Desk.", "Already Logged On To System?");
                return false;
            }
            else if (ses.CheckForText(12, 9, "=== CHOOSE ONE OF THE FOLLOWING THREE OPTIONS ===")) //never logged into pheaa
            {
                ses.Hit(ReflectionInterface.Key.F3);//logout
                Error.Ok("The system is detecting that you have not yet agreed to PHEAA's system policies.  Please log into COMPASS normally before using this system.", "Error accessing Compass");

                return false;
            }
            else if (ses.CheckForText(23, 2, "ON09A"))
            {
                Error.Ok("You are logged into the system too many times.  Please close some existing COMPASS sessions.", "Too many Logins");
                return false;
            }
            else if (ses.CheckForText(23, 2, "ON001")) //password expired
            {
                do
                {
                    using PasswordExpiredForm pef = Helper.UI.CreateForm<PasswordExpiredForm>((f) => f.Initialize(ses));
                    if (pef.ShowDialog(owner) == DialogResult.OK)
                        return true;
                    else
                        return false;
                } while (ses.CheckForText(23, 2, "ON003")); //invalid new password
            }

            if (DataAccessHelper.TestMode)
            {
                //be sure that it got passed the login
                if (!ses.Check(TestRegion))
                {
                    Error.Ok("An error occurred while trying to log in.  Please investigate the problem and either try again or contact the Systems Support Help Desk.", "Fatal Error While Logging In");
                    return false;
                }
                //do the STUP thing
                Coordinate coord = ses.FindText("RS/UT");
                ses.PutText(coord.Row, coord.Column - 2, "X", ReflectionInterface.Key.Enter);
            }
            else
            {
                //be sure that it got passed the login
                if (!ses.CheckForText(7, 11, "YOU ARE LOGGED ON TO THE PHEAA NETWORK."))
                {
                    Error.Ok("An error occurred while trying to log in.  Please investigate the problem and either try again or contact the Systems Support Help Desk.", "Fatal Error While Logging In");
                    return false;
                }
            }

            if (!DataAccessHelper.TestMode)
                ses.FastPath("STUPUT");
            else
                ses.FastPath("STUPQ0RS");
            IsLoggedIn = true;
            //cornerstone is gone so no need to display the greeting form
            return true;
        }

        public static string GetLoginWarningMessage()
        {
            if (!ses.WaitForText(16, 10, ">", 1))
                ses.FastPath("LOGOUT");
            //wait for the logon screen to be displayed
            ses.Wait(FirstScreen);
            ses.PutText(16, 12, "PHEAA", ReflectionInterface.Key.Enter, false);
            //wait for the greetings screen to be displayed
            ses.ReflectionSession.WaitForDisplayString("USERID", "0:0:20", 20, 8);

            StringBuilder warningMessage = new StringBuilder();
            for (int i = 1; i <= 18; i++)
            {
                string line = ses.GetText(i, 1, 80);
                if (line.Length > 0)
                    warningMessage.AppendLine(line);
            }
            return warningMessage.ToString();
        }
    }
}