using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uheaa.Common.DataAccess
{
    public static partial class DataAccessHelper
    {
        public static int MaxConnectionAgeInSeconds { get; set; }
        private static Thread ConnectionMonitorThread { get; set; }
        public class MonitoredConnection
        {
            public SqlConnection Connection { get; set; }
            public DateTime LastUsed { get; set; }
            public int AgeInSeconds
            {
                get
                {
                    return (int)(DateTime.Now - LastUsed).TotalSeconds;
                }
            }
        }
        public class DbAndMode
        {
            public DbAndMode(Database db, Mode mode)
            {
                DB = db;
                Mode = mode;
            }
            public Database DB { get; set; }
            public Mode Mode { get; set; }
            public override bool Equals(object obj)
            {
                var otherObject = obj as DbAndMode;
                if (otherObject == null)
                    return false;
                return otherObject.GetHashCode() == this.GetHashCode();
            }
            public override int GetHashCode()
            {
                return (DB.ToString() + Mode.ToString()).GetHashCode();
            }
        }
        public static ConcurrentDictionary<DbAndMode, MonitoredConnection> MonitoredConnections = new ConcurrentDictionary<DbAndMode, MonitoredConnection>();

        public static SqlConnection GetManagedConnection(Database db, Mode mode)
        {

            var dbmode = new DbAndMode(db, mode);
            var existing = MonitoredConnections.ContainsKey(dbmode) ? MonitoredConnections[dbmode] : null;
            if (existing == null || existing.Connection.State == System.Data.ConnectionState.Broken)
            {
                existing = new MonitoredConnection();
                existing.Connection = new SqlConnection(GetConnectionString(db, mode));
                existing.LastUsed = DateTime.Now;
                existing.Connection.Open();
                MonitoredConnections[dbmode] = existing;
            }
            else if (existing.Connection.State == System.Data.ConnectionState.Closed)
                existing.Connection.Open();
            else
                existing.LastUsed = DateTime.Now;

            if (ConnectionMonitorThread == null)
            {
                ConnectionMonitorThread = new Thread(CheckConnectionTimeouts);
                ConnectionMonitorThread.Start();
            }
            return existing.Connection;
        }
        static bool shouldCloseAllManagedConnections = false;
        public static void CheckConnectionTimeouts()
        {
            while (MonitoredConnections.Any())
            {
                Thread.Sleep(5000);
                foreach (var key in MonitoredConnections.Keys.ToArray())
                {
                    MonitoredConnection conn = null;
                    if (MonitoredConnections.TryGetValue(key, out conn))
                    {
                        bool old = conn.AgeInSeconds >= MaxConnectionAgeInSeconds && conn.Connection.State == System.Data.ConnectionState.Open;
                        if (old || shouldCloseAllManagedConnections)
                        {
                            MonitoredConnection notUsed;
                            MonitoredConnections.TryRemove(key, out notUsed);
                            conn.Connection.Close();
                        }
                    }
                }
            }
            ConnectionMonitorThread = null;
        }
        public static void CloseAllManagedConnections()
        {
            shouldCloseAllManagedConnections = true;
        }
    }
}
