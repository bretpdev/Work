using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace REQUETASK
{
    public partial class RequeueTaskForm : Form
    {
        private ReflectionInterface ri;
        public RequeueTaskForm(ReflectionInterface ri)
        {
            this.ri = ri;
            InitializeComponent();
            LoadControls();
            LoadScreenInfo();
        }

        private void LoadControls()
        {
            ForBox.DataSource = ForGroups.All;
            ForBox.SelectedIndexChanged += (o, ea) =>

            {
                if (queue.IsNullOrEmpty())
                    ArcBox.Text = SelectedForGroup.Arc;
            };
        }
        private ForGroup SelectedForGroup { get { return ForGroups.All[ForBox.SelectedIndex]; } }

        private string queue;
        private string subQueue;
        #region PrePopulation
        private void LoadScreenInfo()
        {
            if (ri.ScreenCode == "TXX6S")
            {
                ForBox.Enabled = false;
                LoadTX6QScreenInfo();
            }
            else if (ri.ScreenCode == "TCX13")
            {
                LoadTC00ScreenInfo();
            }
        }
        private void LoadTX6QScreenInfo()
        {
            queue = ri.GetText(4, 19, 2);
            subQueue = ri.GetText(5, 19, 2);
            SsnBox.Text = ri.GetText(1, 15, 9);
            ArcBox.Text = ri.GetText(6, 19, 5);
            ArcDescBox.Text = ri.GetText(6, 27, 50);
            RequeueTextBox.Text = ri.GetText(16, 2, 78) + " " + ri.GetText(20, 2, 78);
        }
        private void LoadTC00ScreenInfo()
        {
            SsnBox.Text = ri.GetText(1, 9, 9);
            string comment = string.Join(" ", Enumerable.Range(12, 6).Select(i => ri.GetText(i, 10, 60)).ToArray());
            comment = comment.Replace("_", "");
            RequeueTextBox.Text = comment;
        }
        #endregion
        private bool LP9OProcessing()
        {
            ri.FastPath("LP9OA" + SsnBox.Text + ";;ZZZZCOMP");
            DateTime date = DateTime.Parse(DateBox.Text);
            ri.PutText(11, 25, date.ToString("MMddyyyy"));
            ri.PutText(16, 12, string.Join(", ", SsnBox.Text, ArcBox.Text, SelectedForGroup.Code, RequeueTextBox.Text));
            ri.Hit(ReflectionInterface.Key.Enter);
            ri.Hit(ReflectionInterface.Key.F6);
            return ri.MessageCode == "48003"; 
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (LP9OProcessing())
            {
                if (queue.IsNullOrEmpty())
                {
                    ri.FastPath("TX3Z/ITX6X");
                    ri.Hit(ReflectionInterface.Key.EndKey);
                    ri.Hit(ReflectionInterface.Key.Enter);
                }
                else
                {
                    ri.FastPath("TX3Z/ITX6X" + queue + ";" + subQueue);
                }
                MessageBox.Show("The task has been successfully re-queued");
                this.DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("The queue task wasn't able to be added.");
        }

        private void Validation()
        {
            ConfirmButton.Enabled = SsnBox.Text.Length == 9 && DateBox.Text.Length == 10;
        }

        private void DateBox_ValidationOnLeave(object sender, SimpleValidationEventArgs e)
        {
            DateTime parse = DateTime.Now;
            e.Valid = DateTime.TryParse(DateBox.Text, out parse);
            DateBox.Text = parse.ToString("MM/dd/yyyy");
            Validation();
        }

        private void SsnBox_ValidationOnLeave(object sender, SimpleValidationEventArgs e)
        {
            e.Valid = SsnBox.Text.Length == 9;
            Validation();
        }

        private void SsnBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validation();
        }
    }
}
