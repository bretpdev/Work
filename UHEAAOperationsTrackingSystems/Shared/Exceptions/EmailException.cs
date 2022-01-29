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

namespace UHEAAOperationsTrackingSystems
{
    public class EmailException : Exception
    {

        public EmailException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public EmailException(string message)
            : base(message)
        {
        }

        public EmailException()
            : base()
        {
        }
    }
}
