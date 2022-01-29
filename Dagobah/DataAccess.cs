using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Dagobah
{
    class DataAccess
    {
        private const string BART_CONNECTION_STRING = @"Data Source=Bart\Bart;Initial Catalog=BSYS;Persist Security Info=True;User ID=procauto;Password=sqlserver;";
        private const string NOCHOUSE_CONNECTION_STRING = @"Data Source=NOCHOUSE;Initial Catalog=BSYS;Persist Security Info=True;User ID=procauto;Password=sqlserver;";
        private static DataContext _bartBsys = new DataContext(BART_CONNECTION_STRING);
        private static DataContext _nochouseBsys = new DataContext(NOCHOUSE_CONNECTION_STRING);
        private static List<User> _users;
        private static object _databaseLock = new object();

        public static User CurrentUser
        {
            get { return Users.Where(p => p.ID == Environment.UserName).Single(); }
        }

        public static List<User> Users
        {
            get
            {
                if (_users == null)
                {
                    StringBuilder queryBuilder = new StringBuilder("SELECT ");
                    queryBuilder.Append("ID, ");
                    queryBuilder.Append("COALESCE(Name, '') AS Name ");
                    queryBuilder.Append("FROM DGBH_LST_Users");
                    lock (_databaseLock) { _users = _bartBsys.ExecuteQuery<User>(queryBuilder.ToString()).ToList(); }
                }
                return _users;
            }
        }//Users

        public enum RequestType
        {
            Dagobah,
            Script,
            Sas,
            Letter
        }

        public static List<Request> GetRequests(string userName, RequestType requestType, bool getRequestsInUsersCourt)
        {
            //Define a SQL-formatted list of statuses we don't want to see when getting all requests.
            string uninterestingStatuses = "'Complete', 'Withdrawn', 'Post-Implementation Queue', 'Post-Implementation Review', 'Publication'";

            //Build a SELECT clause that converts the database types to our Request class properties.
            StringBuilder queryBuilder = new StringBuilder("SELECT ");
            queryBuilder.Append("CONVERT(FLOAT, COALESCE(A.Priority, '0')) AS P, ");
            queryBuilder.Append("COALESCE(A.TopTen, 'False') AS [Top], ");
            if (requestType == RequestType.Dagobah)
            {
                queryBuilder.Append("A.Number, ");
            }
            else
            {
                queryBuilder.Append("A.Request AS Number, ");
            }
            queryBuilder.Append("COALESCE(A.Title, '') AS Title, ");
            queryBuilder.Append("COALESCE(A.Court, '') AS Court, ");
            if (requestType == RequestType.Dagobah)
            {
                queryBuilder.Append("COALESCE(A.Status, '') AS Status, ");
            }
            else
            {
                queryBuilder.Append("COALESCE(A.CurrentStatus, '') AS Status, ");
            }
            if (requestType == RequestType.Letter)
            {
                queryBuilder.Append("COALESCE(A.Requirements, '') AS Summary ");
            }
            else
            {
                queryBuilder.Append("COALESCE(A.Summary, '') AS Summary ");
            }
            //Determine the FROM, JOIN, and WHERE clauses based on request type
            //and whether to only get the requests in the user's court.
            switch (requestType)
            {
                case RequestType.Dagobah:
                    queryBuilder.Append(string.Format("FROM DGBH_DAT_Tasks A WHERE A.Court = '{0}'", userName));
                    break;
                case RequestType.Script:
                    if (getRequestsInUsersCourt)
                    {
                        queryBuilder.Append(string.Format("FROM SCKR_DAT_ScriptRequests A WHERE A.Court = '{0}'", userName));
                    }
                    else
                    {
                        queryBuilder.Append(string.Format("FROM SCKR_DAT_ScriptRequests A INNER JOIN SCKR_REF_Programmer B ON A.Request = B.Request AND B.Class = 'Scr' WHERE B.Programmer = '{0}' AND A.CurrentStatus NOT IN ({1})", userName, uninterestingStatuses));
                    }
                    break;
                case RequestType.Sas:
                    if (getRequestsInUsersCourt)
                    {
                        queryBuilder.Append(string.Format("FROM SCKR_DAT_SASRequests A WHERE A.Court = '{0}'", userName));
                    }
                    else
                    {
                        queryBuilder.Append(string.Format("FROM SCKR_DAT_SASRequests A INNER JOIN SCKR_REF_Programmer B ON A.Request = B.Request AND B.Class = 'SAS' WHERE B.Programmer = '{0}' AND A.CurrentStatus NOT IN ({1})", userName, uninterestingStatuses));
                    }
                    break;
                case RequestType.Letter:
                    if (getRequestsInUsersCourt)
                    {
                        queryBuilder.Append(string.Format("FROM LTDB_DAT_Requests A WHERE A.Court = '{0}'", userName));
                    }
                    else
                    {
                        queryBuilder.Append(string.Format("FROM LTDB_DAT_Requests A WHERE A.SOC = '{0}' AND A.CurrentStatus NOT IN ({1})", userName, uninterestingStatuses));
                    }
                    break;
            }
            //Return the command results in order of priority. Dagobah tasks are in BART, so use the BART user if we're looking up Dagobah.
            DataContext bsys = (requestType == RequestType.Dagobah ? _bartBsys : _nochouseBsys);
            lock (_databaseLock) { return bsys.ExecuteQuery<Request>(queryBuilder.ToString()).OrderByDescending(p => p.P).ToList(); }
        }//GetRequests()

        public static List<string> GetSkins()
        {
            List<string> skinList = Directory.GetDirectories(Skin.SkinsDirectory).ToList();
            //Directory.GetDirectories() returns full path names, so chop off the path.
            for (int i = 0; i < skinList.Count; i++)
            {
                skinList[i] = skinList[i].Substring(skinList[i].LastIndexOf('\\') + 1);
            }
            return skinList;
        }//GetSkins()
    }//class
}//namespace
