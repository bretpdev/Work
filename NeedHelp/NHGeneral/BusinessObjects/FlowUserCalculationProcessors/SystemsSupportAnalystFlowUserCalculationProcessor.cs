using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    class SystemsSupportAnalystFlowUserCalculationProcessor : BaseFlowCalculationProcessor
    {
        //constructor
        public SystemsSupportAnalystFlowUserCalculationProcessor(TicketData theTicket)
            : base(theTicket)
        {
        }

        public override void PerformCalculations(List<SqlUser> userList, ProcessLogRun logRun)
        {
            try
            {
                DataAccess dataAccess = new DataAccess(logRun);
                //Get the rotation list
                List<CourtRotation> rotationList = dataAccess.GetRotationList();
                //Figure out who is next
                CourtRotation rotationId = rotationList.Where(p => p.Rotation == 1).SingleOrDefault();
                if (TheTicket.AssignedTo.ID == rotationId.SqlUserId && rotationList.Count > 1) //If the ticket is already assigned to someone, get the next person in the list
                    rotationId = rotationList.Where(p => p.Rotation == 2).SingleOrDefault();
                else
                    rotationId = rotationList.Where(p => p.Rotation == 1).SingleOrDefault();
                //Assigned the ticket to the next person
                TheTicket.Court = userList.Where(p => p.ID == rotationId.SqlUserId).FirstOrDefault();
                TheTicket.AssignedTo = userList.Where(p => p.ID == rotationId.SqlUserId).FirstOrDefault();

                //Update the rotation list to move everyone up a spot and the person in number down to the bottom
                foreach (CourtRotation item in rotationList)
                {
                    if (item.Rotation == rotationList.Count)
                        dataAccess.SetRotation(item.SqlUserId, 1);
                    else
                    {
                        int rotation = item.Rotation + 1;
                        if (rotation <= rotationList.Count)
                            dataAccess.SetRotation(item.SqlUserId, rotation);
                        else
                            dataAccess.SetRotation(item.SqlUserId, GetUnassignedRotation(rotation, dataAccess));
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("There was an error assigning the court to the next staff member in the list");
                logRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
        }

        private int GetUnassignedRotation(int rotation, DataAccess dataAccess)
        {
            List<CourtRotation> rotations = dataAccess.GetRotationList();
            int count = 1;
            foreach (CourtRotation item in rotations.OrderBy(p => p.Rotation))
            {
                if (item.Rotation == count)
                    continue;
                else if (item.Rotation < count)
                    continue;
                else if (item.Rotation > count)
                    break;
                count++;
            }
            return count;
        }
    }
}