using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Windows.Forms;

namespace Payments
{
    public class ExtendedEmployerDemographics : EmployerDemographics
    {

        public string ID { get; set; }


        public ExtendedEmployerDemographics():base()
        {
        }

        public ExtendedEmployerDemographics(string id, string name, string addr, string city, string state, string zip):base()
        {
            ID = id;
            Name = name;
            Addr1 = addr;
            City = city;
            State = state;
            Zip = zip;
        }

    }
}
