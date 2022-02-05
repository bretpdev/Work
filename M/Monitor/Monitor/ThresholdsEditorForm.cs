using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace Monitor
{
    public partial class ThresholdsEditorForm : Form
    {
        public MonitorSettings WorkingSettings { get; private set; }
        public ThresholdsEditorForm(MonitorSettings settings)
        {
            InitializeComponent();
            WorkingSettings = settings ?? new MonitorSettings();

            MaxIncreaseBox.DataBindings.Add("Value", WorkingSettings, "MaxIncrease");
            MaxIncreaseDbBox.Text = WorkingSettings.MaxIncrease.ToString("C2").Replace("$", "");

            MaxForceBox.DataBindings.Add("Value", WorkingSettings, "MaxForce");
            MaxForceDbBox.Text = WorkingSettings.MaxForce.ToString();

            MaxPreNoteBox.DataBindings.Add("Value", WorkingSettings, "MaxPreNote");
            MaxPreNoteDbBox.Text = WorkingSettings.MaxPreNote.ToString();

            HeaderLabel.Text = "CornerStone " + HeaderLabel.Text;
        }
    }
}
