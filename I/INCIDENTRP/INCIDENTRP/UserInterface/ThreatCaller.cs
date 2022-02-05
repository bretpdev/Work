using System;
using System.Windows.Forms;

namespace INCIDENTRP
{
    partial class ThreatCaller : Form
    {
        private Threat Threat { get; set; }

        /// <summary>
        /// DO NOT USE!!!
        /// The parameterless constructor is required for the Visual Studio Forms Designer, but won't work with the script.
        /// </summary>
        public ThreatCaller()
        {
            InitializeComponent();
        }

        public ThreatCaller(Threat threat)
        {
            InitializeComponent();
            Threat = threat;
            voiceBindingSource.DataSource = Threat.Caller.Voice;
            languageBindingSource.DataSource = Threat.Caller.Language;
            dialectBindingSource.DataSource = Threat.Caller.Dialect;
            mannerBindingSource.DataSource = Threat.Caller.Manner;
            backgroundNoiseBindingSource.DataSource = Threat.Caller.BackgroundNoise;
            //I couldn't get the ThreatInfo object to bind to both forms, so we need to initialize txtRemarks text explicitly.
            txtRemarks.Text = Threat.Info.AdditionalRemarks;
        }

        public new void ShowDialog()
        {
            base.ShowDialog();
        }

        private void ChkVoiceOther_CheckedChanged(object sender, EventArgs e)
        {
            Threat.Caller.Voice.Other = chkVoiceOther.Checked;
            txtVoiceOther.Enabled = chkVoiceOther.Checked;
            if (!chkVoiceOther.Checked)
                txtVoiceOther.Clear();
        }

        private void ChkLanguageOther_CheckedChanged(object sender, EventArgs e)
        {
            Threat.Caller.Language.Other = chkLanguageOther.Checked;
            txtLanguageOther.Enabled = chkLanguageOther.Checked;
            if (!chkLanguageOther.Checked)
                txtLanguageOther.Clear();
        }

        private void ChkDialectRegionalAmericanOther_CheckedChanged(object sender, EventArgs e)
        {
            Threat.Caller.Dialect.RegionalAmerican = chkDialectRegionalAmericanOther.Checked;
            txtDialectRegionalAmericanOther.Enabled = chkDialectRegionalAmericanOther.Checked;
            if (!chkDialectRegionalAmericanOther.Checked)
                txtDialectRegionalAmericanOther.Clear();
        }

        private void ChkDialectForeignAccentOther_CheckedChanged(object sender, EventArgs e)
        {
            Threat.Caller.Dialect.ForeignAccent = chkDialectForeignAccentOther.Checked;
            txtDialectForeignAccentOther.Enabled = chkDialectForeignAccentOther.Checked;
            if (!chkDialectForeignAccentOther.Checked)
                txtDialectForeignAccentOther.Clear();
        }

        private void ChkMannerOther_CheckedChanged(object sender, EventArgs e)
        {
            Threat.Caller.Manner.Other = chkMannerOther.Checked;
            txtMannerOther.Enabled = chkMannerOther.Checked;
            if (!chkMannerOther.Checked)
                txtMannerOther.Clear();
        }

        private void ChkBackgroundNoiseOther_CheckedChanged(object sender, EventArgs e)
        {
            Threat.Caller.BackgroundNoise.Other = chkBackgroundNoiseOther.Checked;
            txtBackgroundNoiseOther.Enabled = chkBackgroundNoiseOther.Checked;
            if (!chkBackgroundNoiseOther.Checked)
                txtBackgroundNoiseOther.Clear();
        }
    }
}