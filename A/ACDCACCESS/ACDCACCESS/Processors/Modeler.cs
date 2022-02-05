using System.Collections.Generic;

namespace ACDCAccess
{
    class Modeler
    {
		public static void DoModeling(bool testMode, IEnumerable<ModelAfterKey> keys, int userToBeChanged, int userPerformingAction)
        {
			DataAccess dataAccess = new DataAccess(testMode);
            foreach (ModelAfterKey mak in keys)
	        {
                if (mak.ActionToBeTaken == DataAccess.REMOVE_ACTION_STRING)
                 {
					 dataAccess.RemoveUserAccess(mak.CalculatedID, userPerformingAction);
                 }
                 else
                 {
                     UserAccessKey uak = new UserAccessKey();
                     uak.Name = mak.UserKey;
                     uak.Application = mak.Application;
                     uak.BusinessUnit = mak.BusinessUnit;
                     uak.UserID = userToBeChanged;
                     try
                     {
						 dataAccess.AddUserAccess(uak, userPerformingAction);
                     }
                     catch (UserKeyAssignmentAlreadyExistsException)
                     {
                         //do nothing just don't choke
                     }
                 }//if/else
	        }//foreach
        }//DoModeling()
    }//class
}//namespace
