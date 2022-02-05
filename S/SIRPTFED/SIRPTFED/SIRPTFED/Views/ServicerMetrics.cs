using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common;


namespace SIRPTFED
{
    public partial class ServicerMetrics : Form
    {
        private List<ServicerCategories> SerCategories { get; set; }
        private List<ServicerMetricsData> SerMetrics { get; set; }
        private bool IsAddition { get; set; }
        private MetricSummaryData Data { get; set; }
        private DataAccess da { get; set; }


        private int userId { get; set; }
        public ServicerMetrics(int UserId, DataAccess DA, MetricSummaryData data = null)
        {
            InitializeComponent();
            da = DA;
            Data = data;
            userId = UserId;
            IsAddition = data == null;
            if (IsAddition)
                LoadNewItem();
            else
                LoadExistingItem(data);

        }

        private void LoadExistingItem(MetricSummaryData data)
        {
            Category.DataSource = new List<string>() { data.Category };
            Category.SelectedIndex = 0;
            Category.Enabled = false;

            Metric.DataSource = new List<string>() { data.Metric };
            Metric.SelectedIndex = 0;
            Metric.Enabled = false;

            Month.DataSource = new List<string>() { data.MetricMonth.ToString() };
            Month.SelectedIndex = 0;
            Month.Enabled = false;

            Year.Text = data.MetricYear.ToString();
            Year.Enabled = false;

            CompliantRecords.Text = data.CompliantRecords.ToString();
            TotalRecords.Text = data.TotalRecords.ToString();
            AvgBacklog.Text = data.AvgBacklog.ToString();

            SuspenseAmountDecimalTextBox.Text = data.SuspenseAmount.HasValue ? data.SuspenseAmount.ToString() : "";
            SuspenseAmountDecimalTextBox.Enabled = data.Metric == "Payment Suspense";

            SuspenseTotalDecimalTextBox.Text = data.SuspenseTotal.HasValue ? data.SuspenseTotal.ToString() : "";
            SuspenseTotalDecimalTextBox.Enabled = data.Metric == "Payment Suspense";
        }

        private void LoadNewItem()
        {
            Year.Text = DateTime.Now.Year.ToString();
            Month.DataSource = new List<string>() { (DateTime.Now.AddMonths(-1).Month).ToString(), DateTime.Now.Month.ToString() };
            Month.SelectedIndex = -1;
            SerCategories = da.PopulateCategories(userId);
            Category.DataSource = SerCategories.Select(p => p.ServicerCategory).ToList();
            Category.SelectedIndex = -1;
            SuspenseAmountDecimalTextBox.Enabled = false;
            SuspenseTotalDecimalTextBox.Enabled = false;
        }

        private void Category_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int category = SerCategories.Where(p => p.ServicerCategory == Category.SelectedItem.ToString()).Select(p => p.ServicerCategoryId).First();
            SerMetrics = da.PopulateMetrics(userId, category);
            Metric.DataSource = SerMetrics.Select(p => p.ServicerMetric).ToList();
            Metric.SelectedIndex = -1;
        }

        private bool ValidateInput()
        {
            List<string>errors = new List<string>();
            if (Category.Text.IsNullOrEmpty())
                errors.Add("You must select a Category.");
            if(Metric.Text.IsNullOrEmpty())
                errors.Add("You must select a Metric.");
            if (Month.Text.IsNullOrEmpty())
                errors.Add("You must select a Month.");
            if (Year.Text.IsNullOrEmpty())
                errors.Add("You must enter a Year.");
            if (CompliantRecords.Text.IsNullOrEmpty())
                errors.Add("You must enter the number of Compliant Records.");
            if (TotalRecords.Text.IsNullOrEmpty())
                errors.Add("You must enter the number of Total Records.");
            if(AvgBacklog.Text.IsNullOrEmpty())
                errors.Add("You must select the AverageBackLog.");
            if (TotalRecords.Text.ToInt() < CompliantRecords.Text.ToInt())
                errors.Add("The number of compliant records cannot be greater then the number of total records.");
            if(!Metric.Text.IsNullOrEmpty() && Metric.Text == "Payment Suspense")
            {
                if(!CheckSuspenseValueIsMoney())
                {
                    errors.Add("You must provide a valid, greater than zero, money value for Suspense Amount and Suspense Total.");
                }
            }

            if (errors.Any())
            {
                Dialog.Error.Ok("Please reivew the following errors: \r\n \r\n" + string.Join(Environment.NewLine, errors));
                return false;
            }

            return true;
        }

