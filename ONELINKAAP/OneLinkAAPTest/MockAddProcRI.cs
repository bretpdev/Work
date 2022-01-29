using System;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;



namespace OneLinkAAPTEST
{
    public class MockArcAddProcRI : IReflectionInterface
    {
        public string Message { get; set; }

        public string MessageCode { get; set; }

        public string ScreenCode { get; set; }

        public bool CheckForText(int row, int column, params string[] text)
        {
            throw new NotImplementedException();  //code shouldn't call this
        }
        public void FastPath(string input)
        {
            // dummy method
        }

        public bool Hit(ReflectionInterface.Key keyToHit)
        {
            return true; // dummy method
        }

        public void PutText(int row, int column, string text)
        {
            // dummy method
        }

        public void PutText(int row, int column, string text, ReflectionInterface.Key keyToHit)
        {
            // dummy method
        }

        public void PutText(int row, int column, string text, bool blankFieldFirst)
        {
            // dummy method
        }

        public void PutText(int row, int column, string text, ReflectionInterface.Key keyToHit, bool blankFieldFirst)
        {
            // dummy method
        }

        public bool Hit(ReflectionInterface.Key key, int eax)
        {
            // dummy method
            return true;
        }

        public string GetText(int int1, int int2, int int3)
        {
            // dummy method
            return @"string";
        }

        public bool Login(string eax, string xae, DataAccessHelper.Region region)
        {
            // dummy method
            return false;
        }
        public void CloseSession()
        {
            // dummy method
        }

        public bool Login(string userId, string password, DataAccessHelper.Region region, bool useVuk3)
        {
            throw new NotImplementedException();
        }
    }
}
