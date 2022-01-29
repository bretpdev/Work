using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using Q;

namespace ACDCFlows
{
	public partial class FlowStepControl : UserControl
	{
		public FlowStepControl(FlowStep step, List<SqlUser> users)
		{
			InitializeComponent();
			flowStepBindingSource.DataSource = step;
			lblBusinessUnit.Text = step.AccessAlsoBasedOffBusinessUnit == true ? "True" : "False";
		}
	}
}
