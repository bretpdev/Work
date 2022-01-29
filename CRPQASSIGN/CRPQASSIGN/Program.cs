using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CRPQASSIGN
{
    static class Program
    {
        public static string ScriptId = "CRPQASSIGN";
        private static string UserName { get; set; }
        private static string Password { get; set; }
        private static ReflectionInterface RI = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Pheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, true))
                return;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return;
            ProcessLogRun plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Pheaa, DataAccessHelper.CurrentMode, true, false, false);
            DataAccess da = new DataAccess(plr.ProcessLogId);
            while (true)
            {
                using (QueueAssignment qa = new QueueAssignment(da))
                {
                    if (qa.ShowDialog() != DialogResult.OK)
                        break;

                    LoginAndProcessQueues(qa.SelectedQueue, qa.SelectedUsers, qa.UnassignTask);
                }
            }
            if (RI != null)
                RI.CloseSession();

            plr.LogEnd();
        }

        static void LoginAndProcessQueues(Queues queue, Queue<string> users, bool unassign)
        {

            using (LoginForm lf = new LoginForm())
            {
                if (UserName.IsNullOrEmpty())
                {
                    if (lf.ShowDialog() != DialogResult.OK)
                        return;
                    RI = new ReflectionInterface();
                    UserName = lf.UserName;
                    Password = lf.Password;

                    RI.Login(UserName, Password);
                    if (!RI.ValidateRegion(DataAccessHelper.Region.Pheaa))
                    {
                        Dialog.Error.Ok("Unable to Login.  Please try again.");
                        UserName = string.Empty;
                        return;
                    }
                }


            }
            if (unassign)
                UnassignTasks(queue, users, RI);
            else
                Assign(queue, users, RI);
        }

        static void UnassignTasks(Queues queue, Queue<string> users, ReflectionInterface ri)
        {
            while (users.Any())
            {
                string user = users.Dequeue();
                ri.FastPath("TX3Z/CTX6J");
                while (ri.MessageCode != "01020")
                {
                    ri.FastPath("TX3Z/CTX6J");
                    ri.PutText(7, 42, queue.Queue, true);
                    ri.PutText(8, 42, queue.SubQueue, true);
                    ri.PutText(9, 42, "", true);
                    ri.PutText(10, 42, queue.Arc, true);
                    ri.PutText(13, 42, user, true);
                    ri.PutText(12, 42, "A", ReflectionInterface.Key.Enter, true);


                    if (ri.ScreenCode == "TXX6N")
                        ri.PutText(21, 18, "01", ReflectionInterface.Key.Enter);
                    else if (ri.MessageCode == "01020" || ri.MessageCode == "90018" || ri.MessageCode == "80014")
                        break;

                    ri.PutText(8, 15, "", ReflectionInterface.Key.Enter, true);
                    ri.Hit(ReflectionInterface.Key.F12);

                    if (ri.ScreenCode == "TXX6N")
                    {
                        ri.Hit(ReflectionInterface.Key.F12);
                        ri.Hit(ReflectionInterface.Key.Enter);
                    }
                }
            }

            Dialog.Def.Ok("All tasks have been unassigned.");
        }


        private static void Assign(Queues queue, Queue<string> users, ReflectionInterface ri)
        {
            string ssn = AccessQueue(queue, users, ri);

            while (ssn.IsPopulated())
            {
                ri.FastPath("TX3Z/CTX6J");
                ri.PutText(7, 42, queue.Queue, true);
                ri.PutText(8, 42, queue.SubQueue, true);
                ri.PutText(9, 42, ssn + "*", true);
                ri.PutText(10, 42, queue.Arc, true);
                ri.PutText(9, 76, "A", ReflectionInterface.Key.Enter, true);

                if (ri.ScreenCode == "TXX6N") //Muliple tasks
                    AssignMultipleTask(ri, users);
                else if (ri.ScreenCode == "TXX6O")
                {
                    string user = users.Dequeue();
                    ri.PutText(8, 15, user, ReflectionInterface.Key.Enter);
                    users.Enqueue(user);
                }
                else
                {
                    Dialog.Info.Ok("There are no task to assign");
                    return;
                }

                ssn = AccessQueue(queue, users, ri);
            }
        }

        static void AssignMultipleTask(ReflectionInterface ri, Queue<string> users)
        {
            string user = null;
            for (int row = 9; ri.MessageCode != "90007"; row += 2)
            {
                if (ri.CheckForText(row, 3, " "))
                {
                    if (ri.GetText(2, 69, 3) == "20")
                    {
                        ri.Hit(ReflectionInterface.Key.Enter);
                        row = 7;
                        continue;
                    }
                    ri.Hit(ReflectionInterface.Key.F8);
                    row = 7;
                    continue;
                }
                if (ri.GetText((row + 1), 20, 7).IsPopulated())
                {
                    user = ri.GetText((row + 1), 20, 7);
                    if (!users.Where(p => p == user).Any())//If the user is not in the list of selected users we want to just get the next user in the queue.
                        user = "";
                    break;
                }
            }

            bool deQueued = false;
            if (user.IsNullOrEmpty())
            {
                deQueued = true;
                user = users.Dequeue();
            }

            ri.PutText(5, 70, user, ReflectionInterface.Key.Enter);
            ri.Hit(ReflectionInterface.Key.F4);
            if(deQueued)
                users.Enqueue(user);
        }

        static string AccessQueue(Queues queue, Queue<string> users, ReflectionInterface ri)
        {
            ri.FastPath("TX3Z/CTX6J");
            ri.PutText(7, 42, queue.Queue, true);
            ri.PutText(8, 42, queue.SubQueue, true);
            ri.PutText(9, 42, "", true);
            ri.PutText(10, 42, queue.Arc, true);
            ri.PutText(12, 42, "U", ReflectionInterface.Key.Enter, true);

            if (ri.ScreenCode == "TXX6N")
                return ri.GetText(9, 38, 9);
            else if (ri.ScreenCode == "TXX6O")
                return ri.GetText(13, 2, 9);
            else
            {
                Dialog.Info.Ok("There are no task to assign");
                return null;
            }
        }
    }
}
