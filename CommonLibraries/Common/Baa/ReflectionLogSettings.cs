using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.Baa
{
    public class ReflectionLogSettings
    {
        public bool ScreenshotAfterPutText { get; set; }
        public bool ScreenshotAfterEnter { get; set; }
        public bool AutoSaveToTDrive { get; set; }
        public ReflectionLogSettings()
        {
            ScreenshotAfterEnter = true;
            ScreenshotAfterPutText = true;
            AutoSaveToTDrive = true;
        }
    }
}
