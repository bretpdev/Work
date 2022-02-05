using Common;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace CopyingProperties.SameName
{
    public class ProcessSameName
    {
        private List<Person> People { get; set; }
        private List<Company> Companies { get; set; }

        public void Start()
        {
            People = new List<Person>();
            Companies = new List<Company>();

            WriteLine("Copying properties with the same name.");
            WriteLine("");

            LoadPerson();
            LoadCompany();
            CopyPeople();
            WriteOutPeople();
        }

        private void LoadPerson()
        {
            People.Add(new Person("John", "Smith", Program.Sex.M, 25, 74250, Program.JobType.Software));
            People.Add(new Person("Jacob", "Johnson", Program.Sex.M, 19, 26500, Program.JobType.Groceries));
            People.Add(new Person("Erin", "Porter", Program.Sex.F, 32, 48000, Program.JobType.Software));
            People.Add(new Person("Paul", "Revere", Program.Sex.M, 56, 110986, Program.JobType.Groceries));
            People.Add(new Person("Jessica", "Johnson", Program.Sex.F, 48, 164535, Program.JobType.Groceries));
            People.Add(new Person("Jordan", "Allred", Program.Sex.M, 27, 73238, Program.JobType.Software));
            People.Add(new Person("Randy", "Butterfield", Program.Sex.O, 45, 54893, Program.JobType.Groceries));
            People.Add(new Person("Kevin", "Kindred", Program.Sex.M, 35, 64000, Program.JobType.Groceries));
        }

        private void LoadCompany()
        {
            Companies.Add(new Company("Google", "456 Main St", "Salt Lake City", "UT", "12345", Program.JobType.Software));
            Companies.Add(new Company("Harmons", "1450 E 200 S", "Salt Lake City", "UT", "54321", Program.JobType.Groceries));
        }

        private void CopyPeople()
        {
            int g = 0;
            int s = 0;
            foreach (var person in People)
            {
                Employee e = new Employee();

                PropertyCopier<Person, Employee>.CopyByName(person, e);
                if (person.Job == Program.JobType.Groceries)
                {
                    e.EmployeeNumber = ++g;
                    Company company = Companies.Where(p => p.Job == Program.JobType.Groceries).SingleOrDefault();
                    company.Employees.Add(e);
                }
                else
                {
                    e.EmployeeNumber = ++s;
                    Company company = Companies.Where(p => p.Job == Program.JobType.Software).SingleOrDefault();
                    company.Employees.Add(e);
                }
            }
        }

        private void WriteOutPeople()
        {
            WriteLine("All People");
            foreach (Person p in People)
                WriteLine($"Name: {p.FirstName} {p.LastName}, Gender: {p.Gender}, Age: {p.Age}, Salary: ${p.Salary:0,00}");

            WriteLine("");

            WriteLine("Companies");
            foreach (var company in Companies)
                foreach (var employee in company.Employees)
                    WriteLine($"Company: {company.CompanyName}, Employee: {employee.EmployeeNumber} - {employee.FirstName} {employee.LastName}");
        }
    }
}