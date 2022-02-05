using System.Windows.Forms;


namespace ACHSETUPFD
{
    public partial class MainMenu : Form
    {
        private UserProvidedMainMenuData MainMenuResponse { get; set; }

        public MainMenu(UserProvidedMainMenuData mainMenuResponse)
        {
            InitializeComponent();
            MainMenuResponse = mainMenuResponse;
            userProvidedMainMenuDataBindingSource.DataSource = mainMenuResponse;
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            //do radio button to enum translation
            if (radAdd.Checked)
            {
                MainMenuResponse.UserSelectedACHOption = UserProvidedMainMenuData.UserSelectedACHAction.Add;
            }
            else if (radChange.Checked)
            {
                MainMenuResponse.UserSelectedACHOption = UserProvidedMainMenuData.UserSelectedACHAction.Change;
            }
            else if (radRemove.Checked)
            {
                MainMenuResponse.UserSelectedACHOption = UserProvidedMainMenuData.UserSelectedACHAction.Remove;
            }
            else if (radSuspend.Checked)
            {
                MainMenuResponse.UserSelectedACHOption = UserProvidedMainMenuData.UserSelectedACHAction.Suspend;
            }
            else if (radMissingInfo.Checked)
            {
                MainMenuResponse.UserSelectedACHOption = UserProvidedMainMenuData.UserSelectedACHAction.MissingInformation;
            }
            //data validation
            if (MainMenuResponse.SSN.Length < 9 || MainMenuResponse.FirstName.Length == 0 ||
                MainMenuResponse.UserSelectedACHOption == UserProvidedMainMenuData.UserSelectedACHAction.None)
            {
                MessageBox.Show("There is required data missing that needs to be supplied.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}
