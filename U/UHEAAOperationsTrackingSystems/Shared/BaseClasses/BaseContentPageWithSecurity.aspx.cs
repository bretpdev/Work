using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace UHEAAOperationsTrackingSystems
{
    public partial class BaseContentPageWithSecurity : BaseContentPage
    {

        protected string _uheaaAccessKey;
        protected string _uheaaSystem;

        protected override void OnInit(EventArgs e)
        {
            if (_uheaaAccessKey == null || _uheaaAccessKey.Length == 0 || _uheaaSystem == null || _uheaaSystem.Length == 0)
            {
                throw new SecurityVariablesNotSetToValueException("When inheriting the BaseContentPageWithSecurity object you must set the _uheaaAccessKey and _uheaaSystem variables to a value.  In order to do this you'll need to override the child class' OnInit() method and assign the _uheaaAccessKey and _uheaaSystem variables a value before the parent class' OnInit() method is called.  Please contact a member of the programming team if you received this error in production.");
            }
            if (DataAccessBase.HasAccess(_uheaaAccessKey, this, _uheaaSystem) == false)
            {
                Response.Redirect("/Portal/Portal.aspx");
            }
            base.OnInit(e);
        }

    }
}
