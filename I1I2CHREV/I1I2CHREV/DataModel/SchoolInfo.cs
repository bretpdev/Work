using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace I1I2CHREV
{
    public class SchoolDemographics
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class School
    {
        public string SchoolCode { get; set; }
        public string LenderCode { get; set; }
        public bool IsTilp { get; set; }
        public bool IsPlus { get; set; }
        public bool StopPursuit { get; set; }
        public override int GetHashCode() { return SchoolCode.GetHashCode(); }
        public SchoolDemographics GetDemographics(ReflectionInterface ri)
        {
            if (SchoolCode.IsNullOrEmpty())
                return null;
            SchoolDemographics demo = new SchoolDemographics();
            ri.FastPath("LPSCI" + SchoolCode);
            //select dept 112 if it is displayed
            if (ri.CheckForText(21, 3, "SEL"))
            {
                //find the row for dept 112
                for (int row = 7; !ri.CheckForText(22, 3, "46004"); row++)
                {
                    //go to the next page if the bottom of the page is reached
                    if (row == 19)
                    {
                        ri.Hit(ReflectionInterface.Key.F8);
                        row = 6;
                        continue;
                    }
                    if (ri.GetText(row, 7, 3) == "112")
                    {
                        ri.PutText(21, 13, ri.GetText(row, 2, 2), ReflectionInterface.Key.Enter, true);
                        demo.Name = ri.GetText(5, 21, 40);
                        demo.Address = ri.GetText(8, 21, 40).Trim('_');
                        demo.Address2 = ri.GetDisplayTest(9, 21, 40).Trim('_');
                        demo.Address3 = ri.GetText(10, 21, 30);
                        demo.City = ri.GetText(11, 21, 30);
                        demo.State = ri.GetText(11, 59, 2);
                        demo.Zip = ri.GetText(11, 66, 5);
                        if (!string.IsNullOrEmpty(ri.GetText(11, 71, 1)))
                            demo.Zip += "-" + ri.GetText(11, 71, 4);
                        break;
                    }
                }
            }
            else
            {
                ri.FastPath("TX3Z/ITX0Y;");
                ri.PutText(8, 27, SchoolCode, ReflectionInterface.Key.Enter);
                ri.PutText(22, 18, "01", ReflectionInterface.Key.Enter);
                demo.Name = ri.GetText(6, 19, 40);
                demo.Address = ri.GetText(11, 23, 40).Trim('_');
                demo.Address2 = ri.GetText(12, 23, 40).Trim('_');
                demo.City = ri.GetText(14, 13, 23);
                demo.State = ri.GetText(14, 53, 2);
                if (string.IsNullOrEmpty(demo.State))
                    demo.State = ri.GetText(15, 21, 13);
                demo.Zip = ri.GetText(14, 69, 5);
            }
            return demo;
        }
    }

    public class SchoolInfo
    {
        public HashSet<School> Schools { get; private set; }
        public bool AllLoansArePlus { get { return Schools.All(o => o.IsPlus) && Schools.Any(); } }
        public bool AtLeastOneLoanIsTilp { get { return Schools.Any(o => o.IsTilp); } }
        public bool AllLoansAreStopPursuit { get { return Schools.All(o => o.StopPursuit); } }
        public bool SkipOneLink { get { return Schools.All(o => o.LenderCode != "828476"); } }
        public static SchoolInfo Load(ReflectionInterface ri, string ssn)
        {
            SchoolInfo info = new SchoolInfo();
            HashSet<School> schools = new HashSet<School>();
            ri.FastPath("TX3Z/ITS26" + ssn);
            if (ri.ScreenCode == "TSX28")
            {
                PageHelper.Iterate(ri, (row) =>
                {
                    School school = new School();
                    school.IsTilp = ri.CheckForText(row, 19, "TILP");
                    ri.PutText(21, 12, ri.GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);
                    if (ri.GetText(11, 12, 10).ToDouble() > 0)
                    {
                        school.StopPursuit = ri.CheckForText(3, 10, "CNSLD-STOP-PURSUIT");
                        school.SchoolCode = ri.GetText(13, 18, 8);
                        school.IsPlus = ri.CheckForText(6, 66, "PLUS");
                        school.LenderCode = ri.GetText(7, 48, 8);
                        if (!schools.Any(o => o.SchoolCode == school.SchoolCode) && !string.IsNullOrEmpty(school.SchoolCode))
                            schools.Add(school);
                        if (school.LenderCode != "828476")
                        {
                            ri.Hit(ReflectionInterface.Key.Enter);  //enter TSX2A from TSX29
                            string originalSchoolCode = ri.GetText(9, 40, 8);
                            if (originalSchoolCode != school.SchoolCode)
                            {
                                School original = new School();
                                original.SchoolCode = originalSchoolCode;
                                original.LenderCode = ri.GetText(10, 40, 8);
                                if (!schools.Any(o => o.SchoolCode == originalSchoolCode))
                                    schools.Add(original);
                            }
                            ri.Hit(ReflectionInterface.Key.F12);  //return to TSX29
                        }
                    }
                    ri.Hit(ReflectionInterface.Key.F12); //back to the list
                });
            }
            else
            {
                School school = new School();
                string loan = ri.GetText(6, 66, 4);
                school.IsPlus = loan == "PLUS";
                school.IsTilp = loan == "TILP";
                school.StopPursuit = ri.CheckForText(3, 10, "CNSLD-STOP-PURSUIT");
                school.SchoolCode = ri.GetText(13, 18, 8);
                school.LenderCode = ri.GetText(7, 48, 8);
                schools.Add(school);
            }
            info.Schools = schools;
            return info;
        }
    }
}