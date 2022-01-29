using System.Windows.Forms;

namespace SftpCoordinator
{
    public partial class RunHistoryForm : Form
    {
        const string dateFormat = "MM/dd/yyyy hh:mm tt";
        public RunHistoryForm(RunHistoryDetail history)
        {
            InitializeComponent();

            RunIdText.Text = history.RunHistoryId.ToString();
            RunByText.Text = history.RunBy;
            StartedOnText.Text = history.StartedOn.ToString(dateFormat);
            EndedOnText.Text = history.EndedOn.HasValue ? history.EndedOn.Value.ToString(dateFormat) : "";

            ResultsGrid.DataSource = ActivityLogDetail.GetActivityLogsByRunHistory(history.RunHistoryId);
        }
    }
}
