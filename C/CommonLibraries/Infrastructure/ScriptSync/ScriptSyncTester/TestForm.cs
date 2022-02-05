using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ScriptSyncTester
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ModeSelector.DataSource = new ArrayList {
                new {Value = DataAccessHelper.Mode.Test, Text = "Test"},
                new {Value = DataAccessHelper.Mode.Live, Text = "Live"},
                new {Value = DataAccessHelper.Mode.Dev, Text = "Dev"},
                new {Value = DataAccessHelper.Mode.QA, Text = "QA"}
            };
            ModeSelector.DisplayMember = "Text";
            ModeSelector.ValueMember = "Value";
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            string scriptId = ScriptIdText.Text;
            var mode = (DataAccessHelper.Mode)ModeSelector.SelectedValue;
            ScriptSync ss = new ScriptSync();
            Reflection.Session session = ReflectionInterface.OpenExistingSession(ReflectionInterface.Flag.None);
            new ReflectionInterface(session).Login(InsecureCredentials.Username, InsecureCredentials.Password);
            IInstantiationWrapper<object> wrapper = null;
            //try cornerstone first.
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            try
            {
                wrapper = ss.InstantiateScript(session, (int)mode, scriptId);
            }
            catch (Exception ex)
            {
                //try uheaa now
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                wrapper = ss.InstantiateScript(session, (int)mode, scriptId);
            }
            if (wrapper != null)
                MessageBox.Show("Script instantiated successfully");
            else
                MessageBox.Show("Unable to successfully instantiate script.");
            session.Exit();
        }

        private void TestRunButton_Click(object sender, EventArgs e)
        {
            string scriptId = ScriptIdText.Text;
            var mode = (DataAccessHelper.Mode)ModeSelector.SelectedValue;
            ScriptSync ss = new ScriptSync();
            Reflection.Session session = ReflectionInterface.OpenExistingSession(ReflectionInterface.Flag.None);
            var ri = new ReflectionInterface(session);
            ri.Login(InsecureCredentials.Username, InsecureCredentials.Password);
            //TODO: make sure session is stuped correctly before running.
            ri.Stup();
            var script = ss.InstantiateScript(session, (int)mode, scriptId);
            if (script != null)
                ss.SyncAndStartWithErrorPopup(session, (int)mode, scriptId);
            else
                MessageBox.Show("Unable to successfully instantiate script.");
            session.Exit();
        }

        private void FullTestMenu_Click(object sender, EventArgs e)
        {
            FullTest ft = new FullTest();
            ft.ShowDialog();
        }
    }
}
