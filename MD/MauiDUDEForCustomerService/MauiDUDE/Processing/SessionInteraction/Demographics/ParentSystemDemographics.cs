using MDIntermediary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;

namespace MauiDUDE
{
    public abstract class ParentSystemDemographics
    {
        public abstract void Populate(Borrower borrower);
        public abstract void Update(string source, UpdateDemoCompassIndicators systemsUpdateIndicators, bool isSchool, Demographics demosForUpdating, DemographicVerifications demographicVerifications, MDBorrowerDemographics altAddress);

    }
}
