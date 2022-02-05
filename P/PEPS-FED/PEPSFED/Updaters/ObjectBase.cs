using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace PEPSFED
{
    public abstract class ObjectBase
    {
        public abstract long RecordId { get; set; }
        public abstract string RecordType { get; set; }
        public abstract string OpeId { get; set; }

        protected string RemoveSpecialCharacters(string inString)
        {
            List<string> specialCharacters = new List<string>();
            specialCharacters.Add("(");
            specialCharacters.Add(")");
            specialCharacters.Add("#");
            specialCharacters.Add("*");
            specialCharacters.Add("&");
            specialCharacters.Add("%");
            specialCharacters.Add("$");
            specialCharacters.Add("@");
            specialCharacters.Add("!");
            specialCharacters.Add("^");
            specialCharacters.Add("-");
            specialCharacters.Add("_");
            specialCharacters.Add(".");

            foreach (string s in specialCharacters)
                inString = inString.Replace(s, "");

            return inString;
        }

        //Dates in the PEPS file are all "yyyyMMdd" format.
        //A number of sub-classes need to parse a date if it exists, so here's a function they can all use.
        public DateTime ParseDate(string dateString, ObjectBase obj)
        {
            dateString = dateString.Trim();
            if (dateString.Length == 8)
            {
                try
                {
                    int year = int.Parse(dateString.SafeSubString(0, 4));
                    int month = int.Parse(dateString.SafeSubString(4, 2));
                    int day = int.Parse(dateString.Substring(6));

                    return new DateTime(year, month, day);
                }
                catch (Exception ex)
                {
                    Program.PLR.AddNotification(string.Format("Unable to parse date: {0} for peps line: {1}", dateString, obj.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    return default(DateTime);
                }
            }
            else
            {
                return default(DateTime);
            }
        }
    }
}
