using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public partial class HomePageMdi : Form
    {
        public enum UserChosenPathFromHomePage
        {
            IncomingCall,
            BackToMainMenu,
            SaveAndContinue,
            UpdateDemographics,
            None
        }

        private List<IHomePage> _homePages;

        public UserChosenPathFromHomePage UserChosenPathFromHP { get; set; }
        public string SSN { get; set; }


        public HomePageMdi()
        {
            InitializeComponent();
        }

        public HomePageMdi(List<IHomePage> homePages)
        {
            InitializeComponent();

            UserChosenPathFromHP = UserChosenPathFromHomePage.None;
            _homePages = homePages;
            foreach (var homePage in _homePages)
            {
                ((Form)homePage).MdiParent = this;

                homePage.SaveAndContinue.Click += ButtonSaveAndContinue_Click;
                homePage.ReturnToMainMenuButtons.Click += ButtonReturnToMainMenu_Click;
                homePage.UpdateDemographicsButton.Click += ButtonUpdateDemos_Click;
            }
        }

        public new void ShowDialog()
        {
            UserChosenPathFromHP = UserChosenPathFromHomePage.None;
            foreach (var homePage in _homePages)
            {
                homePage.ShowCustom();
            }
            base.ShowDialog();
        }

        private void ButtonSaveAndContinue_Click(object sender, EventArgs e)
        {
            if(((IHomePage)((Button)sender).Parent).HasValidData)
            {
                ((Form)((Button)sender).Parent).Hide();
                if(_homePages.Where(p => p.IsVisible == true).Count() == 0)
                {
                    UserChosenPathFromHP = UserChosenPathFromHomePage.SaveAndContinue;
                }
                Hide();
            }
        }

        private void ButtonReturnToMainMenu_Click(object sender, EventArgs e)
        {
            UserChosenPathFromHP = UserChosenPathFromHomePage.BackToMainMenu;
            if (MessageBox.Show("Are you sure you want to go back to the main menu and abandon all changes?", "Back to Main Menu?", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            else
            {
                BrwInfo411Processor.Close411Form();
                foreach(var form in _homePages)
                {
                    form.CloseAllForms();
                    form.Hide();
                }
                Hide();
            }
        }

        private void ButtonUpdateDemos_Click(object sender, EventArgs e)
        {
            UserChosenPathFromHP = UserChosenPathFromHomePage.UpdateDemographics;
            Hide();
        }

    }
}
