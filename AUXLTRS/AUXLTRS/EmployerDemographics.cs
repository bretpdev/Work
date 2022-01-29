using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace AUXLTRS
{
    public class EmployerDemographics
    {
        public int EmployerId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public EmployerDemographics(string employerId, ReflectionInterface ri)
        {
            ri.FastPath("LPEMI" + employerId);
            while (ri.CheckForText(1, 61, "DEPARTMENT SELECTION"))
            {
                string message = "More than one employer record was found on the system. Please select the correct one and hit <INSERT> to continue.";
                Dialog.Info.Ok(message, "Multiple Records Found");
                ri.PauseForInsert();
            }
            if (!ri.CheckForText(1, 57, "INSTITUTION DEMOGRAPHICS"))
                throw new DemographicException("The employer information wasn't found on the system.  Please contact Systems Support.");

            Name = ri.GetText(5, 21, 40);
            Address1 = ri.GetText(8, 21, 40);
            Address2 = ri.GetText(9, 21, 40);
            City = ri.GetText(11, 21, 30);
            State = ri.GetText(11, 59, 2);
            Zip = ri.GetText(11, 66, 9);
        }
    }
}