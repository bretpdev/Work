namespace INCIDENTRP
{
	partial class IncidentDetailNarrative : BaseDetail
	{
		public IncidentDetailNarrative(Incident incident)
		{
			InitializeComponent();
			incidentBindingSource.DataSource = incident;
		}

		public override void CheckValidity()
		{
			bool isValid = true;
			if (isValid != _isValidated)
			{
				_isValidated = isValid;
			}
		}
	}
}
