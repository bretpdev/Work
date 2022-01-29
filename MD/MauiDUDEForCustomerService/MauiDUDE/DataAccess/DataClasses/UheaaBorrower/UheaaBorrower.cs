using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class UheaaBorrower : Borrower
    {
        //This is updated asynchronously when DemographcisUI is being run
        public EmployerDemographics EmployerDemo { get; set; }

        public UheaaBorrower() : base()
        {

        }

        public UheaaBorrower(Borrower tempBorrower) : base(tempBorrower)
        {

        }
    }
}
