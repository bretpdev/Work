using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace KEYIDCHNG
{
    public class KeyIdentifierChangeComparison
    {
        public bool AnyChanges
        {
            get
            {
                return FirstNameChanged || MiddleInitialChanged || LastNameChanged || DobChanged;
            }
        }
        public bool FirstNameChanged { get; set; }
        public bool MiddleInitialChanged { get; set; }
        public bool LastNameChanged { get; set; }
        public bool DobChanged { get; set; }
        public KeyIdentifierChangeComparison(KeyIdentifierChangeModel newInfo, SystemBorrowerDemographics oldDemos)
        {
            FirstNameChanged = newInfo.FirstName?.ToUpper() != oldDemos.FirstName?.ToUpper() && !newInfo.FirstName.IsNullOrEmpty();
            MiddleInitialChanged = newInfo.MiddleInitial?.ToUpper() != oldDemos.MiddleIntial?.ToUpper() && !newInfo.MiddleInitial.IsNullOrEmpty();
            LastNameChanged = newInfo.LastName != oldDemos.LastName?.ToUpper() && !newInfo.LastName.IsNullOrEmpty();
            DobChanged = newInfo.DOB.HasValue && newInfo.DOB != oldDemos.DateOfBirth.ToDateNullable();
        }
    }
}
