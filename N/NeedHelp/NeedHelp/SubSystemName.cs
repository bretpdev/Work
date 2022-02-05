using System;
using System.Windows.Forms;
using System.Drawing;

namespace NeedHelp
{
    public partial class SubSystemName : UserControl
    {
        private readonly string _name;
        public string SubSystem
        {
            get
            {
                return _name;
            }
        }

		private bool _isSelected;
        public bool IsSelected
        {
			get { return _isSelected; }
            set
            {
				_isSelected = value;
				btnSubSystemName.BackColor = (_isSelected ? Color.LightGreen : Color.LightGray);
            }
        }

        public SubSystemName(string name)
        {
            InitializeComponent();
            btnSubSystemName.Text = name;
            _name = name;
        }

        public SubSystemName()
        {
            InitializeComponent();
        }

        public class ClickedEventArgs : EventArgs
        {
            public readonly string Name;

            public ClickedEventArgs(string name)
            {
                Name = name;
            }
        }
        public event EventHandler<ClickedEventArgs> Clicked;
        protected virtual void OnClicked(ClickedEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        private void BtnSubSystemName_Click(object sender, EventArgs e)
        {
            OnClicked(new ClickedEventArgs(btnSubSystemName.Text));
        }
    }
}
