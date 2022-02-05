using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;

namespace ADDMOREFED
{
    class DataAccess 
    {
        /// <summary>
		/// get a list of valid domestic state codes
        /// </summary>
        /// <returns>Valid domestic state codes</returns>
		[UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetStateCodes")]
        public IEnumerable<string> GetStateCodes()
        {
			return DataAccessHelper.ExecuteList<string>("spGENR_GetStateCodes", DataAccessHelper.Database.Csys, new SqlParameter("IncludeTerritories", true));
        }

		/// <summary>
		/// get a state name based on the 2 letter code
		/// </summary>
		/// <returns>State Name string</returns>
		[UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetStateNameForStateCode")]
        public string GetStateNameFromStateCode(string stateCode)
        {
            return DataAccessHelper.ExecuteList<string>("spGENR_GetStateNameForStateCode", DataAccessHelper.Database.Csys, new SqlParameter("StateCode", stateCode)).SingleOrDefault();
        }

        /// <summary>
        /// Get a list of sources from which an address may have come
        /// </summary>
        /// <returns>list of address sources</returns>
		[UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetAddressSourceDescriptions")]
        public IEnumerable<string> GetAddressSourceDescriptions()
        {
            return DataAccessHelper.ExecuteList<string>("spGENR_GetAddressSourceDescriptions", DataAccessHelper.Database.Csys);
        }

        /// <summary>
        /// Get the source code of the description
        /// </summary>
        /// <param name="description">string for the source of the address (ie parent, certified mail, etc)</param>
        /// <returns>string representing the source of the address</returns>
		[UsesSproc(DataAccessHelper.Database.Csys, "spGENR_getAddressSourceCodeForDescription")]
        public string GetAddressSourceCodeForDescription(string description)
        {
            return DataAccessHelper.ExecuteList<string>("spGENR_getAddressSourceCodeForDescription", DataAccessHelper.Database.Csys, new SqlParameter("Description", description)).SingleOrDefault();
        }

        /// <summary>
		/// get a list of country codes
        /// </summary>
		[UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetCountryNames")]
        public IEnumerable<string> GetCountryNames()
        {
            return DataAccessHelper.ExecuteList<string>("spGENR_GetCountryNames", DataAccessHelper.Database.Csys);
        }

        /// <summary>
		/// get the name of the country for the country code provided
        /// </summary>
		/// <param name="name">name of the country</param>
		[UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetCountryCodeForName")]
        public string GetCountryCodeForName(string name)
        {
            return DataAccessHelper.ExecuteList<string>("spGENR_GetCountryCodeForName", DataAccessHelper.Database.Csys, new SqlParameter("Name", name)).SingleOrDefault();
        }

		/// <summary>
		/// get the description for the relationship code provided 
		/// </summary>
		/// <param name="code">relationship code</param>
		/// <returns>Description of the relationship</returns>
		[UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetRelationshipDescriptionForCode")]
        public string GetRelationshipDescriptionForCode(string code)
        {
            return DataAccessHelper.ExecuteList<string>("spGENR_GetRelationshipDescriptionForCode", DataAccessHelper.Database.Csys, new SqlParameter("Code",code)).SingleOrDefault();
        }

		/// <summary>
		/// get a list of relationship descriptions
		/// </summary>
		/// <returns>list of relationship descriptions</returns>
		[UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetRelationshipDescriptions")]
        public IEnumerable<string> GetRelationshipDescriptions()
        {
            return DataAccessHelper.ExecuteList<string>("spGENR_GetRelationshipDescriptions", DataAccessHelper.Database.Csys);
        }

		/// <summary>
		/// Takes a relationship description and returns the code for that relationship
		/// </summary>
		/// <param name="description">relationship description</param>
		/// <returns>relationship code</returns>
		[UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetRelationshipCodeForDescription")]
        public string GetRelationshipCodeForDescription(string description)
        {
			return DataAccessHelper.ExecuteList<string>("spGENR_GetRelationshipCodeForDescription", DataAccessHelper.Database.Csys, new SqlParameter("description", description)).SingleOrDefault();
        }

		/// <summary>
		/// get a list of suffixes
		/// </summary>
		/// <returns>List of strings (suffixes)</returns>
        public IEnumerable<string> GetSuffixes()
        {
			return DataAccessHelper.ExecuteList<string>("spGENR_GetSuffixes", DataAccessHelper.Database.Csys);
        }
    }
}
