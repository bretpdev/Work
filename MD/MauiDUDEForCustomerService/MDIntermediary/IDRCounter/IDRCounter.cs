using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIntermediary
{
    public partial class IDRCounter : UserControl
    {
        public BindingList<IDRCountRecord> PGM1;
        public BindingList<IDRCountRecord> PGM2;
        public BindingList<IDRCountRecord> PGM3;
        public BindingList<IDRCountRecord> PGM4;

        public IDRCounter()
        {
            PGM1 = new BindingList<IDRCountRecord>();
            PGM2 = new BindingList<IDRCountRecord>();
            PGM3 = new BindingList<IDRCountRecord>();
            PGM4 = new BindingList<IDRCountRecord>();
            InitializeComponent();
            dataGridView1.DataSource = PGM1;
            dataGridView2.DataSource = PGM2;
            dataGridView3.DataSource = PGM3;
            dataGridView4.DataSource = PGM4;
        }

        public void UpdateTabs(string accountNumber)
        {
            if(PGM1.Count == 0 && PGM2.Count == 0 && PGM3.Count == 0 && PGM4.Count == 0)
            {
                IDRCounterDataAccess DA = new IDRCounterDataAccess();
                List<IDRCountRecord> records = DA.GetIDRCounterRecords(accountNumber);
                foreach (IDRCountRecord r in records)
                {
                    if (r.LoanForgivenessProgram == 1)
                    {
                        PGM1.Add(r);
                    }
                    else if (r.LoanForgivenessProgram == 2)
                    {
                        PGM2.Add(r);
                    }
                    else if (r.LoanForgivenessProgram == 3)
                    {
                        PGM3.Add(r);
                    }
                    else if (r.LoanForgivenessProgram == 4)
                    {
                        PGM4.Add(r);
                    }
                }
            }
        }
    }
}
