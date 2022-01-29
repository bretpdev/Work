namespace CopyingProperties.ExtensionAttribute
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Program.Sex Gender { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
        public Program.JobType Job { get; set; }

        public Person() { }

        public Person(string fName, string lName, Program.Sex gender, int age, double salary, Program.JobType job)
        {
            FirstName = fName;
            LastName = lName;
            Gender = gender;
            Age = age;
            Salary = salary;
            Job = job;
        }
    }
}