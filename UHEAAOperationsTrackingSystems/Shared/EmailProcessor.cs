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
using OSSMTP;

namespace UHEAAOperationsTrackingSystems
{
    public class EmailProcessor
    {

        public enum EmailImportanceLevel
        {
            High,
            Low,
            Normal
        }

        public static void SendMail(bool testMode, string mto, string mFrom, string mSubject, string mBody, string mCC, string mBCC, EmailImportanceLevel mImportance, bool testIt, bool AsHTML)
        {
            SMTPSession Email = new SMTPSession() ;
            int SecondsWaited = 0;
            importance_level importanceLevel;
            switch (mImportance)
            {
                case EmailImportanceLevel.High:
                    importanceLevel = importance_level.ImportanceHigh;
                    break;
                case EmailImportanceLevel.Low:
                    importanceLevel = importance_level.ImportanceLow;
                    break;
                default:
                    importanceLevel = importance_level.ImportanceNormal;
                    break;
            }

            //set server and time out (default = 10 secs, set to 20 secs)
            Email.Server = "mail.utahsbr.edu";
            Email.Timeout = 20000;

            //add test text and recipient
            if (testIt && testMode)
            {
                //////mto = string.Format("{0},{1}@utahsbr.edu",mto, Environment.UserName);
                mSubject = string.Format("{0} -- THIS IS A TEST", mSubject);
                mBody = string.Format("THIS IS A TEST{0}{0}{1}", Environment.NewLine, mBody);
            }

            ////////create message
            //////if (mFrom.Length == 0)
            //////{
            //////    mFrom = Environment.UserName + "@utahsbr.edu";
            //////}
            Email.MailFrom = mFrom;
            Email.SendTo = mto;
            Email.CC = mCC;
            Email.BCC = mBCC;
            Email.MessageSubject = mSubject;
            if (AsHTML)
            {
                Email.MessageHTML = mBody;
            }
            else
            {
                Email.MessageText = mBody;
            }
            Email.Importance = importanceLevel;

            //send message
            Email.SendEmail();

            //wait up to five seconds for the email to successfully be sent
            while (Email.Status != "SMTP connection closed")
	        {
    	        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 1));
                SecondsWaited++;
                if (SecondsWaited == 5 && Email.Status != "SMTP connection closed")
                {
                    //if the script has waited 5 seconds then give error message and exit function
                    throw new EmailException("Your message was not sent for the following reason:  " + Email.Status + "  Please contact Process Automation for assistance.");
                }   
	        }
            //if the script makes it through loop above then it successfully sent the email and received the all is OK message
            }

    }
}
