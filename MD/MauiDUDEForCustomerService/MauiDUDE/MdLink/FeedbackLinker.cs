using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public static class FeedbackLinker
    {
        public static Action<Form> BugReportAction { get; set; }
        public static Action<Form> FeatureRequestAction { get; set; }
    }
}
