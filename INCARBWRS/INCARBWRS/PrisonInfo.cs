using System.Linq;
namespace INCARBWRS
{
    public class PrisonInfo
    {
        public string SSN { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string Phone { get; set; }
        public string InmateNumber { get; set; }
        public string AnticipatedReleaseDate { get; set; }
        public string FollowUpDate { get; set; }
        public ContactSource Contact { get; set; }
        public string OtherInfo { get; set; }

        /// <summary>
        /// Formats the PrisonInfo properties to be used in system comments.
        /// </summary>
        public string CommentText
        {
            get
            {
                const string FORMAT = "{0};{1} {2} {3} {4};{5};{6};ARD:{7};FUD:{8};{9};{10}";
                string[] arguments = { Name, Address, City, State, ZIP, Phone, InmateNumber, AnticipatedReleaseDate, FollowUpDate, Contact.Source, OtherInfo };
                return string.Format(FORMAT, arguments);
            }
        }

        /// <summary>
        /// Indicates whether enough info was entered to update the system demographics.
        /// </summary>
        public bool IsComplete
        {
            get
            {
                var emptyFields = new string[] { SSN, Name, Address, City, State, ZIP, Phone, FollowUpDate };
                var hasEmptyFields = emptyFields.Any(o => string.IsNullOrEmpty(o));
                return !hasEmptyFields;
            }
        }
    }
}