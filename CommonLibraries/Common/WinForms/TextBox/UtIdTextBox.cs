using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.WinForms
{
    [Designer(typeof(UtIdDesigner))]
    public class UtIdTextBox : Panel
    {
        private TableLayoutPanel Table { get; set; }
        public Label Label { get; private set; }
        public NumericTextBox TextBox { get; private set; }
        public event EventHandler UtIdChanged;
        const string LabelPrefix = "UT";
        static string LabelText = LabelPrefix;
        //these numbers don't take the UT prefix into account
        const int MaxIdLength = 5;
        static int IdLength { get { return (MaxIdLength + LabelPrefix.Length) - LabelText.Length; } }
        public UtIdTextBox()
        {
            Table = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill
            };
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));  //this will be reset once the label is finalized
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            Table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.Controls.Add(Table);

            Label = new Label()
            {
                Text = LabelText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0)
            };
            Label.DoubleClick += Label_DoubleClick;
            Label.Font = new Font(Label.Font, FontStyle.Bold);
            Table.Controls.Add(Label, 0, 0);

            TextBox = new NumericTextBox()
            {
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.CustomSource,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 3, 0),
                MaxLength = IdLength
            };
            TextBox.TextChanged += (o, ea) =>
            {
                UtIdChanged?.Invoke(this, new EventArgs());
            };
            Table.Controls.Add(TextBox, 1, 0);

            Sync();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string UtId
        {
            get
            {
                return LabelText + TextBox.Text;
            }
            set
            {
                bool startsWith = value.StartsWith(LabelText);
                while (!startsWith && LabelText.Length > 0)
                    LabelText = LabelText.Substring(0, LabelText.Length - 1);
                if (startsWith)
                {
                    string newValue = value.TrimLeft(LabelText);
                    TextBox.Text = newValue;
                }
                UtIdChanged?.Invoke(this, new EventArgs());
            }
        }

        public bool IsValid { get { return (LabelText + TextBox.Text).Length == 7; } }

        void Sync()
        {
            RefreshLabel();
            LoadAutoComplete();
        }
        void RefreshLabel()
        {
            Label.Text = LabelText;
            Table.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, Label.GetPreferredSize(Size.Empty).Width);
            TextBox.MaxLength = IdLength;
        }
        private void LoadAutoComplete()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;
            var ids = UtIdHelper.CachedUtIds.Select(o => o.TrimLeft(LabelText));
            if (ids.Any() && TextBox.Text.IsNullOrEmpty())
                TextBox.Text = ids.First();
            var collection = new AutoCompleteStringCollection();
            collection.AddRange(ids.ToArray());
            TextBox.AutoCompleteCustomSource = collection;
        }

        void Label_DoubleClick(object sender, EventArgs e)
        {
            LabelText = LabelPrefix;
            Sync();
            TextBox.Text = TextBox.Text.PadLeft(IdLength, '0');
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, TextBox.Height + TextBox.Margin.Top + TextBox.Margin.Bottom, specified);
        }

        /// <summary>
        /// Initializes the LabelText and TextBox.MaxLength to accomodate for the current maximum UT ID in BSYS
        /// </summary>
        static UtIdTextBox()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                string windowsUserName = Environment.UserName;

                // var mode = DataAccessHelper.CurrentMode;
                // DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
                int max = DataAccessHelper.ExecuteSingle<int>("GetUTIds", DataAccessHelper.Database.Bsys, new SqlParameter("LabelPrefix", LabelPrefix), new SqlParameter("LabelLen", LabelPrefix.Length));
                //  DataAccessHelper.CurrentMode = mode;
                LabelText = LabelPrefix + "".PadRight(MaxIdLength - max.ToString().Length, '0'); //determine number of leading redundant zeros and apply them to the label.

            }
        }
    }

    /// <summary>
    /// Ensures a UtId Control's height cannot be changed
    /// </summary>
    public class UtIdDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                return SelectionRules.RightSizeable | SelectionRules.LeftSizeable | SelectionRules.Moveable;
            }
        }
    }
}
