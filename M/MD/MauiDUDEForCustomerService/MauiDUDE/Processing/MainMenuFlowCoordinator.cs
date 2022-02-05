using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{

    public class MainMenuFlowCoordinator
    {
        /// <summary>
        /// Main starting point for DUDE processing.
        /// </summary>
        public static void Coordinate(BaseUserSelectedFlow flow)
        {
            string SSNOrAccountNumber = string.Empty; 
            try
            {
                flow.Process(); //do whatever the flow is coded to do
                SSNOrAccountNumber = string.Empty; //ssn can be wiped out
            }
            catch(BorrowerCanNotBeProcessedException ex)
            {
                //for some reason the borrower couldn't be processed
                SSNOrAccountNumber = flow.SSN;
            }
            catch(WipeOutCancelledException ex)
            {
                //who am i was cancelled
                SSNOrAccountNumber = string.Empty; //ssn can be wiped out
            }
        }
    }
}
