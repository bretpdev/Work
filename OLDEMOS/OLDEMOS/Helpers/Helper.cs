using Uheaa.Common.Scripts;

namespace OLDEMOS
{
    public static class Helper
    {
        public static LoginHelper Login { get { return LoginHelper.Instance; } }
        public static ReflectionHelper RH { get { return ReflectionHelper.Instance; } }
        public static ReflectionInterface RI { get { return RH.CurrentSession; } }
        public static UIHelper UI { get { return UIHelper.Instance; } }
        public static DataAccess DA { get; set; }

        public static void Instantiate()
        {
            RH.Instantiate();
        }
    }
}