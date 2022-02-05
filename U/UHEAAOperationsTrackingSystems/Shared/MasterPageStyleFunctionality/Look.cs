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
using System.IO;

namespace UHEAAOperationsTrackingSystems
{
    public class Look
    {

        private string _titleText;
        public string TitleText
        {
            get
            {
                return _titleText;
            }
        }

        private LookCoordinator.SystemLook _sysLook;
        public LookCoordinator.SystemLook SysLook
        {
            get
            {
                return _sysLook;
            }
        }

        private string _cssText = string.Empty;
        public string CSSText
        {
            get
            {
                return _cssText;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexColorNumber">Do not include "#" in hex number.</param>
        /// <param name="headerImageFileName">File name only.  No path, because all images are in one place.</param>
        /// <param name="titleText"></param>
        /// <param name="sysLook"></param>
        public Look(string hexColorNumber, string headerImageFileName, string titleText, LookCoordinator.SystemLook sysLook)
        {
            _titleText = titleText;
            _sysLook = sysLook;
            if (sysLook != LookCoordinator.SystemLook.DefaultAndPortal)
            {
                _cssText = Properties.Resources.MasterPageGeneralCSS.ToString();
                //for some stupid reason string.format throws and exception so we used to replace calls (Daren says it has to do with the properties encoding.
                _cssText = _cssText.Replace("{0}", hexColorNumber);
                _cssText = _cssText.Replace("{1}", headerImageFileName);
            }
        }

    }
}
