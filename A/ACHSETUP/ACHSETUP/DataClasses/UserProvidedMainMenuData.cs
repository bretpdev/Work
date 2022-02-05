namespace ACHSETUP
{
    public class UserProvidedMainMenuData
    {
        public enum UserSelectedACHAction
        {
            Add,
            Change,
            Remove,
            Suspend,
            MissingInformation,
            None
        }

        public UserSelectedACHAction UserSelectedACHOption { get; set; }
        public string FirstName { get; set; }
        public string SSN { get; set; }

        public UserProvidedMainMenuData()
        {
            UserSelectedACHOption = UserSelectedACHAction.None;
            FirstName = string.Empty;
            SSN = string.Empty;
        }
    }
}