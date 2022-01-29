using Uheaa.Common.Scripts;

namespace I1I2CHREV
{
    public class Demographics
    {
        public string Ssn { get; set; }
        public string Name { get; set; }
        public string AddressStatus { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string Email { get; set; }
        public SchoolInfo SchoolInfo { get; set; }
        public StudentInfo StudentInfo { get; set; }

        public static Demographics FromTS24(ReflectionInterface ri)
        {
            Demographics demo = new Demographics();

            LoadDemographics(ri, demo);
            demo.SchoolInfo = SchoolInfo.Load(ri, demo.Ssn);
            if (demo.SchoolInfo.AllLoansArePlus)
                demo.StudentInfo = StudentInfo.Load(ri, demo.Ssn);

            return demo;
        }
        private static void LoadDemographics(ReflectionInterface ri, Demographics demo)
        {
            demo.Ssn = ri.GetText(4, 16, 3) + ri.GetText(4, 20, 2) + ri.GetText(4, 23, 4);
            demo.Name = ri.GetText(4, 37, 40);
            demo.Address1 = ri.GetText(8, 13, 33);
            demo.Address2 = ri.GetText(9, 13, 33);
            demo.City = ri.GetText(11, 13, 20);
            //if the country is not blank, get the foreign state (if it is blank, leave the state blank) and foreign country
            if (!string.IsNullOrEmpty(ri.GetText(9, 55, 1)))
            {
                if (!string.IsNullOrEmpty(ri.GetText(8, 61, 1)))
                    demo.State = ri.GetText(8, 61, 15);
                demo.Country = ri.GetText(9, 55, 25);
            }
            else//if the country is blank, get the domestic state and leave the country blank
                demo.State = ri.GetText(11, 36, 2);
            demo.AddressStatus = ri.GetText(7, 36, 10);
            demo.Zip = ri.GetText(11, 40, 17);
            demo.Phone = "(" + ri.GetText(13, 21, 3) + ") " + ri.GetText(13, 25, 8);
            //Get the alternate phone and e-mail if they're not blank.
            if (!ri.CheckForText(15, 21, " "))
                demo.AltPhone = "(" + ri.GetText(15, 21, 3) + ") " + ri.GetText(15, 25, 8);
            if (!ri.CheckForText(17, 10, " "))
                demo.Email = ri.GetText(17, 10, 56);

        }
    }
}