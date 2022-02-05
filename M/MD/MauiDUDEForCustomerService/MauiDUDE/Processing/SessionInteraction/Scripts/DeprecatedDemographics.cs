using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    /// <summary>
    /// DO NOT USE, THIS IS USED FOR BACKWARDS COMPATIBILITY WITH Q
    /// </summary>
    class DeprecatedDemographics : Q.MDBorrowerDemographics
    {
        public override void PopulateObjectFromSystem()
        {
            throw new NotImplementedException();
        }
    }
}
