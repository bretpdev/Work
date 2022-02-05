using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.WinForms;

namespace CommonTesting
{
    public partial class CycleButtonTest : Form
    {
        public CycleButtonTest()
        {
            InitializeComponent();
            EnumCycleButton button = new EnumCycleButton();
            button.EnumType = typeof(TestEnum);
            this.Controls.Add(button);
        }

        public enum TestEnum
        {
            [Description("ONE")]
            One,
            Two
        }
    }
}
