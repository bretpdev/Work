using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDIntermediary
{
    public class Country
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public static List<Country> GetCountries()
        {
            return DataAccessHelper.ExecuteList<Country>("GetCountries", DataAccessHelper.Database.MauiDude);
        }
        public static string GetCountryCode(string countryName)
        {
            return GetCountries().Single(o => o.CountryName.ToLower() == countryName.ToLower()).CountryCode;
        }
    }
}
