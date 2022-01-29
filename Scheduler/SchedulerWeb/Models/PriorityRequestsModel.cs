using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulerWeb
{
    public class PriorityRequestsModel
    {
        public UserType UserType { get; set; }
        public List<PriorityRequest> Requests { get; set; }
        public List<PriorityRequest> CourtRequests { get; set; }
        public string UserFullName { get; set; }

        public void LoadRequests()
        {
            var requests = PriorityRequest.GetAll();
            Action<string> byCourt = new Action<string>(court =>
            {
                var statuses = requests.Select(o => o.Status).Distinct().ToList();
                Requests = requests.Where(o => o.Status == court).ToList();
                CourtRequests = requests.Where(o => o.CurrentCourt == UserFullName).ToList();
            });
            switch (UserType)
            {
                case UserType.Admin:
                    Requests = requests;
                    break;
                case UserType.Developer:
                    byCourt(programmerCourt);
                    break;
                case UserType.BusinessAnalyst:
                    byCourt(baCourt);
                    break;
                case UserType.ProjectManager:
                    Requests = requests.Where(o => !o.DevBegin.HasValue || !o.TestEnd.HasValue).ToList();
                    CourtRequests = requests.Except(Requests).ToList();
                    break;
            }
        }

        const string programmerCourt = "Programmer Assignment";
        const string baCourt = "Tester Assignment";


        public static PriorityRequestsModel ForCurrentUser(UserHelper uh)
        {
            var model = new PriorityRequestsModel();
            model.UserType = uh.CurrentUserType;
            model.UserFullName = uh.CurrentFullName;
            model.LoadRequests();
            return model;
        }
    }
}