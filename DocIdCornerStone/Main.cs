using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Q;

namespace DocIdCornerStone
{
	partial class Main : FormBase
	{
		private readonly IEnumerable<DocumentDetail> _documentDetails;
		private readonly IEnumerable<DocumentSource> _documentSources;
		private readonly UserInput _userInput;

		public Main(IEnumerable<DocumentDetail> documentDetails, IEnumerable<DocumentSource> sources, UserInput userInput)
		{
			InitializeComponent();
			_documentDetails = documentDetails;
			_documentSources = sources;
			_userInput = userInput;
			cmbDescription.DataSource = documentDetails;
			cmbSource.DataSource = sources;
			userInputBindingSource.DataSource = userInput;
		}

		private void btnProcess_Click(object sender, EventArgs e)
		{
			DocumentDetail selectedDocument = cmbDescription.SelectedItem as DocumentDetail;
			_userInput.DocumentDetail = _documentDetails.Single(p => p.DocId == selectedDocument.DocId && p.Description == selectedDocument.Description);
			_userInput.DocumentSource = _documentSources.Single(p => p.Code == (cmbSource.SelectedItem as DocumentSource).Code);
			this.DialogResult = DialogResult.OK;
		}
	}//class
}//namespace
