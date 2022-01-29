using System.Windows.Forms;

namespace NHGeneral
{
    public class TicketDetailLinkButtonData
    {
        public LinkLabel StatusChanger { get; set; }
        public LinkLabel EditOrSave { get; set; }
        public LinkLabel Return { get; set; }
        public LinkLabel PreviousStatus { get; set; }
        public LinkLabel Hold { get; set; }
        public LinkLabel Withdraw { get; set; }
        public LinkLabel UpdateLink { get; set; }
        public Ticket TheTicket { get; set; }
    }
}