using System;

namespace INCIDENTRP
{
    public class Threat
    {
        public Reporter Reporter { get; set; }
        public Notifier Notifier { get; set; }
        public ActionTaken InformationTechnologyAction { get; set; }
        public ActionTaken LawEnforcementAction { get; set; }
        public ThreatInfo Info { get; set; }
        public BombInfo BombInfo { get; set; }
        public Caller Caller { get; set; }

        public Threat()
        {
        }

        public Threat(Reporter reporter)
        {
            Reporter = reporter;
            Notifier = new Notifier();
            InformationTechnologyAction = new ActionTaken(ActionTaken.CONTACTED_IT_INFORMATION_SECURITY_OFFICE);
            LawEnforcementAction = new ActionTaken(ActionTaken.CONTACTED_LAW_ENFORCEMENT);
            Info = new ThreatInfo();
            BombInfo = new BombInfo();
            Caller = new Caller();
        }

        public static Threat Load(DataAccess dataAccess, long ticketNumber)
        {
            Threat threat = new Threat();
            threat.Reporter = Reporter.Load(dataAccess, ticketNumber, Ticket.THREAT);
            threat.Notifier = Notifier.Load(dataAccess, ticketNumber, Ticket.THREAT);
            threat.InformationTechnologyAction = ActionTaken.Load(dataAccess, ticketNumber, Ticket.THREAT, ActionTaken.CONTACTED_IT_INFORMATION_SECURITY_OFFICE);
            threat.LawEnforcementAction = ActionTaken.Load(dataAccess, ticketNumber, Ticket.THREAT, ActionTaken.CONTACTED_LAW_ENFORCEMENT);
            threat.Info = ThreatInfo.Load(dataAccess, ticketNumber);
            threat.BombInfo = BombInfo.Load(dataAccess, ticketNumber);
            threat.Caller = Caller.Load(dataAccess, ticketNumber);
            return threat;
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            //We have no basic properties to save, so call on member objects save themselves.
            Reporter.Save(dataAccess, ticketNumber, Ticket.THREAT);
            Notifier.Save(dataAccess, ticketNumber, Ticket.THREAT);
            InformationTechnologyAction.Save(dataAccess, ticketNumber, Ticket.THREAT);
            LawEnforcementAction.Save(dataAccess, ticketNumber, Ticket.THREAT);
            Info.Save(dataAccess, ticketNumber);
            BombInfo.Save(dataAccess, ticketNumber);
            Caller.Save(dataAccess, ticketNumber);
        }
    }
}