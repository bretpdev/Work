using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public partial class FormBuilder
    {
        public List<IFormField<Control>> Fields { get; internal set; }
        public string WindowTitle { get; set; }
        public Font Font { get; set; }
        public int InputWidth { get; set; }
        public bool IncludeAlternateButton { get; set; }
        public string AcceptButtonText { get; set; }
        public string CancelButtonText { get; set; }
        public string AlternateButtonText { get; set; }

        public FormBuilder(string windowTitle) : this(windowTitle, new Font(FontFamily.GenericSansSerif, 12), 150) { }
        public FormBuilder(string windowTitle, Font font, int inputWidth)
        {
            Fields = new List<IFormField<Control>>();
            WindowTitle = windowTitle.Trim();
            Font = font;
            InputWidth = inputWidth;
            AcceptButtonText = "Save";
            CancelButtonText = "Cancel";
            AlternateButtonText = "Delete";
            IncludeAlternateButton = false;
        }

        private Dictionary<IFormField<Control>, Control> GeneratedControls { get; set; }
        public T GetControl<T>(IFormField<T> field) where T : Control, new()
        {
            return (T)GeneratedControls[field];
        }

        public IFormField<T> AddField<T>(string label, Action<T> setProperties, int? ordinal = null) where T : Control, new()
        {
            IFormField<T> field = new FormField<T>(label, setProperties, ordinal);
            Fields.Add(field);
            return field;
        }

        public delegate bool OnFormAccepted(BuiltForm form);
        public event OnFormAccepted FormAccepted;
        public delegate bool OnFormCancelled(BuiltForm form);
        public event OnFormCancelled FormCancelled;
        public delegate bool OnFormAlternate(BuiltForm form);
        public event OnFormAlternate FormAlternate;

        public BuiltForm Build()
        {
            OrderFields();

            BuiltForm form = new BuiltForm(this);
            form.Text = WindowTitle;
            form.AutoSize = true;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.MinimumSize = Size.Empty;

            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Visible = true;
            panel.ColumnCount = 2;
            panel.RowCount = Fields.Count;
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            form.Controls.Add(panel);

            int row = 0;
            int totalHeight = 0;
            int labelWidth = Fields.Max(o => TextRenderer.MeasureText(o.Label, Font).Width);
            GeneratedControls = new Dictionary<IFormField<Control>, Control>();
            foreach (IFormField<Control> field in Fields)
            {
                var label = new Label();
                label.Text = field.Label;
                label.Visible = true;
                label.TextAlign = ContentAlignment.MiddleRight;
                label.Font = Font;
                label.Dock = DockStyle.Fill;
                label.Width = labelWidth;
                panel.Controls.Add(label, 0, row);
                

                var control = field.GenerateControl();
                if (control is TextBox || control is ListBox || control is ComboBox || control is DateTimePicker) //TODO: create list of applicable controls
                    control.Width = InputWidth;
                (control as INullableControl<Control>).IfExists(o => o.SetInputWidth(InputWidth));
                if (control is ComboBox)
                    control.Margin = new Padding(0, 10, 0, 10);
                control.Visible = true;
                control.Font = Font;
                field.FinalizeControl(control);
                if (control is Button)
                    NormalizeHeight(control as Button);
                GeneratedControls[field] = control;
                form.InputControls.Add(control);
                panel.Controls.Add(control, 1, row);

                panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                totalHeight += panel.GetRowHeights()[row];
                row++;
            }

            //divider row
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var groupBox = new Control()
            {
                Dock = DockStyle.Fill,
                Height = 1,
                BackColor = Color.DarkGray
            };
            panel.Controls.Add(groupBox, 0, row);
            panel.SetColumnSpan(groupBox, 2);
            totalHeight += panel.GetRowHeights()[row];
            row++;

            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); //bottom row

            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.FlowDirection = FlowDirection.RightToLeft;
            flowPanel.Dock = DockStyle.Fill;
            panel.Controls.Add(flowPanel, 1, row);

            ValidationButton acceptButton = new ValidationButton();
            SetButton(acceptButton, AcceptButtonText);
            form.AcceptButton = acceptButton;
            acceptButton.OnValidate += (o, ea) =>
            {
                if (ea.FormIsValid)
                    if (FormAccepted != null && FormAccepted(form))
                    {
                        form.DialogResult = DialogResult.OK;
                    }
            };
            flowPanel.Controls.Add(acceptButton);
            if (IncludeAlternateButton)
            {
                Button alternateButton = BuildButton(AlternateButtonText);
                alternateButton.Click += (o, ea) =>
                {
                    if (FormAlternate != null && FormAlternate(form))
                    {
                        form.DialogResult = DialogResult.Ignore; //TODO: Use a better enum (DialogResult is starting to become an awkward fit).
                    }
                };
                flowPanel.Controls.Add(alternateButton);
            }
            flowPanel.Height = acceptButton.Height;
            flowPanel.Margin = new Padding(0);

            Button cancelButton = BuildButton(CancelButtonText);
            cancelButton.Text = CancelButtonText;
            form.CancelButton = cancelButton;
            cancelButton.Click += (o, ea) =>
            {
                if (FormCancelled != null)
                {
                    if (FormCancelled(form))
                    {
                        form.DialogResult = DialogResult.Cancel;
                    }
                }
                else
                {
                    form.DialogResult = DialogResult.Cancel;
                }
            };
            panel.Controls.Add(cancelButton, 0, row);
            totalHeight += panel.GetRowHeights()[row];

            panel.Height = totalHeight;
            panel.Width = panel.GetColumnWidths()[1] + panel.GetColumnWidths()[0];

            form.Height = 0; //force a height autosize

            return form;
        }

        private Button BuildButton(string caption)
        {
            Button button = new Button();
            SetButton(button, caption);
            return button;
        }
        private void SetButton(Button button, string caption)
        {
            button.Text = caption;
            button.Font = Font;
            NormalizeHeight(button);
        }

        private void OrderFields()
        {
            Fields = Fields.OrderBy(o => o.Ordinal).ToList();
        }

        private static void NormalizeHeight(Button b)
        {
            b.Height = TextRenderer.MeasureText(b.Text, b.Font).Height + 10;
        }
    }
}
