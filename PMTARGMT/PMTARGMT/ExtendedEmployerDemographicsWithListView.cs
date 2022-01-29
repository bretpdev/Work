using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Payments
{
    public class ExtendedEmployerDemographicsWithListView:ListViewItem
    {

        public ExtendedEmployerDemographics EmployerDemos { get; set; }

        public ExtendedEmployerDemographicsWithListView(string id, string name, string addr, string city, string state, string zip):base()
        {
            EmployerDemos = new ExtendedEmployerDemographics(id, name, addr, city, state, zip);
            //create needed columns for list view item 
            this.Text = id;
            this.SubItems.Add(new ListViewSubItem(this,name));
            this.SubItems.Add(new ListViewSubItem(this, addr));
            this.SubItems.Add(new ListViewSubItem(this, city));
            this.SubItems.Add(new ListViewSubItem(this, state));
            this.SubItems.Add(new ListViewSubItem(this, zip));
        }

    }
}
