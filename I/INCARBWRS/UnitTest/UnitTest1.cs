using Microsoft.VisualStudio.TestTools.UnitTesting;
using INCARBWRS;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        PrisonInfo pi { get; set; }
        public ContactSource Contact { get; set; }

        [TestMethod]
        public void Test_IsComplete()
        {
            pi = new PrisonInfo();
            pi.SSN = "123456789";
            pi.Name = "Test Q. Data";
            pi.Address = "Rural Route 5";
            pi.City = "Gold Hill";
            pi.State = "UT";
            pi.ZIP = "01001";
            pi.Phone = "9065551212";
            pi.FollowUpDate = "01/01/2020";

            Assert.IsTrue(pi.IsComplete == true);
        }

        [TestMethod]
        public void Test_Comment()
        {
            string answer = @"Test Q. Data;Rural Route 5 Gold Hill UT 01001;9065551212;10101001;ARD:01/01/2021;FUD:11/01/2020;Brother;Other Info";
            //const string FORMAT = "{0};{1} {2} {3} {4};{5};{6};ARD:{7};FUD:{8};{9};{10}";
            pi = new PrisonInfo();
            pi.Contact = new ContactSource();
            pi.SSN = "123456789";
            pi.Name = "Test Q. Data";
            pi.Address = "Rural Route 5";
            pi.City = "Gold Hill";
            pi.State = "UT";
            pi.ZIP = "01001";
            pi.Phone = "9065551212";
            pi.FollowUpDate = "11/01/2020";
            pi.AnticipatedReleaseDate = "01/01/2021";
            pi.InmateNumber = "10101001";
            pi.OtherInfo = "Other Info";
            pi.Contact.Source = "Brother";
            pi.Contact.ContactType = "0x";
            pi.Contact.ActivityType = "A1";


            Assert.IsTrue(pi.CommentText == answer);

        }

    }
}
