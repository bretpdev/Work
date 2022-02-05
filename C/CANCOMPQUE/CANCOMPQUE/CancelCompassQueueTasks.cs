using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using System.Data.SqlClient;

namespace CANCOMPQUE
{
    public class CancelCompassQueueTasks : BatchScript
    {

        public CancelCompassQueueTasks(ReflectionInterface ri)
            : base(ri, "CANCOMPQUE", "ERR_BU35", "EOJ_BU35", new List<string>(), DataAccessHelper.Region.Uheaa)
        {
        }

        public override void Main()
        {
            string startupMessage = "This script is designed to cancel queue tasks and, if desired, error out activity comments.  If you wish to proceed click OK else click cancel.";
            if (MessageBox.Show(startupMessage, ScriptId, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK) { EndDllScript(); }

            if (IsAuthUser())
            {
                CancelData data = new CancelData();

                frmCancelQueue cancel = new frmCancelQueue(data);
                if (cancel.ShowDialog() == DialogResult.Cancel) { EndDllScript(); }

                CancelQueues(data);
            }
            else
            {
                MessageBox.Show("Your user ID is not allowed to access this script", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                EndDllScript();
            }
            MessageBox.Show("Process Complete", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Gets list of authorized users from database and compares them to the user logged in
        /// </summary>
        /// <returns></returns>
        private bool IsAuthUser()
        {
            using (SqlCommand comm = DataAccessHelper.GetCommand("spUserHasAccessKey", DataAccessHelper.Database.Csys))
            {
                comm.Parameters.AddWithValue("WindowsUserName", Environment.UserName);
                comm.Parameters.AddWithValue("UserKey", "Cancel Compass Queue");
                return (bool)comm.ExecuteScalar();
            }
        }

        /// <summary>
        /// Checks the data that was input and processes the Queue's
        /// </summary>
        /// <param name="data"></param> 
        private void CancelQueues(CancelData data)
        {
            //If there is no message, delete all queue tasks
            if (data.Message == null || data.Message == string.Empty) { TX6XCancellAllTasks(data); }
            else { TX6XCancelErrorMsgTasks(data.Queue, data.SubQueue, data.Message); }
        }

        /// <summary>
        /// Loops through all tasks in the queue and cancels them
        /// </summary>
        /// <param name="data">Data Object</param>
        private void TX6XCancellAllTasks(CancelData data)
        {
            FastPath("TX3Z/ITX6X" + data.Queue + ";" + data.SubQueue);
            if (CheckForText(23, 2, "80039"))
            {
                MessageBox.Show("Subqueue Not Found", "Wrong Subqueue", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                while (!CheckForText(23, 2, "01020"))
                {
                    PutText(21, 18, "01", ReflectionInterface.Key.F2);
                    data.SSN = GetText(12, 2, 9);
                    PutText(8, 19, "X");
                    PutText(9, 19, "CANCL", ReflectionInterface.Key.Enter);
                    //If there is an error, blank out the action response
                    if (CheckForText(23, 2, "01644")) { PutText(9, 19, "     ", ReflectionInterface.Key.Enter); }
                    if (data.ARC != null && data.ARC != string.Empty) { ErrorOutActivityComments(data); }
                    FastPath("TX3Z/ITX6X" + data.Queue + ";" + data.SubQueue);
                }
            }
        }

        /// <summary>
        /// For each borrower in the queue, this finds the given arc and deletes it.
        /// </summary>
        /// <param name="data"></param>
        private void ErrorOutActivityComments(CancelData data)
        {
            string ERROR_FILE = @"T:\Error Log - Erroring Activity Comments";
            switch (ScriptHelper.DeleteActivityComments(RI, data.SSN, data.ARC, data.FromDate, data.ToDate))
            {
                case ScriptHelper.DeleteActivityCommentsResults.ArcNotFoundOrInactive:
                    string arcMessage = string.Format("{0} is not valid or is inactive.  Check the ARC you entered and run the script again.");
                    MessageBox.Show(arcMessage, ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    EndDllScript();
                    break;
                case ScriptHelper.DeleteActivityCommentsResults.SsnIsNotABorrower:
                    using (StreamWriter errorWriter = new StreamWriter(ERROR_FILE, true))
                    {
                        errorWriter.WriteLine(data.SSN);
                    }
                    break;
            }//switch
        }

        /// <summary>
        /// Cancels all tasks in given queue with a specific message
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="subQueue"></param>
        /// <param name="message"></param>
        private void TX6XCancelErrorMsgTasks(string queue, string subQueue, string message)
        {
            bool isLocated = true;
            while (isLocated)
            {
                FastPath("TX3Z/ITX6X" + queue + ";" + subQueue);
                if (CheckForText(23, 2, "01020"))
                {
                    MessageBox.Show("No records found for this queue", "No Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isLocated = false;
                }
                else
                {
                    if (LocatedValidTask(message))
                    {
                        PutText(8, 19, "X");
                        PutText(9, 19, "CANCL", ReflectionInterface.Key.Enter);
                        //If there is an error, put in a blank instead
                        if (CheckForText(23, 2, "01644")) { PutText(9, 19, "     ", ReflectionInterface.Key.Enter); }
                    }
                    else { isLocated = false; }
                }
            }
        }

        /// <summary>
        /// Finds the tasks
        /// </summary>
        /// <param name="message">The message in the task</param>
        /// <returns>Returns true if a task was found with the given message</returns>
        private bool LocatedValidTask(string message)
        {
            string row = string.Empty;
            while (!CheckForText(22, 3, "46004"))
            {
                row = FindErrorMessageOnTX6X(message);
                if (row != string.Empty)
                {
                    PutText(21, 18, GetText(Convert.ToInt32(row) - 1, 4, 1), ReflectionInterface.Key.F2);
                    return true;
                }
                else
                {
                    Hit(ReflectionInterface.Key.F8);
                    if (CheckForText(23, 2, "90007")) { return false; }
                    else if (CheckForText(23, 2, "01033")) { Hit(ReflectionInterface.Key.Enter); }
                }
            }
            return false;
        }

        /// <summary>
        /// Finds the row number for the task using the specified message
        /// </summary>
        /// <param name="message">The message in the task</param>
        /// <returns>Row number for found task</returns>
        private string FindErrorMessageOnTX6X(string message)
        {
            if (CheckForText(9, 2, message)) { return "9"; }
            else if (CheckForText(12, 2, message)) { return "12"; }
            else if (CheckForText(15, 2, message)) { return "15"; }
            else if (CheckForText(18, 2, message)) { return "18"; }
            else { return string.Empty; }
        }
    }//Class
}//Namespace