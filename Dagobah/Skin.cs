using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace Dagobah
{
    class Skin
    {
        private const string DEFAULT_SKIN = "Machine";
        private const string REGISTRY_PATH = @"Software\Uheaa\Dagobah";
        private const string REGISTRY_KEY = "SelectedSkin";
        private const string SKINS_DIRECTORY = @"X:\PADU\DagobahSkins\";

        private static string _name;
        public static string Name
        {
            get
            {
                //The first time the Name property is accessed, it will be null, so set its value.
                if (string.IsNullOrEmpty(_name))
                {
                    //Get the selected skin from the registry, or set it to the default value if it's not in the registry.
                    try
                    {
                        _name = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH).GetValue(REGISTRY_KEY).ToString();
                    }
                    catch
                    {
                        _name = DEFAULT_SKIN;
                    }
                }
                return _name;
            }
            set
            {
                //Make sure the newly selected skin exists.
                Debug.Assert(Directory.Exists(SKINS_DIRECTORY + value), string.Format("The {0} skin was not found.", value));

                //Update both the local object and the registry when the selected skin changes.
                if (value != _name)
                {
                    _name = value;
                    Registry.CurrentUser.CreateSubKey(REGISTRY_PATH).SetValue(REGISTRY_KEY, _name);
                }
            }
        }

        public static string SkinsDirectory
        {
            get { return SKINS_DIRECTORY; }
        }

        public static string BottomPanelImage { get { return string.Format(@"{0}{1}\BottomPanel.gif", SKINS_DIRECTORY, Name); } }
        public static string CenterPanelImage { get { return string.Format(@"{0}{1}\CenterPanel.gif", SKINS_DIRECTORY, Name); } }
        public static string DagobahImage { get { return string.Format(@"{0}{1}\Dagobah.gif", SKINS_DIRECTORY, Name); } }
        public static string LeftPanelImage { get { return string.Format(@"{0}{1}\LeftPanel.gif", SKINS_DIRECTORY, Name); } }
        public static string LettersImage { get { return string.Format(@"{0}{1}\Letters.gif", SKINS_DIRECTORY, Name); } }
        public static string RightPanelImage { get { return string.Format(@"{0}{1}\RightPanel.gif", SKINS_DIRECTORY, Name); } }
        public static string SasImage { get { return string.Format(@"{0}{1}\Sas.gif", SKINS_DIRECTORY, Name); } }
        public static string ScriptsImage { get { return string.Format(@"{0}{1}\Scripts.gif", SKINS_DIRECTORY, Name); } }
    }//class
}//namespace
