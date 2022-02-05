using System.Collections.Generic;
using System.Linq;
using Q;

namespace ACDCAccess
{
	partial class AddAndRemoveAccessBUBased : BaseMainTabUserControl
	{
		public AddAndRemoveAccessBUBased()
			: base()
		{
			InitializeComponent();
		}

		public AddAndRemoveAccessBUBased(bool testMode)
			: base(testMode)
		{
			InitializeComponent();
			List<BusinessUnit> businessUnits = DataAccess.GetBusinessUnits(testMode).ToList();
			foreach (BusinessUnit bu in businessUnits)
			{
				pnlData.Controls.Add(new BusinessUnitRoot(testMode, bu));
			}
		}
	}
}
