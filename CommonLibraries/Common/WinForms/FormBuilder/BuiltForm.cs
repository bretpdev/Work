using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace Uheaa.Common.WinForms
{
    public class BuiltForm : Form
    {
        public List<Control> InputControls { get; internal set; }
        public T GetInput<T>(int index) where T : Control
        {
            return (T)InputControls[index];
        }
        public T GetInput<T>(IFormField<T> formField) where T : Control, new()
        {
            return Builder.GetControl(formField);
        }
        FormBuilder Builder { get; set; }
        public BuiltForm(FormBuilder builder)
            : base()
        {
            InputControls = new List<Control>();
            Builder = builder;
        }

        /// <summary>
        /// Displays the form at the mouse location.  Does not allow for form movement.
        /// </summary>
        public bool? ShowPopoverDialog(Form owner)
        {
            FormPopoverManager manager = new FormPopoverManager(this);
            return manager.ShowPopoverDialog(owner);
        }
    }
}
