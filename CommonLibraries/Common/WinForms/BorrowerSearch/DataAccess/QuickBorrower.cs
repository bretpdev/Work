using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace Uheaa.Common.WinForms
{
    public class QuickBorrower
    {
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string DOB { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string Zip { get; set; }
        public string HomePhone { get; set; }
        public bool HomePhoneConsent { get; set; }
        public string WorkPhone { get; set; }
        public bool WorkPhoneConsent { get; set; }
        public string AlternatePhone { get; set; }
        public bool AlternatePhoneConsent { get; set; }
        public string HomeEmail { get; set; }
        public string WorkEmail { get; set; }
        public string AlternateEmail { get; set; }
        [DbIgnore]
        public string Emails { get { return string.Join(";", new string[] { HomeEmail, WorkEmail, AlternateEmail }.Where(o => !o.IsNullOrEmpty()).ToArray()); } }
        [DbIgnore]
        public string Region { get { return RegionEnum.ToString(); } }
        [DbIgnore]
        public RegionSelectionEnum RegionEnum { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + (string.IsNullOrEmpty(MiddleInitial) ? "" : MiddleInitial + " ") + LastName;
            }
        }
        public string FullAddress
        {
            get
            {
                return Address1 + (Address2.IsNullOrEmpty() ? "" : (" " + Environment.NewLine + Address2));
            }
        }


        [UsesSproc(DataAccessHelper.Database.Udw, "QuickBorrowerSearch")]
        public static List<QuickBorrower> QuickSearch(string firstName, string lastName, LogDataAccess lda)
        {
            var totalResults = new List<QuickBorrower>();
            var parameters = new SqlParameter[] { SqlParams.Single("FirstName", firstName), SqlParams.Single("LastName", lastName) };

            var searchDatabase = new Action<DataAccessHelper.Database, RegionSelectionEnum>((db, region) =>
            {
                List<QuickBorrower> results;
                if (lda != null)
                    results = lda.ExecuteList<QuickBorrower>("QuickBorrowerSearch", db, parameters).Result;
                else
                    results = DataAccessHelper.ExecuteList<QuickBorrower>("QuickBorrowerSearch", db, parameters);
                foreach (var borrower in results)
                {
                    borrower.RegionEnum = region;
                    borrower.DOB = borrower.DOB.ToDateNullable()?.ToShortDateString();
                }
                totalResults.AddRange(results);
            });
            searchDatabase(DataAccessHelper.Database.Udw, RegionSelectionEnum.Uheaa);
            searchDatabase(DataAccessHelper.Database.Odw, RegionSelectionEnum.OneLINK);

            return totalResults;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "BorrowersSearch")]
        [UsesSproc(DataAccessHelper.Database.Odw, "BorrowersSearch")]
        public static List<QuickBorrower> Search(SearchBorrower template, RegionSelectionEnum searchType, LogDataAccess lda, bool includeUdw = true, bool includeOdw = true)
        {
            StringHelper.Sanitize(template);
            template.FormatForSearch();
            var totalResults = new List<QuickBorrower>();
            var parameters = SqlParams.Generate(template).ToArray();

            var searchDatabase = new Action<DataAccessHelper.Database, RegionSelectionEnum>((db, region) =>
            {
                if (searchType == RegionSelectionEnum.All || searchType == region)
                {
                    List<QuickBorrower> results;
                    if (lda != null)
                        results = lda.ExecuteList<QuickBorrower>("BorrowersSearch", db, parameters).Result;
                    else
                        results = DataAccessHelper.ExecuteList<QuickBorrower>("BorrowersSearch", db, parameters);
                    foreach (var borrower in results)
                        borrower.RegionEnum = region;
                    totalResults.AddRange(results);
                }
            });
            if (includeUdw)
                searchDatabase(DataAccessHelper.Database.Udw, RegionSelectionEnum.Uheaa);
            if (includeOdw)
                searchDatabase(DataAccessHelper.Database.Odw, RegionSelectionEnum.OneLINK);
            return totalResults;
        }
    }
}