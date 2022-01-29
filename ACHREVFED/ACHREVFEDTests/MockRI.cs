using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ACHREVFEDTests
{
    public class MockRI : IReflectionInterface
    {
        public string Message { get; set; }

        public string MessageCode { get; set; }

        public string ScreenCode { get; set; }

        public bool CheckForText(int row, int column, params string[] text)
        {
            return true; //dummy method
        }

        public void FastPath(string input)
        {
            //dummy method
        }

        public void CloseSession()
        {
            //dummy method
        }

        public string GetText(int row, int col, int length)
        {
            return ""; //dummy method
        }

        public bool Hit(ReflectionInterface.Key keyToHit)
        {
            return true; //dummy method
        }

        public bool Hit(ReflectionInterface.Key keyToHit, int numberOfTimes)
        {
            return true; //dummy method
        }

        public void PutText(int row, int column, string text)
        {
            //dummy method
        }

        public void PutText(int row, int column, string text, ReflectionInterface.Key keyToHit)
        {
            //dummy method
        }

        public void PutText(int row, int column, string text, bool blankFieldFirst)
        {
            //dummy method
        }

        public void PutText(int row, int column, string text, ReflectionInterface.Key keyToHit, bool blankFieldFirst)
        {
            //dummy method
        }

        public bool Login(string userId, string password, DataAccessHelper.Region region)
        {
            return true; //dummy method
        }
    }
}
