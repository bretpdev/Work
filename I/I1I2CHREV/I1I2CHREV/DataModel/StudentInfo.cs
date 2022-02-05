using System;
using System.Collections.Generic;
using Uheaa.Common.Scripts;

namespace I1I2CHREV
{
    public class Student
    {
        public string SSN { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityStateZip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }

        public override int GetHashCode()
        {
            return SSN.GetHashCode();
        }
    }

    public class StudentInfo
    {
        public HashSet<Student> Students { get; private set; }
        public static StudentInfo Load(ReflectionInterface ri, string ssn)
        {
            StudentInfo info = new StudentInfo();
            HashSet<Student> students = new HashSet<Student>();
            ri.FastPath("TX3ZITS26" + ssn);
            Action addStudent = new Action(() =>
            {
                Student student = new Student();
                student.SSN = ri.GetText(7, 15, 9);
                students.Add(student);
            });
            if (ri.ScreenCode == "TSX28")
                PageHelper.Iterate(ri, (row) =>
                {
                    if (ri.CheckForText(row, 19, "PLUS") && !ri.CheckForText(row, 69, "CR") && !ri.CheckForText(row, 64, "0.00"))
                    {
                        ri.PutText(21, 12, ri.GetText(row, 2, 3), ReflectionInterface.Key.Enter, true);
                        addStudent();
                        ri.Hit(ReflectionInterface.Key.F12);
                    }
                });
            else
                addStudent();

            LoadStudentDemographics(ri, students);

            info.Students = students;
            return info;
        }

        private static void LoadStudentDemographics(ReflectionInterface ri, HashSet<Student> students)
        {
            foreach (Student student in students)
            {
                var demo = ri.GetDemographicsFromTx1j(student.SSN);
                student.Name = demo.FirstName + " " + demo.LastName;
                student.CityStateZip = demo.City + ", " + demo.State + ", " + demo.ZipCode;
                student.Address1 = demo.Address1;
                student.Address2 = demo.Address2;
                student.Country  = demo.Country;
                student.Phone = demo.PrimaryPhone;
            }
        }
    }
}