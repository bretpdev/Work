using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace EmployeeHistory
{
    class AvatierHistory
    {
        public int AvatierHistoryId { get; set; }
        public string EmployeeId { get; set; }
        public string UserGuid { get; set; }
        public string Role { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string ManagerEmployeeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public UpdateType UpdateTypeId { get; set; }
        public DateTime AddedAt { get; set; }
        public string AddedBy { get; set; }

        public static AvatierHistory Parse(usersUser user)
        {
            var hist = new AvatierHistory();
            hist.EmployeeId = user.AuthParam;
            hist.UserGuid = user.UserGUID;
            hist.ManagerEmployeeId = user.ManagerID;
            hist.Role = user.Role_dropdown;
            hist.Title = user.Position;
            hist.Department = user.Job_Template_Name;
            hist.FirstName = user.first_name;
            hist.MiddleName = user.middle_name;
            hist.LastName = user.last_name;
            hist.HireDate = user.Offer_Date.ToDateNullable() ?? user.HireDate.ToDateNullable();
            hist.TerminationDate = user.TerminationDate.ToDateNullable();
            if (hist.TerminationDate.HasValue)
            {
                //Termination Date has no time, default to 7:30pm
                hist.TerminationDate = hist.TerminationDate.Value.Date.AddHours(19).AddMinutes(30);
            }
            return hist;
        }

        public static bool SameHistory(AvatierHistory one, AvatierHistory two)
        {
            return one.EmployeeId == two.EmployeeId
                && one.Role == two.Role
                && one.Title == two.Title
                && one.Department == two.Department
                && one.ManagerEmployeeId == two.ManagerEmployeeId
                && one.FirstName == two.FirstName
                && one.MiddleName == two.MiddleName
                && one.LastName == two.LastName
                && one.HireDate == two.HireDate
                && one.TerminationDate == two.TerminationDate;
        }

        public static bool SamePropertyPushFields(AvatierHistory one, AvatierHistory two)
        {
            return one.Title == two.Title
                && one.Department == two.Department
                && one.ManagerEmployeeId == two.ManagerEmployeeId
                && one.EmployeeId == two.EmployeeId;
        }
    }
}
