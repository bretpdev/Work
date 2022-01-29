using System;
using System.Windows.Forms;

namespace INCIDENTRP
{
	//Effing designer won't let us see forms that inherit an abstract class.
	partial class BaseDetail : UserControl
	{
		protected bool _isValidated;

		public BaseDetail()
		{
			InitializeComponent();
		}

		//This would be abstract if it weren't for an apparent shortcoming in Visual Studio's
		//ability to display (in the designer) forms that inherit an abstract class.
		public virtual void CheckValidity() { }
	}
}
