using System;
using System.Windows.Forms;

namespace ACDCFlows
{
	public partial class StaffAssignment : UserControl
	{
		public StaffAssignment(FlowStepInfoForUserSearch step)
		{
			InitializeComponent();
			flowStepInfoForUserSearchBindingSource.DataSource = step;
			lblBusinessUnit.Text = step.AccessAlsoBasedOffBusinessUnit == true ? "True" : "False";
		}
	}
}
