using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BATEXTCNSL
{
    class ExitCounselingRecord
    {
        [CsvPos(0)]
        public string FirstName { get; set; }
        [CsvPos(1)]
        public string LastName { get; set; }
        [CsvPos(2)]
        public string MiddleInitial { get; set; }
        [CsvPos(3)]
        public string Ssn { get; set; }
        [CsvPos(4)]
        public string Address { get; set; }
        [CsvPos(5)]
        public string City { get; set; }
        [CsvPos(6)]
        public string State { get; set; }
        [CsvPos(7)]
        public string Zip { get; set; }
        [CsvPos(8)]
        public string ZipPlus { get; set; }
        [CsvPos(9)]
        public string Phone { get; set; }
        [CsvPos(10)]
        public string Email { get; set; }
        [CsvPos(11)]
        public string SchoolCodeAndBranch { get; set; }
        [CsvPos(12)]
        public DateTime DateOfBirth { get; set; }
        [CsvPos(13)]
        public string DriversLicense { get; set; }
        [CsvPos(14)]
        public string DriversLicenseState { get; set; }
        [CsvPos(24)]
        public string Employer { get; set; }
        [CsvPos(25)]
        public string EmployerAddress { get; set; }
        [CsvPos(26)]
        public string EmployerCity { get; set; }
        [CsvPos(27)]
        public string EmployerState { get; set; }
        [CsvPos(28)]
        public string EmployerZip { get; set; }
        [CsvPos(29)]
        public string EmployerZipPlus { get; set; }
        [CsvPos(30)]
        public string EmployerPhone { get; set; }
        [CsvPos(31)]
        public string KinFirstName { get; set; }
        [CsvPos(32)]
        public string KinLastName { get; set; }
        [CsvPos(33)]
        public string KindMiddleInitial { get; set; }
        [CsvPos(34)]
        public string KinAddress { get; set; }
        [CsvPos(35)]
        public string KinCity { get; set; }
        [CsvPos(36)]
        public string KinState { get; set; }
        [CsvPos(37)]
        public string KinZip { get; set; }
        [CsvPos(38)]
        public string KinZipPlus { get; set; }
        [CsvPos(39)]
        public string KinPhone { get; set; }
        [CsvPos(40)]
        public string KinRelationship { get; set; }
        [CsvPos(41)]
        public string Ref1FirstName { get; set; }
        [CsvPos(42)]
        public string Ref1LastName { get; set; }
        [CsvPos(43)]
        public string Ref1MiddleInitial { get; set; }
        [CsvPos(44)]
        public string Ref1Address { get; set; }
        [CsvPos(45)]
        public string Ref1City { get; set; }
        [CsvPos(46)]
        public string Ref1State { get; set; }
        [CsvPos(47)]
        public string Ref1Zip { get; set; }
        [CsvPos(48)]
        public string Ref1ZipPlus { get; set; }
        [CsvPos(49)]
        public string Ref1Phone { get; set; }
        [CsvPos(50)]
        public string Ref1Employer { get; set; }
        [CsvPos(51)]
        public string Ref1Relationship { get; set; }
        [CsvPos(52)]
        public string Ref2FirstName { get; set; }
        [CsvPos(53)]
        public string Ref2LastName { get; set; }
        [CsvPos(54)]
        public string Ref2MiddleInitial { get; set; }
        [CsvPos(55)]
        public string Ref2Address { get; set; }
        [CsvPos(56)]
        public string Ref2City { get; set; }
        [CsvPos(57)]
        public string Ref2State { get; set; }
        [CsvPos(58)]
        public string Ref2Zip { get; set; }
        [CsvPos(59)]
        public string Ref2ZipPlus { get; set; }
        [CsvPos(60)]
        public string Ref2Phone { get; set; }
        [CsvPos(61)]
        public string Ref2Employer { get; set; }
        [CsvPos(62)]
        public string Ref2Relationship { get; set; }
        [CsvPos(63)]
        public string Guarantor { get; set; }
        [CsvPos(64)]
        public string Lender { get; set; }
        [CsvPos(65)]
        public DateTime CreationTimestamp { get; set; }

        public static string GenerateHeader()
        {
            int headerLength = 73;
            var props = typeof(ExitCounselingRecord).GetProperties();
            List<string> headers = Enumerable.Range(0, headerLength).Select(o => "").ToList();
            foreach (var prop in props)
            {
                var attr = Attribute.GetCustomAttribute(prop, typeof(CsvPosAttribute)) as CsvPosAttribute;
                if (attr != null)
                {
                    while (headers.Count <= attr.Pos)
                    {
                        headers.Add("");
                        headerLength = attr.Pos + 1;
                    }
                    headers[attr.Pos] = prop.Name;
                }
            }
            return string.Join(",", headers);
        }
    }

    class CsvPosAttribute : Attribute
    {
        public int Pos { get; set; }
        public CsvPosAttribute(int pos)
        {
            this.Pos = pos;
        }
    }
}
