using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace REINSTREV
{
    public class Borrower
    {
        public string Ssn { get; set; }
        public SystemBorrowerDemographics Demographics { get; set; }

        public string SchoolName { get; set; }
        public string SchoolAddress1 { get; set; }
        public string SchoolAddress2 { get; set; }
        public string SchoolCity { get; set; }
        public string SchoolState { get; set; }
        public string SchoolZip { get; set; }
        public string SchoolCountry { get; set; }

        /// <summary>
        /// Load borrower and school information from the session
        /// </summary>
        public bool LoadFromLP9AC(ReflectionInterface ri)
        {
            Ssn = ri.GetText(17, 70, 9);
            SchoolName = ri.GetTextRemoveUnderscores(12, 11, 30);
            SchoolAddress1 = ri.GetTextRemoveUnderscores(12, 61, 8) + ri.GetTextRemoveUnderscores(13, 11, 22);
            SchoolAddress2 = ri.GetTextRemoveUnderscores(13, 53, 16) + ri.GetTextRemoveUnderscores(14, 11, 14);
            {   //parse school city/state/zip/country block
                string cityStateZip = ri.GetTextRemoveUnderscores(14, 45, 24) + ri.GetTextRemoveUnderscores(15, 11, 58);
                string[] cityStateZipSplit = cityStateZip.Split('/');
                if (cityStateZipSplit.Length != 4) //must have city, state, and zip
                {
                    Dialog.Warning.Ok("The school information is missing or is incorrectly formatted.  Contact the user who created the task to get the school information and complete the task manually.", "School Information");
                    return false;
                }
                SchoolCity = cityStateZipSplit[0];
                SchoolZip = cityStateZipSplit[2];
                if (SchoolZip.Length > 5) //format long zip code with dash
                    SchoolZip = string.Format("#####-####");
                string possibleCountry = cityStateZipSplit[3];
                if (possibleCountry.StartsWith("US"))
                    SchoolState = cityStateZipSplit[1];
                else 
                    SchoolCountry = possibleCountry.Substring(10);  //element [3] starts with " (UT00###)"[10 chars long], and a Country may follow

            }
            this.Demographics = ri.GetDemographicsFromLP22(Ssn);
            this.Demographics.AccountNumber = this.Demographics.AccountNumber.Replace(" ", "");

            return true;
        }
    }
}
