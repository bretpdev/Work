using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

namespace UHEAAOperationsTrackingSystems
{
    public class LookCoordinator
    {

        //must be kept in sync with looks added in constructor below
        public enum SystemLook
        {
            NotSet,
            DefaultAndPortal,
            SystemAccess,
            NeedHelp,
            ChangePassword,
            FlowControl
        }

        static private List<Look> _looks;

        static LookCoordinator()
        {
            //create list off all look information (must be coordinated with enum above)
            _looks = new List<Look>();
            _looks.Add(new Look(string.Empty, string.Empty, string.Empty, SystemLook.DefaultAndPortal));
            _looks.Add(new Look("cf8d23","SystemAccess.gif","System Access", SystemLook.SystemAccess));
            _looks.Add(new Look("5ea86f", "NeedHelp.gif", "Need Help", SystemLook.NeedHelp));
            _looks.Add(new Look("636363", "PasswordReset.gif", "Password Reset", SystemLook.ChangePassword));
            _looks.Add(new Look("4f54ea", "FlowControl.gif", "ACDC Flows", SystemLook.FlowControl));
            //When inheriting the BaseContentPage object you must set the _systemLook variable to a value other than NotSet.
        }

        /// <summary>
        /// Gets master page look information for passed in system
        /// </summary>
        /// <param name="sessionVariableText"></param>
        /// <returns></returns>
        static public Look GetLook(SystemLook system)
        {
            //check for error condition
            if (system == SystemLook.NotSet)
            {
                throw new LookNotAssignedException("When inheriting the BaseContentPage object you must set the _systemLook variable to a value other than NotSet.  In order to do this you'll need to override the child class' OnInit() method and assign the _systemLook variable a value before the parent class' OnInit() method is called.  Please contact a member of the programming team if you received this error in production.");
            }
            //return correct look if error condition not found
            return (from l in _looks
                    where l.SysLook == system
                    select l).First();
        }

    }
}
