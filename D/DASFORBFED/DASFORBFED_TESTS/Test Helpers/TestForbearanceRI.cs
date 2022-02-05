﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;


namespace DASFORBFED_TESTS
{
    class TestForbearanceRI : IReflectionInterface
    {
        public string Message { get; set; }

        public string MessageCode { get; set; }

        public string ScreenCode { get; set; }

        public bool CheckForText(int row, int column, params string[] text)
        {
            return new Random().Next(0, 2) > 0;  //Test code uses this method only to determine whether or not to insert into a field in virtual session; in this Xunit implementation, doesn't matter what bool value is returned
        }

        public void CloseSession()
        {
            throw new NotImplementedException();
        }

        public void FastPath(string input)
        {
            //dummy method
        }

        public string GetText(int row, int col, int length)
        {
            throw new NotImplementedException();
        }

        public bool Hit(ReflectionInterface.Key keyToHit)
        {
            return true; //dummy method
        }

        public bool Hit(ReflectionInterface.Key keyToHit, int numberOfTimes)
        {
            throw new NotImplementedException();
        }

        public bool Login(string userId, string password, DataAccessHelper.Region region)
        {
            throw new NotImplementedException();
        }

        public bool Login(string userId, string password, DataAccessHelper.Region region, bool useVuk3)
        {
            throw new NotImplementedException();
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
    }
}