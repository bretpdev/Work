using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public static class FaqLinker
    {
        public static Action<Form> ShowFaq { get; set; }
        public static Action<Form> ShowTraining { get; set; }
    }
}
