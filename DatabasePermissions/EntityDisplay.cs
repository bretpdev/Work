using System.Windows.Forms;

namespace DatabasePermissions
{
	partial class EntityDisplay : UserControl
	{
		private readonly Entity _entity;
		public Entity Entity { get { return _entity; } }

		public bool Insert
		{
			get { return chkInsert.Checked; }
			set { chkInsert.Checked = value; }
		}

		new public bool Update
		{
			get { return chkUpdate.Checked; }
			set { chkUpdate.Checked = value; }
		}

		public bool Delete
		{
			get { return chkDelete.Checked; }
			set { chkDelete.Checked = value; }
		}

		new public bool Select
		{
			get { return chkSelect.Checked; }
			set { chkSelect.Checked = value; }
		}

		public bool Execute
		{
			get { return chkExecute.Checked; }
			set { chkExecute.Checked = value; }
		}

		public EntityDisplay(Entity entity)
		{
			InitializeComponent();
			_entity = entity;
			entityBindingSource.DataSource = _entity;
		}
	}//class
}//namespace
