using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace MauiDUDE
{
    public partial class MainMenu : Form
    {
        public static List<PreviousBorrower> PreviousBorrowers { get; private set; } = new List<PreviousBorrower>();
        private BaseUserSelectedFlow SelectedFlow { get; set; }
        private static Point FormLocation { get; set; }
        private static bool DemosOnly { get; set; }

        public static void AddPreviousBorrower(PreviousBorrower addition)
        {
            PreviousBorrowers.Insert(0, addition);
            if (PreviousBorrowers.Count == 11)
            {
                PreviousBorrowers.RemoveAt(10);
            }
        }

        public MainMenu()
        {
            InitializeComponent();
        }

        public MainMenu(string prePopulateSSNOrAccountNumber = "")
        {
            InitializeComponent();

            //pre-populate SSN or Account Number
            textBoxAccountNumberOrSSN.Text = prePopulateSSNOrAccountNumber;
            //use shared values to return form to it's last viewed state
            CoordinateDemosOnlyFunctionality();
        }

        /// <summary>
        /// Overriding the base show dialog to return the User Selected flow
        /// </summary>
        /// <returns></returns>
        public new BaseUserSelectedFlow ShowDialog()
        {
            base.ShowDialog();
            return SelectedFlow;
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(SelectedFlow == null)
            {
                SelectedFlow = new CloseDUDEFlow();
            }
            FormLocation = Location; // get form location
        }

        private void noUHEAAConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedFlow = new NoUHEAAConnectionFlow();
            DialogResult = DialogResult.OK;
        }

        private void askDudeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FaqLinker.ShowFaq(this);
        }

        private void unexpectedResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnexpectedResults.ShowUnexpectedResults();
        }

        private void brightIdeaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrightIdea.ShowBrightIdea();
        }

        private void demosOnlyOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DemosOnly = !DemosOnly;
            CoordinateDemosOnlyFunctionality();
        }

        /// <summary>
        /// Coordinates demos only menu option look
        /// </summary>
        private void CoordinateDemosOnlyFunctionality()
        {
            if(DemosOnly)
            {
                //do only demos processing
                demosOnlyOffToolStripMenuItem.ForeColor = Color.Green;
                demosOnlyOffToolStripMenuItem.Text = "Demos Only: ON";
            }
            else
            {
                //do full home page processing
                demosOnlyOffToolStripMenuItem.ForeColor = Color.Red;
                demosOnlyOffToolStripMenuItem.Text = "Demos Only: OFF";
            }
        }

        //Coordinates
        private void CoordinateScreenLocation()
        {
            if(FormLocation != null)
            {
                Left = FormLocation.X;
                Top = FormLocation.Y;
            }
        }

        private void buttonPhysicalThreat_Click(object sender, EventArgs e)
        {
            string arguments = "--ticketType Threat --region UHEAA";
            if(DataAccessHelper.TestMode)
            {
                arguments += " --dev";
            }
            Proc.Start("IncidentReporting", arguments);
        }

        private void buttonSecurityIncident_Click(object sender, EventArgs e)
        {
            string arguments = "--ticketType Incident --region UHEAA";
            if (DataAccessHelper.TestMode)
            {
                arguments += " --dev";
            }
            Proc.Start("IncidentReporting", arguments);
        }

        private void labelShowPreviousBorrowers_Click(object sender, EventArgs e)
        {
            if(labelShowPreviousBorrowers.Text == "+" && PreviousBorrowers.Count > 0)
            {
                labelShowPreviousBorrowers.Text = "-";

                //header
                PreviousBorrowerDisplay controlToBeAdded = new PreviousBorrowerDisplay();
                
                controlToBeAdded.BackColor = Color.White;
                flowLayoutPanelPreviousBorrowers.Controls.Add(controlToBeAdded);
                //data
                foreach(var borrower in PreviousBorrowers)
                {
                    controlToBeAdded = new PreviousBorrowerDisplay(borrower, flowLayoutPanelPreviousBorrowers.Controls.Count);
                    if(flowLayoutPanelPreviousBorrowers.Controls.Count % 2 == 0)
                    {
                        //if not even turn it white
                        controlToBeAdded.BackColor = Color.White;
                    }
                    flowLayoutPanelPreviousBorrowers.Controls.Add(controlToBeAdded);
                }

            }
            else
            {
                labelShowPreviousBorrowers.Text = "+";
                flowLayoutPanelPreviousBorrowers.Controls.Clear();
            }
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            CoordinateScreenLocation();
        }
    }
}
