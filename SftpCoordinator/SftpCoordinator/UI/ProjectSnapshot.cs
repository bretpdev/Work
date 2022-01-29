using System.Windows.Forms;

namespace SftpCoordinator
{
    public partial class ProjectSnapshot : Form
    {
        public ProjectSnapshot()
        {
            InitializeComponent();

            ResultsGrid.DataSource = ProjectFileDetail.GetAll();
        }
    }
}
