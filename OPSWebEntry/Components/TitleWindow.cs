using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OPSWebEntry
{
    public class TitleWindow : Window
    {
        private string title;
        public new string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                base.Title = "OPS Web Entry " + title + (ModeHelper.IsTest ? "- (TEST)" : "");
            }
        }
    }
}
