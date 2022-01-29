using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public partial class InputBox<C> : Form where C : Control, new()
    {
        internal InputBox()
        {
            InitializeComponent();
        }

        public InputBox(string message, string title = "Input", string okText = "OK", string cancelText = "Cancel")
            : this()
        {
            MessageLabel.Text = message;
            OkButton.Text = okText;
            CancelButton.Text = cancelText;
            this.Text = title;
            InputControl = new C() { Dock = DockStyle.Fill };
            ControlHost.Controls.Add(InputControl);
        }

        public C InputControl { get; private set; }
    }
}