        private bool CheckSuspenseValueIsMoney()
        {
            decimal? suspenseAmount = SuspenseAmountDecimalTextBox.Text.ToDecimalNullable();
            decimal? suspenseTotal = SuspenseTotalDecimalTextBox.Text.ToDecimalNullable();
            
            if(!suspenseAmount.HasValue || !suspenseTotal.HasValue)
            {
                return false;
            }

            if(suspenseAmount.Value < 0 || suspenseTotal < 0)
            {
                return false;
            }

            if(SuspenseAmountDecimalTextBox.Text.Length > 16 || SuspenseTotalDecimalTextBox.Text.Length > 16)
            {
                return false;
            }

            //Makes sure that there are not more than 2(meaningful) Decimal places in the provided values
            decimal amountDecimalCheck = suspenseAmount.Value * 100;
            decimal totalDecimalCheck = suspenseTotal.Value * 100;
            if (amountDecimalCheck == Math.Floor(amountDecimalCheck) && totalDecimalCheck == Math.Floor(totalDecimalCheck))
            {
                return true;
            }

            return false;
        }

        private void validationButton1_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;
            if (IsAddition)
            {
                var sqlParams = new SqlParameter[] 
                {
                    SqlParams.Single("ServicerMetricId", SerMetrics.Where(p => p.ServicerMetric == Metric.Text).First().ServicerMetricsId),
                    SqlParams.Single("ServicerCategoryId", SerCategories.Where(p => p.ServicerCategory == Category.Text).First().ServicerCategoryId),
                    SqlParams.Single("MetricMonth", Month.Text),
                    SqlParams.Single("MetricYear",Year.Text),
                    SqlParams.Single("ComplaintRecords", CompliantRecords.Text),
                    SqlParams.Single("TotalRecords", TotalRecords.Text),
                    SqlParams.Single("AvgBacklog", AvgBacklog.Text),
                    SqlParams.Single("SuspenseAmount", SuspenseAmountDecimalTextBox.Text == "" ? DBNull.Value : (object)SuspenseAmountDecimalTextBox.Text.ToDecimal()),
                    SqlParams.Single("SuspenseTotal", SuspenseTotalDecimalTextBox.Text == "" ? DBNull.Value : (object)SuspenseTotalDecimalTextBox.Text.ToDecimal())
                };
                try
                {
                    da.InsertMetric(sqlParams);
                    DialogResult = DialogResult.OK;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                object suspenseAmountDbValue = SuspenseAmountDecimalTextBox.Text == "" ? DBNull.Value : (object)SuspenseAmountDecimalTextBox.Text.ToDecimal();
                object suspenseTotalDbValue = SuspenseTotalDecimalTextBox.Text == "" ? DBNull.Value : (object)SuspenseTotalDecimalTextBox.Text.ToDecimal();
                da.UpdateMetric(Data.MetricsSummaryId, Int32.Parse(TotalRecords.Text), Int32.Parse(CompliantRecords.Text), Int32.Parse(AvgBacklog.Text), suspenseAmountDbValue, suspenseTotalDbValue);
                DialogResult = DialogResult.OK;
            }
        }

        private void Metric_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!Metric.Text.IsNullOrEmpty() && Metric.Text == "Payment Suspense")
            {
                SuspenseAmountDecimalTextBox.Enabled = true;
                SuspenseTotalDecimalTextBox.Enabled = true;
            }
            else
            {
                SuspenseAmountDecimalTextBox.Enabled = false;
                SuspenseTotalDecimalTextBox.Enabled = false;
            }
        }
    }
}
