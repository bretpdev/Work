namespace TEXTCOORD
{
    public class FileData
    {
        public string FriendlyTo { get; set; }
        public string ContentType { get; set; }
        public string FirstName { get; set; }

        public override string ToString()
        {
            return $"{FriendlyTo},{ContentType},{FirstName}";
        }
    }
}