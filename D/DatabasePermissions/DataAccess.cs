using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Security.Principal;

namespace DatabasePermissions
{
	class DataAccess
	{
        const string UHEAA_DIRECTORY_PATH = "LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local";

		private static IEnumerable<string> _databaseNames;
        private static IEnumerable<string> _activeDirectoryGroups;

		public static IEnumerable<string> DatabaseNames
		{
			get
			{
				if (_databaseNames == null)
				{
					DataContext dc = new DataContext("Data Source=OPSDEV;Initial Catalog=master;Integrated Security=SSPI;");
					string query = "SELECT [name] FROM sys.databases WHERE [name] NOT IN ('master', 'tempdb', 'model', 'msdb')";
					_databaseNames = dc.ExecuteQuery<string>(query).ToList();
				}
				return _databaseNames;
			}
		}

		public static IEnumerable<string> EntityTypes { get { return new string[] { "Table", "Stored Procedure" }; } }

        public static IEnumerable<string> ActiveDirectoryGroups
        {
            get
            {
                if (_activeDirectoryGroups == null)
                {
          //        Public Shared Function GetActiveDirectoryGroups(ByVal securityId As String) As IEnumerable(Of String)
                    DirectoryEntry searchEntry = new DirectoryEntry(UHEAA_DIRECTORY_PATH);
                    DirectorySearcher searcher = new DirectorySearcher();
                    searcher.SearchRoot = searchEntry;
    //                searcher.Filter = "(&(objectClass=group))";
    //                searcher.PropertiesToLoad.Add("CN");

                    searcher.Filter = "samaccountname=lneill";
                    searcher.PropertiesToLoad.Add("memberOf");
                    SearchResult result = searcher.FindOne();


    //                SearchResultCollection allGroups = searcher.FindAll();
    
                    List<string> groupMemberships = new List<string>();

    //                if (allGroups != null)
                    if (result != null)
                    {
                        const string CONTAINER_NAME_INDICATOR = "CN=";

                        foreach (object fullyQualifiedGroupObject in result.Properties["memberof"])

         //               foreach (object fullyQualifiedGroupObject in result.Properties["name"])
        //                foreach (SearchResult fullyQualifiedGroupObject in allGroups)
                        {
                            string group = fullyQualifiedGroupObject.ToString();
                            string fullyQualifiedGroupName = fullyQualifiedGroupObject.ToString();
                            //string containerNameIndex = fullyQualifiedGroupName.IndexOf(CONTAINER_NAME_INDICATOR, StringComparison.CurrentCultureIgnoreCase);
                            //int commaIndex = fullyQualifiedGroupName.IndexOf(",", containerNameIndex); ing fullyQualifiedGroupName = fullyQualifiedGroupObject.ToString();
                            //string group = fullyQualifiedGroupName.Substring(containerNameIndex +
                            //     CONTAINER_NAME_INDICATOR.Length, commaIndex - containerNameIndex - CONTAINER_NAME_INDICATOR.Length);
                            groupMemberships.Add(group);
                        }
                        _activeDirectoryGroups = groupMemberships;
                        return _activeDirectoryGroups;
                    }
                    return _activeDirectoryGroups;
                }
                return _activeDirectoryGroups;
            }
        }
                
		public static IEnumerable<Entity> GetEntities(string database, string entityType, string groupName)
		{
			string connectionString = string.Format("Data Source=OPSDEV;Initial Catalog={0};Integrated Security=SSPI;", database);
			DataContext dc = new DataContext(connectionString);
			StringBuilder queryBuilder = new StringBuilder("SELECT ent.[Name],");
			queryBuilder.Append(" CASE WHEN ins.type IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS [Insert],");
			queryBuilder.Append(" CASE WHEN updt.type IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS [Update],");
			queryBuilder.Append(" CASE WHEN del.type IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS [Delete],");
			queryBuilder.Append(" CASE WHEN sel.type IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS [Select],");
			queryBuilder.Append(" CASE WHEN ex.type IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS [Execute]");
			queryBuilder.AppendFormat(" FROM sys.{0} ent", (entityType == "Table" ? "tables" : "procedures"));
			queryBuilder.Append(" LEFT OUTER JOIN sys.database_permissions ins ON ent.object_id = ins.major_id AND ins.permission_name = 'INSERT'");
			queryBuilder.AppendFormat(" LEFT OUTER JOIN sys.database_principals ins_prin ON ins.grantee_principal_id = ins_prin.principal_id AND ins_prin.name = '{0}'", groupName);
			queryBuilder.Append(" LEFT OUTER JOIN sys.database_permissions updt ON ent.object_id = updt.major_id AND updt.permission_name = 'UPDATE'");
			queryBuilder.AppendFormat(" LEFT OUTER JOIN sys.database_principals updt_prin ON updt.grantee_principal_id = updt_prin.principal_id AND updt_prin.name = '{0}'", groupName);
			queryBuilder.Append(" LEFT OUTER JOIN sys.database_permissions del ON ent.object_id = del.major_id AND del.permission_name = 'DELETE'");
			queryBuilder.AppendFormat(" LEFT OUTER JOIN sys.database_principals del_prin ON del.grantee_principal_id = del_prin.principal_id AND del_prin.name = '{0}'", groupName);
			queryBuilder.Append(" LEFT OUTER JOIN sys.database_permissions sel ON ent.object_id = sel.major_id AND sel.permission_name = 'SELECT'");
			queryBuilder.AppendFormat(" LEFT OUTER JOIN sys.database_principals sel_prin ON sel.grantee_principal_id = sel_prin.principal_id AND sel_prin.name = '{0}'", groupName);
			queryBuilder.Append(" LEFT OUTER JOIN sys.database_permissions ex ON ent.object_id = ex.major_id AND ex.permission_name = 'EXECUTE'");
			queryBuilder.AppendFormat(" LEFT OUTER JOIN sys.database_principals ex_prin ON ex.grantee_principal_id = ex_prin.principal_id AND ex_prin.name = '{0}'", groupName);
			queryBuilder.Append(" ORDER BY ent.[Name]");
			return dc.ExecuteQuery<Entity>(queryBuilder.ToString()).ToList();
		}
	}//class
}//namespace
