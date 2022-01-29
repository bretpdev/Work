using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;


namespace TEXTCOORD
{
    public partial class Search : Form
    {
        private List<ExcelData> Data { get; set; }
        private List<Campaigns> Campaigns { get; set; }
        private bool IsValid { get; set; }
        private DataAccess DA { get; set; }
        const int lowerAgeLimit = 15;
        const int upperAgeLimit = 110;

        public Search(DataAccess da)
        {
            InitializeComponent();
            DA = da;
            Data = new List<ExcelData>();
            Campaigns = DA.GetCampaigns();
            Campaign.DataSource = Campaigns;
            Campaign.DisplayMember = "Campaign";
            LowerAge.Text = lowerAgeLimit.ToString();
            UpperAge.Text = upperAgeLimit.ToString();
            ToolTip ageHover = new ToolTip();
            ageHover.AutoPopDelay = 5000;
            ageHover.InitialDelay = 1000;
            ageHover.ReshowDelay = 500;
            ageHover.ShowAlways = true;
            ageHover.SetToolTip(this.LowerAge, "Age fields are optional.");
            ageHover.SetToolTip(this.UpperAge, "Age fields are optional.");
            this.Text = $"{this.Text} :: {Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private Campaigns SelectedCampaign
        {
            get
            {
                return Campaign.SelectedItem as Campaigns;
            }
        }

        private async void SearchSub_Click(object sender, EventArgs e)
        {
            if (!Segment.Enabled)
                Segment.BackColor = SystemColors.Window;

            if (!PerformanceCat.Enabled)
                PerformanceCat.BackColor = SystemColors.Window;

            if (IsValid)
            {
                List<Ranks> segment = null;
                List<Ranks> performanceCat = null;
                Campaigns cam = SelectedCampaign;

                segment = new List<Ranks>();
                foreach (var item in Segment.CheckedItems)
                    segment.Add(new Ranks() { Ranking = item.ToString().Substring(0, 2).Replace(" ", "").ToInt() });

                performanceCat = new List<Ranks>();
                foreach (var item in PerformanceCat.CheckedItems)
                    performanceCat.Add(new Ranks() { Ranking = item.ToString().Substring(0, 2).Replace(" ", "").ToInt() });
                ExcelPreview.DataSource = null;
                List<ExcelData> data = new List<ExcelData>();

                await Task.Factory.StartNew(() =>
                data = DisplayData(cam, NumberToSend.Text.ToInt(), LowerDelinquency.Text.ToInt(), UpperDelinquency.Text.ToInt(), LowerAge.Text.ToInt(), UpperAge.Text.ToInt(), segment, performanceCat),
                    TaskCreationOptions.LongRunning);
                SortableBindingList<ExcelData> disData = new SortableBindingList<ExcelData>(data);
                ExcelPreview.DataSource = disData;
                Export.Enabled = true;
            }
        }

        private List<ExcelData> DisplayData(Campaigns cam, int numToSend, int lower, int upper, int lowerAge, int upperAge, List<Ranks> segment, List<Ranks> category)
        {
            SetLoading(true);
            try
            {
                Data = DA.Search(cam.Sproc, numToSend, cam.Campaign, lower, upper, lowerAge, upperAge, segment, category);
                if (Data.Count < numToSend)
                    Dialog.Warning.Ok($"You requested {numToSend} result(s) but the query only returned {Data.Count} result");

                return Data;
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void SetLoading(bool displayLoader)
        {
            if (displayLoader)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    picLoader.Visible = true;
                    this.Cursor = Cursors.WaitCursor;
                });
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    picLoader.Visible = false;
                    this.Cursor = Cursors.Default;
                });
            }
        }

        private void SearchSub_OnValidate(object sender, Uheaa.Common.WinForms.ValidationEventArgs e)
        {
            IsValid = e.FormIsValid;
        }

        private void EnableDisable(Control control, List<CampaignDisabledUiField> disabledFields, UiFields fieldToDisable)
        {
            bool enabled = !disabledFields.Any(o => o.UiFieldId == fieldToDisable);
            if (enabled)
            {
                control.Enabled = true;
                control.BackColor = SystemColors.Window;
            }
            else
            {
                control.Enabled = false;
                control.BackColor = SystemColors.ControlLight;
                (control as TextBox)?.Clear();
            }
        }

        private void Campaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCampaign = Campaign.SelectedItem as Campaigns;
            EnableDisable(LowerDelinquency, selectedCampaign.DisabledUiFields, UiFields.LowerDelinquency);
            EnableDisable(UpperDelinquency, selectedCampaign.DisabledUiFields, UiFields.UpperDelinquency);
            EnableDisable(Segment, selectedCampaign.DisabledUiFields, UiFields.Segment);
            EnableDisable(PerformanceCat, selectedCampaign.DisabledUiFields, UiFields.PerformanceCategory);
            EnableDisable(NumberToSend, selectedCampaign.DisabledUiFields, UiFields.NumberToSend);

            foreach (int i in Segment.CheckedIndices)
                Segment.SetItemCheckState(i, CheckState.Unchecked);

            foreach (int i in PerformanceCat.CheckedIndices)
                PerformanceCat.SetItemCheckState(i, CheckState.Unchecked);

            ExcelPreview.DataSource = null;
        }

        private async void Export_Click(object sender, EventArgs e)
        {
            if (!Dialog.Warning.YesNo("You are about to load the displayed accounts into Nobles Exclusion list.  Are you sure you want to continue?"))
                return;
            string campaign = SelectedCampaign.CampaignCode;

            await Task.Factory.StartNew(() => ExportData(campaign),
                    TaskCreationOptions.LongRunning);

            DialogResult = DialogResult.OK;
        }

        private void ExportData(string campaign)
        {
            SetLoading(true);
            DA.InsertExclusions(Data);
            List<FileData> newData = DA.InsertTrackingGetResults(Data, campaign);
            CreateAndSaveCsv(newData, campaign);
            SetLoading(false);
        }

        private void CreateAndSaveCsv(List<FileData> newData, string campaign)
        {
            string file = Path.Combine(EnterpriseFileSystem.TempFolder, $"cs_text-{campaign}-{DateTime.Now.ToString("MMddyyyyhhmmss")}.csv");
            using (StreamW sw = new StreamW(file, false))
            {
                sw.WriteLine("friendly_to,content_type,first_name");
                foreach (FileData item in newData)
                    sw.WriteLine(item.ToString());
            }

            Dialog.Info.Ok($"The file {file} have been created.");
        }

        private void LowerAge_Leave(object sender, EventArgs e)
        {
            if (LowerAge.Text.ToInt() < lowerAgeLimit)
            {
                Dialog.Info.Ok($"Value must be between {lowerAgeLimit} and {upperAgeLimit}.");
                LowerAge.BackColor = Color.Pink;
                LowerAge.Clear();
            }
            else
            {
                LowerAge.BackColor = SystemColors.Window;
            }
        }

        private void UpperAge_Leave(object sender, EventArgs e)
        {
            if (UpperAge.Text.ToInt() > upperAgeLimit)
            {
                Dialog.Info.Ok($"Value must be between {lowerAgeLimit} and {upperAgeLimit}.");
                UpperAge.BackColor = Color.Pink;
                UpperAge.Clear();
            }
            else
            {
                UpperAge.BackColor = SystemColors.Window;
            }
        }
    }
}