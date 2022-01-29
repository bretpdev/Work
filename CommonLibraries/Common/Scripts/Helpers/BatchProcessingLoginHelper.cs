using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace Uheaa.Common.Scripts
{
    public static class BatchProcessingLoginHelper
    {
        private static List<InactiveIds> Ids { get; set; }

        static BatchProcessingLoginHelper()
        {
            Ids = new List<InactiveIds>();
        }


        /// <summary>
        /// Gets an ID from the database and tries to log in. If unsuccessful, it gets another ID and tries again until there are no more IDs to use.
        /// </summary>
        /// <param name="loginType">The region/type of login</param>
        /// <param name="scriptId">The script ID of the application using this method</param>
        /// <param name="inactiveIds">List of IDs not to use. Only send a list if there are IDs you do not want to use, otherwise is will auto populate with IDs that are not active.</param>
        /// <returns>BatchProcessingHelper if successfully logged into session otherwise return null</returns>
        public static BatchProcessingHelper Login(ProcessLogRun logRun, IReflectionInterface ri, string scriptId, string loginType, bool useVuk3 = false)
        {
            lock (Ids)
            {
                return LoginInternal(logRun, ri, scriptId, loginType, useVuk3);
            }
        }

        private static BatchProcessingHelper LoginInternal(ProcessLogRun logRun, IReflectionInterface ri, string scriptId, string loginType, bool useVuk3)
        {
            BatchProcessingHelper helper = BatchProcessingHelper.GetNextAvailableId(scriptId, loginType, Ids.Select(p => p.UserName).ToList());

            if (helper != null && helper.UserName != null)
            {
                //Moved the call to get the password to here so that the password is not stored on the heap
                //and is therefore not vulnerable to heap inspection
                if (ri.Login(helper.UserName, DataAccessHelper.ExecuteSingle<string>("spGetDecrpytedPassword", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("UserId", helper.UserName)), logRun.Region, useVuk3))
                    return helper;
                else if (ri.MessageCode.IsIn("ON09A", "ON009"))
                    return SecurityViolation(logRun, ri, scriptId, loginType, helper, ri.Message, useVuk3);
                else if (ri.CheckForText(12, 9, "=== CHOOSE ONE OF THE FOLLOWING THREE OPTIONS ===") || ri.MessageCode.IsIn("ON005", "ON002", "ON006", "ON001", "ON007"))
                    return Invalidate(logRun, ri, scriptId, loginType, helper, ri.Message.IsNullOrEmpty() ? ri.GetText(12, 9, 50) : ri.Message, useVuk3);
            }
            return null;
        }

        /// <summary>
        /// Adds the ID to the InactiveIds list and tries to get a new ID
        /// </summary>
        private static BatchProcessingHelper SecurityViolation(ProcessLogRun logRun, IReflectionInterface ri, string scriptId, string loginType, BatchProcessingHelper helper, string reason, bool useVuk3)
        {
            BatchProcessingHelper.AddInvalidLogin(helper.LoginId, scriptId, reason);
            InactiveIds id = new InactiveIds() { UserName = helper.UserName, Active = false, ScreenMessage = ri.GetText(23, 2, 80) };
            Ids.Add(id);
            string message = string.Format("There was an error logging into a session. UserName: {0}; Inactivated: No; Screen Message: {1}", id.UserName, id.ScreenMessage);
            logRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
            ri.Hit(ReflectionInterface.Key.Clear); // Refresh screen by clearing and going back to login screen, that way error messages don't persist to next login attempt
            ri.FastPath("LOG");

            return LoginInternal(logRun, ri, scriptId, loginType, useVuk3);
        }

        /// <summary>
        /// Invalidates the ID that is not logging in then gets a new ID to try again
        /// </summary>
        private static BatchProcessingHelper Invalidate(ProcessLogRun logRun, IReflectionInterface ri, string scriptId, string loginType, BatchProcessingHelper helper, string reason, bool useVuk3)
        {
            BatchProcessingHelper.SetInactive(helper.LoginId, scriptId, reason); //Update the Active flag to 0
            InactiveIds id = new InactiveIds() { UserName = helper.UserName, Active = false, ScreenMessage = ri.GetText(23, 2, 80) };
            Ids.Add(id);
            string message = string.Format("There was an error logging into a session. UserName: {0}; Inactivated: Yes; Screen Message: {1}", id.UserName, id.ScreenMessage);
            logRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            ri.Hit(ReflectionInterface.Key.Clear); // Refresh screen by clearing and going back to login screen, that way error messages don't persist to next login attempt
            ri.FastPath("LOG");

            return LoginInternal(logRun, ri, scriptId, loginType, useVuk3);
        }
    }
}