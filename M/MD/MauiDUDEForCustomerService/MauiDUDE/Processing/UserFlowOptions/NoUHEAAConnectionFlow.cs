using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    class NoUHEAAConnectionFlow : BaseUserSelectedFlow
    {
        public override void Process()
        {
            GenericCallCategorization noUheaaConnection = new GenericCallCategorization(CallCategorizationEntry.CallCategory.NoUHEAAConnection);
            noUheaaConnection.ShowDialog();
        }
    }
}
