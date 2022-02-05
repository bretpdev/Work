using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace UHEAAOperationsTrackingSystems
{
	public class DataAccessBase
	{

		public const string DEFAULT_DROP_DOWN_OPTION = "Please Select...";

		private static DataContext _bsysDataContext;
		protected static DataContext BsysDataContext
		{
			get
			{
				if (_bsysDataContext == null)
				{
					string conn = Properties.Settings.Default.TestMode ? ConfigurationManager.ConnectionStrings["TestBSYSConnectionString"].ConnectionString : ConfigurationManager.ConnectionStrings["BSYSConnectionString"].ConnectionString;
					_bsysDataContext = new DataContext(conn);
				}
				return _bsysDataContext;
			}
		}

		private static DataContext _csysDataContext;
		protected static DataContext CSYSDataContext
		{
			get
			{
				if (_csysDataContext == null)
				{
					string conn = Properties.Settings.Default.TestMode ? ConfigurationManager.ConnectionStrings["TestCSYSConnectionString"].ConnectionString : ConfigurationManager.ConnectionStrings["CSYSConnectionString"].ConnectionString;
					_csysDataContext = new DataContext(conn);
				}
				return _csysDataContext;
			}
		}

		#region "DB Selects"

		/// <summary>
		/// Gathers all possible keys a given user has access to for a given system.  If system is "NO_SYSTEM" then method returns all keys a user has access to.
		/// </summary>
		/// <param name="system">Pass "NO_SYSTEM" for all keys a user has access to.</param>
		/// <param name="user"></param>
		/// <returns></returns>
		public static List<UserAccessKey> GatherExistingKeysForSystemUserCombo(string system, int user)
		{
			string selectQuery = "EXEC spSYSA_UserAccessForApplication {0}, {1}";
			return CSYSDataContext.ExecuteQuery<UserAccessKey>(selectQuery, user, system).ToList();
		}

		/// <summary>
		/// Checks if a user has access for a specified key
		/// </summary>
		/// <param name="accessKey"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool HasAccess(string accessKey, Page thePage, string system)
		{
			//only gather information for the DB if not gathered already
			if (thePage.Session[BaseContentPage.ACCESS_LIST_SESSION_VARIABLE_TEXT] == null)
			{
				//create session variable for list of access
				thePage.Session[BaseContentPage.ACCESS_LIST_SESSION_VARIABLE_TEXT] = GatherExistingKeysForSystemUserCombo("NO_SYSTEM", (int)thePage.Session[BaseContentPage.SQL_USER_ID]);
			}
			//get list our of session variable
			List<UserAccessKey> usersAccessKeys = ((List<UserAccessKey>)thePage.Session[BaseContentPage.ACCESS_LIST_SESSION_VARIABLE_TEXT]);
			//check if the user has access to the passed in key
			return (0 < (from k in usersAccessKeys
						 where k.Name == accessKey && k.Application == system
						 select k).Count());
		}

		/// <summary>
		/// Checks if a user has access for a specified key and business unit combo
		/// </summary>
		/// <param name="accessKey"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool HasAccess(string accessKey, Page thePage, string system, string businessUnit)
		{
			//only gather information for the DB if not gathered already
			if (thePage.Session[BaseContentPage.ACCESS_LIST_SESSION_VARIABLE_TEXT] == null)
			{
				//create session variable for list of access
				thePage.Session[BaseContentPage.ACCESS_LIST_SESSION_VARIABLE_TEXT] = GatherExistingKeysForSystemUserCombo("NO_SYSTEM", (int)thePage.Session[BaseContentPage.SQL_USER_ID]);
			}
			//get list our of session variable
			List<UserAccessKey> usersAccessKeys = ((List<UserAccessKey>)thePage.Session[BaseContentPage.ACCESS_LIST_SESSION_VARIABLE_TEXT]);
			//check if the user has access to the passed in key
			return (0 < (from k in usersAccessKeys
						 where k.Name == accessKey && k.Application == system && k.BusinessUnitName == businessUnit
						 select k).Count());
		}

		/// <summary>
		/// Gets all systems listed for portal.
		/// </summary>
		/// <returns></returns>
		public static List<string> GetSystems()
		{
			return BsysDataContext.ExecuteQuery<string>("SELECT DISTINCT Script FROM SCKR_DAT_Scripts").ToList();
		}

		public static List<DisplayUser> GetSystemUsers()
		{
			List<DisplayUser> du;
			try
			{
				string selectQuery = "EXEC spSYSA_GetSystemUsers";
				du = CSYSDataContext.ExecuteQuery<DisplayUser>(selectQuery).ToList();
				du.Insert(0, new DisplayUser { LegalName = "", ID = 0 });
			}
			catch (SqlException ex)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw;
			}
			return du;
		}

		/// <summary>
		/// Gets users full legal name.
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		public static string GetUsersFullName(string userID)
		{
			List<DisplayUser> users = GetSystemUsers();
			return (from u in users
					where u.LegalName == userID
					select u.LegalName).First<string>();
		}

		public static List<Key> GatherExistingKeysBasedOffFilterCriteria(string system, string type, string keyWord, string keyWordFieldFilter)
		{
			if (system == null) { system = string.Empty; }
			if (type == null) { type = string.Empty; }
			if (keyWord == null) { keyWord = string.Empty; }
			if (keyWordFieldFilter == null) { keyWordFieldFilter = string.Empty; }
			string selectQuery = "EXEC spSYSA_GatherKeyDataBasedOffFilterCriteria {0}, {1}, {2}, {3}";
			List<Key> k;
			try
			{
				k = CSYSDataContext.ExecuteQuery<Key>(selectQuery, system, type, keyWord, keyWordFieldFilter).ToList();
			}
			catch (SqlException ex)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw;
			}
			return k;
		}

		/// <summary>
		/// Gets all steps for a given flow.
		/// </summary>
		/// <param name="flowID"></param>
		/// <returns></returns>
		public static List<FlowStep> GetStepsForSpecifiedFlow(string flowID)
		{
			string selectStr = "EXEC spFLOW_GetStepsForFlow {0}";
			return CSYSDataContext.ExecuteQuery<FlowStep>(selectStr, flowID).ToList();
		}

		/// <summary>
		/// Retrieves specified flow from DB.
		/// </summary>
		/// <param name="flowID"></param>
		/// <returns></returns>
		public static Flow GetSpecifiedFlow(string flowID)
		{
			string selectStr = "EXEC spFLOW_GetSpecifiedFlow {0}";
			return CSYSDataContext.ExecuteQuery<Flow>(selectStr, flowID).First();
		}

		public static int GetSqlUserId(string userName)
		{
			string selectStr = @"EXEC spACDC_GetSquid {0}";
			return CSYSDataContext.ExecuteQuery<int>(selectStr, userName).SingleOrDefault();
		}

		#endregion
	}
}
