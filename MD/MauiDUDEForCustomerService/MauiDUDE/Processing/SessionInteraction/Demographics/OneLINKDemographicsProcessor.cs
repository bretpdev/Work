using MDIntermediary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace MauiDUDE
{
    public class OneLINKDemographicsProcessor : ParentSystemDemographics
    {
        private ReflectionInterface RI;

        public OneLINKDemographicsProcessor() : base()
        {
            RI = SessionInteractionComponents.RI;
        }

        //populate OneLINK demographic information
        public override void Populate(Borrower borrower)
        {
            //access LP22
            RI.Stup(DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
            RI.FastPath($"LP22I{borrower.SSN}");
            //gather the demographic information
            borrower.UpdatedDemographics.SSN = RI.GetText(3, 23, 9); //SSN
            borrower.UpdatedDemographics.CLAccNum = RI.GetText(3, 60, 12).Replace(" ", ""); //account number
            borrower.UpdatedDemographics.Name = RI.GetText(4, 44, 12) + " " + RI.GetText(4, 60, 1) + " " + RI.GetText(4, 5, 35); //name
            borrower.UpdatedDemographics.FirstName = RI.GetText(4, 44, 12);
            borrower.UpdatedDemographics.MI = RI.GetText(4, 60, 1);
            borrower.UpdatedDemographics.LastName = RI.GetText(4, 5, 35);
            borrower.UpdatedDemographics.DOB = RI.GetText(4, 72, 8); //Date of Birth
            borrower.UpdatedDemographics.DOB = borrower.UpdatedDemographics.DOB.Insert(4, "/");
            borrower.UpdatedDemographics.DOB = borrower.UpdatedDemographics.DOB.Insert(2, "/");
        }

        public override void Update(string source, UpdateDemoCompassIndicators systemsUpdateIndicators, bool isSchool, Demographics demosForUpdating, DemographicVerifications demographicVerifications, MDBorrowerDemographics altAddress)
        {
            throw new NotImplementedException("The update process of OneLINK demographics has not been coded.");
        }
    }
}
