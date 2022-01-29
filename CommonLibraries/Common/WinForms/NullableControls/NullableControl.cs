using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public interface INullableControl<out T> where T : Control, new()
    {
        bool IsChecked { get; set; }
        string PropertyName { get; set; }
        int CheckBoxWidth { get; }
        T Field { get; }
        void SetInputWidth(int width);
    }
    public class NullableControl<T> : TableLayoutPanel, INullableControl<T> where T : Control, new()
    {
        #region Properties
        public bool IsChecked
        {
            get { return CheckBox.Checked; }
            set { CheckBox.Checked = value; }
        }
        public string PropertyName { get; set; }
        public CheckBox CheckBox { get; set; }
        public T Field { get; private set; }
        public int CheckBoxWidth { get; private set; }
        public void SetInputWidth(int width)
        {
            Field.Width = width - CheckBoxWidth - Field.Margin.Left - Field.Margin.Right;
        }

        #endregion

        public NullableControl()
        {
            CheckBoxWidth = 30;
            Field = new T()
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 3, 0, 3)
            };
            CheckBox = new CheckBox()
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                CheckAlign = ContentAlignment.MiddleCenter
                
            };
            this.AutoSize = true;

            CheckBox.CheckedChanged += CheckBox_CheckedChanged;

            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, CheckBoxWidth));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            this.Controls.Add(CheckBox, 0, 0);
            this.Controls.Add(Field, 1, 0);
        }

        void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox.Checked)
                Field.Enabled = true;
            else
                Field.Enabled = false;
        }
    }
}
