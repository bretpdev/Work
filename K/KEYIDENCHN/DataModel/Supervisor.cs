using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KEYIDENCHN
{
    public class Supervisor
    {
        public int SqlUserId { get; set; }
        public string WindowsUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UtId { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName + " (" + UtId + ")"; ;
        }

        public static IEnumerable<Supervisor> LoadIds(Supervisor template)
        {
            foreach (string utid in DataAccess.GetSupervisorUtIds(template.WindowsUserName))
            {
                Supervisor newSup = (Supervisor)template.MemberwiseClone();
                newSup.UtId = utid;
                yield return newSup;
            }
        }

        private static List<Supervisor> cachedSupervisors;
        public static List<Supervisor> CachedSupervisors
        {
            get
            {
                if (cachedSupervisors == null)
                    cachedSupervisors = DataAccess.GetAllSupervisors();
                return cachedSupervisors;
            }
        }

        public static void LoadSupervisors()
        {
            var load = CachedSupervisors; //trigger cache by accessing getter
        }
    }
}
