using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PUTSUSPCOM
{
    public partial class OptionUIBase : UserControl
    {

        public OptionProcessorBase Option { get; set; }

        public OptionUIBase()
        {
            InitializeComponent();
        }

    }
}
