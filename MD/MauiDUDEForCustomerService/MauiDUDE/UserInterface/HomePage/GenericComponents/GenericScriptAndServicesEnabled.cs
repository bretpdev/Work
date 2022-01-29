using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace MauiDUDE
{
    public partial class GenericScriptAndServicesEnabled : Form
    {
        protected Borrower Borrower { get; set; }
        protected Dictionary<string, ScriptAndServiceMenuItem> Scripts { get; set; } = new Dictionary<string, ScriptAndServiceMenuItem>();
        private List<ActivityHistory> ActivityHistoryForms { get; set; } = new List<ActivityHistory>();

        public GenericScriptAndServicesEnabled()
        {
            InitializeComponent();
        }

        public GenericScriptAndServicesEnabled(string homePage, Borrower borrower, string scriptsDisableKey, DataAccessHelper.Region region)
        {
            InitializeComponent();

            Borrower = borrower;
            CreateMenuItems(null, homePage, scriptsDisableKey);
            ToolStripMenuItem letterMenu = new ToolStripMenuItem("Letters");
            menuStripScriptsAndServices.Items.Add(letterMenu);
            //string accountNumber = Borrower.AccountNumber;
            letterMenu.Click += (obj, ea) =>
            {
                string mdLettersPath = Path.Combine(EnterpriseFileSystem.GetPath("CodeBase", DataAccessHelper.Region.Uheaa), "MDLetters");
                string localPath = Path.Combine(EnterpriseFileSystem.TempFolder, "MDLetters");
                //check if there are changes and the files need to be re-mirrored
                bool changes = false;
                if(!Directory.Exists(localPath))
                {
                    changes = true;
                }
                else
                {
                    foreach(string serverFile in Directory.GetFiles(mdLettersPath))
                    {
                        string localFile = Path.Combine(localPath, Path.GetFileName(serverFile));
                        if(!File.Exists(localFile))
                        {
                            changes = true;
                            break;
                        }
                        else if(new FileInfo(localFile).LastWriteTime != new FileInfo(serverFile).LastWriteTime)
                        {
                            changes = true;
                            break;
                        }

                    }
                }
                if(changes)
                {
                    if(!Directory.Exists(localPath))
                    {
                        FS.CreateDirectory(localPath);
                    }
                    foreach(string serverFile in Directory.GetFiles(mdLettersPath))
                    {
                        string localFile = Path.Combine(localPath, Path.GetFileName(serverFile));
                        FS.Copy(serverFile, localFile, true);
                    }
                }
                Proc.Start("MDLetters", (DataAccessHelper.TestMode ? "dev" : "live") + " " + region.ToString() + " " + Borrower.AccountNumber);
            };
            //TODO move to enterprise file system
            string location = DataAccessHelper.TestMode ? @"X:\PADU\UHEAACodeBase\CMPLNTRACK\" : @"X:\Sessions\UHEAA Codebase\CMPLNTRACK\";
            //if(Directory.Exists(location) && region == DataAccessHelper.Region.CornerStone)
            //{
            //    ToolStripMenuItem complaintMenu = new ToolStripMenuItem("Complaints");
            //    menuStripScriptsAndServices.Items.Add(complaintMenu);

            //    complaintMenu.Click += (obj, ea) =>
            //    {
            //        try
            //        {
            //            string localLocation = Path.Combine(EnterpriseFileSystem.TempFolder,@"MD\CMPLNTRACK");
            //            if (!Directory.Exists(localLocation))
            //            {
            //                FS.CreateDirectory(localLocation);
            //            }
            //            foreach (string networkFile in Directory.GetFiles(location))
            //            {
            //                FS.Copy(networkFile, Path.Combine(localLocation, Path.GetFileName(networkFile)), true);
            //            }
            //            string modeArg = DataAccessHelper.TestMode ? "dev" : "live";
            //            string args = $"mode:{modeArg} region:{region.ToString()} accountnumber:{Borrower.AccountNumber}";
            //            Proc.Start("MDCMPLNTRACK", args);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("Unable to open Complaint Tracker, please close existing copies.");
            //        }
            //    };
            //}
        }

        //this function is recursive and sets up all menu items for the screen
        private void CreateMenuItems(ToolStripMenuItem parentMenu, string homePage, string scriptsDisableKey)
        {
            string pMenu = "";

            int i = 0;
            if(parentMenu != null)
            {
                pMenu = parentMenu.Text;
            }
            DataTable results = DataAccess.DA.GetScriptAndServicesMenuOptions(homePage, pMenu);
            while (i < results.Rows.Count)
            {
                ScriptAndServiceMenuItem ssmi;
                if (parentMenu == null)
                {
                    //if the menu item is being directly added to the menu bar
                    ssmi = new ScriptAndServiceMenuItem(results.Rows[i], results.Rows[i]["DisplayName"].ToString());
                    if(results.Rows[i]["SubToBeCalled"].ToString() != "")
                    {
                        //if the DB has a method to be called then add an event handler
                        ssmi.Click += EventHandlerForMenuItems; //add event handler for menu items
                    }
                    menuStripScriptsAndServices.Items.Add(ssmi); //add to menu
                }
                else
                {
                    //if menu item is being added to a list under a menu item
                    ssmi = new ScriptAndServiceMenuItem(results.Rows[i], results.Rows[i]["DisplayName"].ToString());
                    if(results.Rows[i]["SubToBeCalled"].ToString() != "")
                    {
                        //if the DB has a sub to be called then add event handler
                        ssmi.Click += EventHandlerForMenuItems; //add event handler for menu item
                    }
                    Scripts.Add(results.Rows[i]["DisplayName"].ToString(), ssmi);
                    parentMenu.DropDownItems.Add(ssmi); //add to menu
                    //check if script should be disabled based off option the user selected on bins screen
                    if(scriptsDisableKey != "")
                    {
                        //disable the menu option if account maintainence was selected on the bind page
                        if(results.Rows[i]["DisableKey"].ToString().Contains("AM"))
                        {
                            ssmi.Enabled = false;
                        }
                    }
                }
                //check if menu item should also be a parent menu item
                int subResultsCount = DataAccess.DA.GetScriptAndServicesMenuOptions(homePage, pMenu).Rows.Count;
                if(subResultsCount > 0)
                {
                    //create child menu items if needed
                    CreateMenuItems(ssmi, homePage, scriptsDisableKey);
                }
                i++;
            }
        }

        protected void RunActivityHistory(int days, string system, DataAccessHelper.Region region)
        {
            ActivityComments.AESSystem enumAesTranslation;
            string screenNameForSystem;
            if(system == Enum.GetName(typeof(ActivityComments.AESSystem), ActivityComments.AESSystem.Compass))
            {
                enumAesTranslation = ActivityComments.AESSystem.Compass;
                screenNameForSystem = "TD2A";
            }
            else
            {
                enumAesTranslation = ActivityComments.AESSystem.OneLINK;
                screenNameForSystem = "LP50";
            }
            string activityFormTitleText;
            if(days == 0)
            {
                activityFormTitleText = $"Maui DUDE Complete Borrower Activity History - {screenNameForSystem}";
            }
            else
            {
                activityFormTitleText = $"Maui DUDE Borrower {days.ToString()} Day Activity History - {screenNameForSystem}";
            }

            //check to be sure that the form doesn't already exist in the list
            List<ActivityHistory> matches = ActivityHistoryForms.Where(p => p.Text == activityFormTitleText).ToList();

            ActivityHistory tempACForm;
            if (matches.Count == 0)
            {
                Processing.MakeVisible(); 
                tempACForm = new ActivityHistory(Borrower);
                if(tempACForm.Show(days, activityFormTitleText, enumAesTranslation, region))
                {
                    ActivityHistoryForms.Add(tempACForm);
                }
                Processing.MakeInvisible();
            }
            else
            {
                try
                {
                    if(matches.First().WindowState == FormWindowState.Minimized)
                    {
                        matches.First().WindowState = FormWindowState.Normal;
                    }
                    matches.First().Focus();
                }
                catch(Exception ex)
                {
                    Processing.MakeVisible();
                    ActivityHistoryForms.Remove(matches.First());
                    tempACForm = new ActivityHistory(Borrower);
                    ActivityHistoryForms.Add(tempACForm);
                    tempACForm.Show(days, activityFormTitleText, enumAesTranslation, region);
                    Processing.MakeInvisible();
                }

            }
        }

        /// <summary>
        /// closes all activity history windows
        /// </summary>
        protected void CloseAllActivityHistoryForms()
        {
            foreach(ActivityHistory form in ActivityHistoryForms)
            {
                if(form != null)
                {
                    form.Close();
                }
            }
        }

        public virtual void EventHandlerForMenuItems(object sender, EventArgs e)
        {
            BaseScriptRequestProcessor scriptProcessor = null;
            if(((ScriptAndServiceMenuItem)sender).gsData["InternalOrExternal"].ToString() == "External")
            {
                scriptProcessor = new SessionScriptProcessor((ScriptAndServiceMenuItem)sender, Borrower);
            }
            else if (((ScriptAndServiceMenuItem)sender).gsData["InternalOrExternal"].ToString() == ".NET DLL")
            {
                scriptProcessor = new DotNetScriptProcessor((ScriptAndServiceMenuItem)sender, Borrower);
            }

            if(scriptProcessor  != null)
            {
                scriptProcessor.RunScript(Text, 1);
            }
            else
            {
                WhoaDUDE.ShowWhoaDUDE($"Failed to run selected script: {Text}. Please contact Systems Support to resolve the issue", "Failed to launch script.");
            }
            
        }
    }
}
