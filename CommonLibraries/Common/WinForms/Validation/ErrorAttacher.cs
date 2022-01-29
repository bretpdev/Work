using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class ErrorAttacher
    {
        readonly Color errorColor = Color.LightPink;
        public int ErrorCount { get { return attachedErrors.Count + AdditionalErrorCount; } }
        /// <summary>
        /// Used for logging errors that don't trigger any visible validation
        /// </summary>
        public int AdditionalErrorCount { get; set; }

        List<AttachedError> attachedErrors = new List<AttachedError>();
        static CustomToolTip tt = new CustomToolTip();
        public void SetError(string message, Control control, Label associatedLabel = null)
        {
            var error = new AttachedError()
            {
                Control = control,
                AssociatedLabel = associatedLabel,
                Message = message,
                OriginalControlBackColor = control.BackColor,
                OriginalLabelBackColor = associatedLabel?.BackColor
            };
            if (associatedLabel != null)
            {
                var possibleExistingLabelMatches = attachedErrors.Where(o => o.AssociatedLabel == associatedLabel);
                if (possibleExistingLabelMatches.Any())
                    error.OriginalLabelBackColor = possibleExistingLabelMatches.First().OriginalLabelBackColor;
            }
            var possibleMatch = attachedErrors.SingleOrDefault(o => o.Control == control);
            if (possibleMatch != null)
                error = possibleMatch;
            else
                attachedErrors.Add(error);

            control.BackColor = errorColor;
            control.GotFocus += ErrorControl_GotFocus;
            control.LostFocus += ErrorControl_LostFocus;
            if (control.Focused)
                SetErrorControlFocused(error);
            if (associatedLabel != null)
            {
                associatedLabel.BackColor = errorColor;
                tt.SetToolTip(associatedLabel, message);
            }
            if (control is Label)
                tt.SetToolTip(control, message);
            control.FindForm().ResizeBegin += ErrorAttacher_FormResizeBegin;
            control.FindForm().ResizeEnd += ErrorAttacher_FormResizeEnd;
        }

        private void ErrorControl_LostFocus(object sender, EventArgs e)
        {
            var matchingError = attachedErrors.SingleOrDefault(o => o.Control == (Control)sender);
            if (matchingError != null)
                SetErrorControlUnfocused(matchingError);
        }

        private void ErrorControl_GotFocus(object sender, EventArgs e)
        {
            var matchingError = attachedErrors.SingleOrDefault(o => o.Control == (Control)sender);
            if (matchingError != null)
                SetErrorControlFocused(matchingError);
        }

        private void ErrorAttacher_FormResizeEnd(object sender, EventArgs e)
        {
            foreach (var error in attachedErrors)
                if (error.Control.Focused)
                    SetErrorControlFocused(error);
        }

        private void ErrorAttacher_FormResizeBegin(object sender, EventArgs e)
        {
            foreach (var error in attachedErrors)
                SetErrorControlUnfocused(error);
        }

        private void SetErrorControlFocused(AttachedError error)
        {
            //SetErrorControlUnfocused(error); //remove any existing tooltips

            var mousePosition = Cursor.Position;
            bool cursorOverControl = error.Control.ClientRectangle.Contains(error.Control.PointToClient(mousePosition));
            cursorOverControl = true;
            if (cursorOverControl)
                Cursor.Position = new Point(int.MaxValue, int.MaxValue);

            tt.Show("", error.Control, 0); //fix for a bug in ToolTip

            tt.Show(error.Message, error.Control, 0, error.Control.Height, int.MaxValue);

            if (cursorOverControl)
                Cursor.Position = mousePosition;
        }

        private void SetErrorControlUnfocused(AttachedError error)
        {
            tt.Hide(error.Control);
        }

        public void ClearAllErrors()
        {
            AdditionalErrorCount = 0;
            tt.RemoveAll();
            foreach (var error in attachedErrors)
            {
                error.Control.BackColor = error.OriginalControlBackColor;
                if (error.AssociatedLabel != null)
                    error.AssociatedLabel.BackColor = error.OriginalLabelBackColor.Value;
                error.Control.GotFocus -= ErrorControl_GotFocus;
                error.Control.LostFocus -= ErrorControl_LostFocus;
                SetErrorControlUnfocused(error);
            }
            if (attachedErrors.Any())
            {
                var form = attachedErrors.First().Control.FindForm();
                if (form != null)
                {
                    form.ResizeBegin -= ErrorAttacher_FormResizeBegin;
                    form.ResizeEnd -= ErrorAttacher_FormResizeEnd;
                }
            }
            attachedErrors.Clear();
        }

        class AttachedError
        {
            public Control Control { get; set; }
            public Label AssociatedLabel { get; set; }
            public string Message { get; set; }
            public Color OriginalControlBackColor { get; set; }
            public Color? OriginalLabelBackColor { get; set; }
        }
    }

    public class CustomToolTip : ToolTip
    {
        const int margin = 10;
        public CustomToolTip()
        {
            this.OwnerDraw = true;
            this.Popup += new PopupEventHandler(this.OnPopup);
            this.Draw += new DrawToolTipEventHandler(this.OnDraw);

        }

        private void OnPopup(object sender, PopupEventArgs e) // use this event to set the size of the tool tip
        {
            using (Graphics g = Graphics.FromHwnd(e.AssociatedControl.Handle))
            {
                SizeF s = g.MeasureString(GetToolTip(e.AssociatedControl), e.AssociatedControl.FindForm().Font);
                e.ToolTipSize = new Size((int)s.Width + margin * 2, (int)s.Height + margin * 2);
            }
        }

        private void OnDraw(object sender, DrawToolTipEventArgs e) // use this event to customise the tool tip
        {
            Graphics g = e.Graphics;
            Brush b = new SolidBrush(Color.White);

            g.FillRectangle(b, e.Bounds);

            g.DrawRectangle(new Pen(Brushes.Red, 1), new Rectangle(e.Bounds.X, e.Bounds.Y,
                e.Bounds.Width - 1, e.Bounds.Height - 1));

            var font = e.AssociatedControl.FindForm().Font;

            g.DrawString(e.ToolTipText, font, Brushes.Black, new PointF(e.Bounds.X + margin, e.Bounds.Y + margin));

            b.Dispose();
        }
    }

}
