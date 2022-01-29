using SubSystemShared;
using System.Collections.Generic;

namespace NHGeneral
{
    partial class MiddleClassBetweenBaseAndControls : BaseNeedHelpTicketDetail
	{
        public MiddleClassBetweenBaseAndControls()
        {
            InitializeComponent();
        }

        public override TicketData GetModifiedTicketData(Ticket activeTicket, List<SqlUser> users, NotifyType.Type type)
        {
            return null;
        }

        public override void BindNewTicket(Ticket activeTicket)
        {
        }

        public override void SetIssueToReadOnly(bool isReadOnly)
        {
        }

        public override void SetEmailRecipientDataSource(List<SqlUser> emailUser)
        {
        }

        public override void AddUploadedFile(AttachedFile file)
        {
        }

        public override void SetPriority()
        {
        }
    }
}
