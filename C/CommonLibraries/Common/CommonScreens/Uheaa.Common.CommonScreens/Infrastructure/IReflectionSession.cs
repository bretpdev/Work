using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.CommonScreens
{
    public interface IReflectionSession
    {
        void PutText(int x, int y, string text);
        void BlankField(int x, int y);
        string GetText(int x, int y, int length);
        void Hit(Key key);
        void FastPath(string fastPath);
    }
}
