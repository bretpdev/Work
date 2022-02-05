namespace ActiveDirectoryGroups
{
    public class TilpUser
    {
        public string UserId { get; set; }
        public string LevelDesc { get; set; }
        public int AuthLevel { get; set; }
        public bool Valid { get; set; }
    }

    public class AuthList
    {
        public int AuthLevel { get; set; }
        public string LevelDesc { get; set; }
    }
}