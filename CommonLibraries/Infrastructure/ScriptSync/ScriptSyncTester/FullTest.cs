using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ScriptSyncTester
{
    public partial class FullTest : Form
    {
        public FullTest()
        {
            InitializeComponent();
        }

        private void FullTest_Load(object sender, EventArgs e)
        {
            TestGrid.AutoGenerateColumns = false;
            Test();
        }

        private Dictionary<string, string> Exclusions = new Dictionary<string, string>()
        {
            {"AWGRelease", "Only run from Maui DUDE"},
            {"OPSCBPFED", "Only run from Maui DUDE"},
            {"QCDBUser", "This is a standalone app"},
            {"BANKORFED", "Instantiates, but crashes because it can't find GnuPG software."},
            {"TestPrintBatchScriptStarter", "Not a real script, no references found in repository."}
        };

        List<ScriptTest> tests = new List<ScriptTest>();
        protected void Test()
        {
            Thread t = new Thread(() =>
            {
                var liveAreas = new IArea<Script>[] { Areas.Live.CommonFfel, Areas.Live.QFfel, Areas.Live.TempFfel, Areas.Live.CommonFed, Areas.Live.QFed, Areas.Live.TempFed };
                var testAreas = new IArea<Script>[] { Areas.Test.CommonFfel, Areas.Test.QFfel, Areas.Test.TempFfel, Areas.Test.CommonFed, Areas.Test.QFed, Areas.Test.TempFed };
                foreach (var group in liveAreas.Select(o => new { Area = o, IsLive = true }).Concat(testAreas.Select(o => new { Area = o, IsLive = false })))
                {
                    group.Area.SyncDependencies();
                    foreach (Script s in group.Area.FindAllScripts())
                    {
                        ScriptTest st = new ScriptTest(s);
                        st.IsLive = group.IsLive;
                        st.IsFed = group.Area.IsFed;
                        st.ScriptId = s.ScriptId;
                        if (s is QScript)
                            st.Type = ScriptType.Q;
                        if (s is TempScript)
                            st.Type = ScriptType.Temp;
                        if (s is CommonScript)
                            st.Type = ScriptType.Common;
                        if (Exclusions.ContainsKey(st.ScriptId))
                        {
                            st.Notes = Exclusions[st.ScriptId];
                            st.TestResults = TestResult.Skipped;
                        }
                        tests.Add(st);
                        UpdateGrid(tests);
                    }
                }
                ScriptSync ss = new ScriptSync();
                using (ManagedReflectionInterface ri = new ManagedReflectionInterface())
                {
                    foreach (ScriptTest st in tests.Where(o => o.TestResults == TestResult.Pending))
                    {
                        DataAccessHelper.CurrentMode = st.IsLive ? DataAccessHelper.Mode.Live : DataAccessHelper.Mode.Test;
                        DataAccessHelper.CurrentRegion = st.IsFed ? DataAccessHelper.Region.CornerStone : DataAccessHelper.Region.Uheaa;
                        if (!ri.CheckForText(16, 2, "LOGON"))
                            ri.LogOut();
                        while (!ri.Login(InsecureCredentials.Username, InsecureCredentials.Password))
                            if (MessageBox.Show("Unable to login for testing, please correct any login errors.", "Login Error", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                                return;
                        ri.Stup();
                        try
                        {
                            if (ss.InstantiateScript(ri.ReflectionSession, (int)DataAccessHelper.CurrentMode, st.Script) != null)
                                st.TestResults = TestResult.Success;
                            else
                                st.TestResults = TestResult.Failure;
                        }
                        catch (InvalidConstructorException)
                        {
                            st.TestResults = TestResult.BadConstructor;
                        }
                        catch (IncorrectRegionException)
                        {
                            st.TestResults = TestResult.BadRegion;
                            if (!st.IsFed && st.ScriptId.EndsWith("FED"))
                                st.Notes = "Script ends in FED but is in an FFEL location.";
                        }
                        UpdateGrid(tests);
                    }
                }
                MessageBox.Show("Test Complete");
            });
            t.Start();
        }

        protected void UpdateGrid(List<ScriptTest> tests)
        {
            this.Invoke(new Action(() =>
            {
                int index = TestGrid.FirstDisplayedScrollingRowIndex;
                TestGrid.DataSource = null;
                TestGrid.DataSource = tests;
                TestGrid.FirstDisplayedScrollingRowIndex = index;
            }));
        }
    }
}
