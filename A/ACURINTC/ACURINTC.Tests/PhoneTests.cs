//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Uheaa.Common.Scripts;
//using Xunit;

//namespace ACURINTC.Tests
//{
//    public class PhoneTests
//    {
//        [Fact]
//        public void ExistingForeignFieldsAreBlankedOut()
//        {
//            var mockRi = new RIWithForeignFields();
//            PendingDemos dummyTask = new PendingDemos() { PendingVerificationDate = DateTime.Now };

//            PhoneHelper ph = new PhoneHelper(mockRi, dummyTask, null, "8889993333", PhoneType.Home);
//            Assert.True(ph.UpdatePhoneNumber(new SystemCode()));
//        }

//        private class RIWithForeignFields : IReflectionInterface
//        {
//            bool firstFieldBlanked = false;
//            bool secondFieldBlanked = false;
//            bool thirdFieldBlanked = false;
//            public string Message
//            {
//                get
//                {
//                    return ""; //dummy method
//                }
//            }

//            public string MessageCode
//            {
//                get
//                {
//                    if (firstFieldBlanked && secondFieldBlanked && thirdFieldBlanked)
//                        return "01097";
//                    else
//                        return "ERROR";
//                }
//            }

//            public string ScreenCode
//            {
//                get
//                {
//                    return ""; //dummy method
//                }
//            }

//            public bool CheckForText(int row, int column, params string[] text)
//            {
//                return false;
//            }

//            public void CloseSession()
//            {
//                throw new NotImplementedException();
//            }

//            public void FastPath(string input)
//            {
//                //dummy method
//            }

//            public string GetText(int row, int col, int length)
//            {
//                if (row == 18 && col == 15 && length == 3 && !firstFieldBlanked)
//                    return "444";
//                if (row == 18 && col == 24 && length == 5 && !secondFieldBlanked)
//                    return "5555";
//                if (row == 18 && col == 36 && length == 10 && !thirdFieldBlanked)
//                    return "66666";
//                return "";
//            }

//            public bool Hit(ReflectionInterface.Key keyToHit)
//            {
//                return true; //dummy method
//            }

//            public bool Hit(ReflectionInterface.Key keyToHit, int numberOfTimes)
//            {
//                return true; //dummy method
//            }

//            public bool Login(string userId, string password, Uheaa.Common.DataAccess.DataAccessHelper.Region region)
//            {
//                throw new NotImplementedException();
//            }

//            public void PutText(int row, int column, string text)
//            {
//                //dummy method
//            }

//            public void PutText(int row, int column, string text, ReflectionInterface.Key keyToHit)
//            {
//                //dummy method
//            }

//            public void PutText(int row, int column, string text, bool blankFieldFirst)
//            {
//                if (row == 18 && blankFieldFirst)
//                {
//                    if (column == 15)
//                        firstFieldBlanked = true;
//                    if (column == 24)
//                        secondFieldBlanked = true;
//                    if (column == 36)
//                        thirdFieldBlanked = true;
//                }
//            }

//            public void PutText(int row, int column, string text, ReflectionInterface.Key keyToHit, bool blankFieldFirst)
//            {
//                //dummy method
//            }
//        }
//    }
//}
