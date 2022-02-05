using System.Windows.Forms;

namespace ACHSETUP
{
    public partial class MainMenu : Form
    {
        private UserProvidedMainMenuData MainMenuResponse;

        /// <summary>
        /// DO NOT USE!!!
        /// The parameterless constructor is required by the Windows Forms Designer,
        /// but it won't work with the script.
        /// </summary>
        public MainMenu()
        {
            InitializeComponent();
        }

        public MainMenu(UserProvidedMainMenuData mainMenuResponse)
        {
            InitializeComponent();
            MainMenuResponse = mainMenuResponse;
            userProvidedMainMenuDataBindingSource.DataSource = mainMenuResponse;
        }

        private void Ok_Click(object sender, System.EventArgs e)
        {
            //do radio button to enum translation
            if (radAdd.Checked)
                MainMenuResponse.UserSelectedACHOption = UserProvidedMainMenuData.UserSelectedACHAction.Add;
            else if (radChange.Checked)
                MainMenuResponse.UserSelectedACHOption = UserProvidedMainMenuData.UserSelectedACHAction.Change;
            else if (radRemove.Checked)
                MainMenuResponse.UserSelectedACHOption = UserProvidedMainMenuData.UserSelectedACHAction.Remove;
            else if (radSuspend.Checked)
                MainMenuResponse.UserSelectedACHOption = UserProvidedMainMenuData.UserSelectedACHAction.Suspend;
            else if (radMissing.Checked)
                MainMenuResponse.UserSelectedACHOption = UserProvidedMainMenuData.UserSelectedACHAction.MissingInformation;
            //data validation
            if (MainMenuResponse.SSN.Length < 9 || MainMenuResponse.FirstName.Length == 0 ||
                MainMenuResponse.UserSelectedACHOption == UserProvidedMainMenuData.UserSelectedACHAction.None)
            {
                MessageBox.Show("All data is needed in order to proceed.  Please provide all data and tray again.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}