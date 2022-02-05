
namespace Uheaa.Common.Scripts
{
    public struct Screen
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string Text { get; set; }
        public Screen(int row, int column, string text)
            : this()
        {
            Row = row;
            Column = column;
            Text = text;
        }

        public static Screen FirstScreen = new Screen(16, 2, "LOGON ==>");
        public static Screen LoginScreen = new Screen(20, 8, "USERID==>");
        public static Screen TestRegion = new Screen(1, 20, "=== PLEASE SELECT ONE OF THE FOLLOWING ===");
        public static Screen EmptyScreen = new Screen(1, 80, new string(' ', 80));
    }

}
