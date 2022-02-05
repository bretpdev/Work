using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace Uheaa.Common.Scripts
{
    public interface IReflectionInterface
    {
        void FastPath(string input);
        bool Hit(Key keyToHit);
        bool Hit(Key keyToHit, int numberOfTimes);
        void PutText(int row, int column, string text);
        void PutText(int row, int column, string text, bool blankFieldFirst);
        void PutText(int row, int column, string text, Key keyToHit);
        void PutText(int row, int column, string text, Key keyToHit, bool blankFieldFirst);
        string GetText(int row, int col, int length);
        bool CheckForText(int row, int column, params string[] text);
        string MessageCode { get; }
        string Message { get; }
        string ScreenCode { get; }
        void CloseSession();
        bool Login(string userId, string password, DataAccessHelper.Region region);
        bool Login(string userId, string password, DataAccessHelper.Region region, bool useVuk3);
    }
}
