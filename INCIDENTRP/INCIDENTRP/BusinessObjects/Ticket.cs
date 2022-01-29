using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace INCIDENTRP
{
    public class Ticket
    {
        //Valid values for Type:
        public const string INCIDENT = "Incident";
        public const string THREAT = "Threat";

        public long Number { get; set; }
        public string Type { get; set; }
        public SqlUser LockHolder { get; set; }
        public DateTime IncidentDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
        public SqlUser Requester { get; set; }
        public string FunctionalArea { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
        public SqlUser Court { get; set; }
        public SqlUser AssignedTo { get; set; }
        public Incident Incident { get; set; }
        public Threat Threat { get; set; }
        public List<HistoryRecord> History { get; private set; }

        /// <summary>
        /// The date that the ticket came into the current court. Calculated from the ticket's history.
        /// </summary>
        public DateTime CourtDate
        {
            get
            {
                DateTime lastCourtDate = DateTime.Now;
                if (History.Count() == 1)
                    lastCourtDate = History.SingleOrDefault().UpdateDateTime;
                else if (History.Count() > 1)
                {
                    HistoryRecord[] sortedHistory = History.OrderByDescending(p => p.UpdateDateTime).ToArray();
                    lastCourtDate = sortedHistory[0].UpdateDateTime;
                    for (int i = 1; i < sortedHistory.Length; i++)
                    {
                        if (sortedHistory[i].User == sortedHistory[0].User)
                            lastCourtDate = sortedHistory[i].UpdateDateTime;
                        else
                            break;
                    }
                }
                return lastCourtDate;
            }
        }

        /// <summary>
        /// The date that the ticket came into the current status. Calculated from the ticket's history.
        /// </summary>
        public DateTime StatusDate
        {
            get
            {
                DateTime lastStatusDate = DateTime.Now;
                if (History.Count() == 1)
                    lastStatusDate = History.SingleOrDefault().UpdateDateTime;
                else if (History.Count() > 1)
                if (History.Count() == 1)
                    lastStatusDate = History.SingleOrDefault().UpdateDateTime;
                else if (History.Count() > 1)
                {
                    HistoryRecord[] sortedHistory = History.OrderByDescending(p => p.UpdateDateTime).ToArray();
                    lastStatusDate = sortedHistory[0].UpdateDateTime;
                    for (int i = 1; i < sortedHistory.Length; i++)
                    {
                        if (sortedHistory[i].Status == sortedHistory[0].Status)
                            lastStatusDate = sortedHistory[i].UpdateDateTime;
                        else
                            break;
                    }
                }
                return lastStatusDate;
            }
        }

        //The Number property may get altered when saving the ticket. Keep track of the old value in case we end up backing out of the save.
        private long OldNumber { get; set; }

        public Ticket()
        {
            //This constructor should only be used by the Load() method, which must set all of the object's members.
        }

        //Constructor for Need Help to call.
        public Ticket(string ticketType, Reporter reporter, string status)
        {
            Number = 0;
            OldNumber = Number;
            Type = ticketType;
            IncidentDateTime = DateTime.Now;
            CreateDateTime = DateTime.Now;
            Requester = reporter.User;
            FunctionalArea = "Security";
            //Priority will be calculated in the Save() method, based on ticket type and urgency.
            Status = status;
            Court = reporter.User;
            if (ticketType == INCIDENT)
                Incident = new Incident(reporter);
            else
                Threat = new Threat(reporter);
            History = new List<HistoryRecord>();
        }

        //Constructor for DUDE to call.
        public Ticket(string ticketType, Reporter reporter, BorrowerDataInvolved borrowerData, string status)
        {
            Number = 0;
            OldNumber = Number;
            Type = ticketType;
            IncidentDateTime = DateTime.Now;
            CreateDateTime = DateTime.Now;
            Requester = reporter.User;
            FunctionalArea = "Security";
            //Priority will be calculated in the Save() method, based on ticket type and urgency.
            Status = status;
            Court = reporter.User;
            if (ticketType == INCIDENT)
                Incident = new Incident(reporter, borrowerData);
            else
                Threat = new Threat(reporter);
            History = new List<HistoryRecord>();
        }

        public void AddHistoryRecord(SqlUser user, string status, string statusChangeDescription, string updateText)
        {
            HistoryRecord record = new HistoryRecord();
            record.IsDirty = true;
            record.Status = status;
            record.StatusChangeDescription = statusChangeDescription;
            record.UpdateDateTime = DateTime.Now;
            record.UpdateText = updateText;
            record.User = user;
            History.Add(record);
        }

        public static Ticket Load(DataAccess dataAccess, long ticketNumber, string ticketType, SqlUser user)
        {
            Ticket ticket = dataAccess.LoadTicket(ticketNumber, ticketType);
            ticket.LockHolder = dataAccess.GetLock(ticket, user);
            ticket.History = dataAccess.GetHistory(ticketNumber, ticketType);
            if (ticketType == INCIDENT)
                ticket.Incident = Incident.Load(dataAccess, ticketNumber);
            else
                ticket.Threat = Threat.Load(dataAccess, ticketNumber);
            return ticket;
        }

        public void Save(DataAccess dataAccess, Ticket ticket, Form displayForm)
        {
            string updateText = "";
            if (!CheckForSsn(ticket, displayForm, ref updateText))
                return;
            //Set the ticket's priority based on type and urgency. This has to be done before committing the ticket to the IncidentReportingUheaa.
            Priority = 9;
            if (Incident != null)
                Priority = dataAccess.GetIncidentPriority(Incident.Priority);
            try
            {
                //Save our own basic properties.
                Number = dataAccess.SaveTicket(this);
                foreach (HistoryRecord record in History.Where(p => p.IsDirty))
                    dataAccess.AddHistoryRecord(Number, Type, record);
                //Call on the appropriate member object to save itself (only one will be instantiated).
                if (Incident != null)
                    Incident.Save(dataAccess, Number);
                else if (Threat != null)
                    Threat.Save(dataAccess, Number);
                OldNumber = Number;
                foreach (HistoryRecord record in History)
                    record.IsDirty = false;
            }
            catch (Exception)
            {
                //Back everything out and re-throw the exception so the UI can alert the user.
                Number = OldNumber;
                throw;
            }
        }

        public bool CheckForSsn(Ticket ticket, Form passedForm, ref string updateText)
        {
            SsnHelper helper = new SsnHelper(passedForm);
            if (passedForm.Name.ToUpper().StartsWith("THREAT") && ticket.Threat != null)
                helper.ThreatCheck(ticket);
            if ((passedForm.Name.ToUpper().StartsWith("INCIDENT") || passedForm.Name.ToUpper().StartsWith("SECURITY")) && ticket.Incident != null)
                helper.IncidentCheck(ticket);
            if (updateText.IsPopulated())
                updateText = helper.MaskSsnIfExists(updateText);
            return helper.FormNotCanceled;
        }
    }
}