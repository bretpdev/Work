using Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WinForms;

namespace Uheaa.Common.Scripts
{
    public partial class ReflectionInterface : IReflectionInterface
    {
        [Flags]
        public enum Flag
        {
            None = 0,
            MasterBatchScript = 1,
            Jams = 2,
            OpenSession = 3
        }

        public enum Key
        {
            F1 = 373,
            F2 = 379,
            F3 = 380,
            F4 = 381,
            F5 = 382,
            F6 = 383,
            F7 = 384,
            F8 = 385,
            F9 = 386,
            F10 = 363,
            F11 = 364,
            F12 = 365,
            Enter = 289,
            Clear = 277,
            EndKey = 290,
            Up = 354,
            Tab = 405,
            Home,
            Insert = 329,
            Down = 355,
            Esc = 393,
            PrintScreen = 389,
            PageUp = 354,
            None = 0
        }

        public Session ReflectionSession { get; set; }
        public ProcessLogData ProcessLogData { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public DataAccessHelper.Region Region { get; set; }
        private string userId;
        public string UserId
        {
            get
            {
                if (userId.IsNullOrEmpty())
                    userId = GetUserId();
                return userId;
            }
            set
            {
                userId = value;
            }
        }

        private string GetUserId()
        {
            FastPath("PROF");
            return GetText(2, 49, 7);
        }

        public bool CalledByJams
        {
            get
            {
                return ReflectionSession.MacroData.Contains("JAMS");
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return !CheckForText(16, 2, "LOGON");
            }
        }

        public ReflectionInterface() : this(OpenExistingSession(Flag.None)) { }

        public ReflectionInterface(Flag flag) : this(OpenExistingSession(flag)) { }

        public ReflectionInterface(string sessionFileName, string oleName) : this(OpenExistingSession(Flag.None, oleName)) { }

        public ReflectionInterface(Session reflectionSession)
        {
            ReflectionSession = reflectionSession;
        }

        private static Session ConnectToOpenSession()
        {
            if (Process.GetProcessesByName("R8win").Length == 0 && Process.GetProcessesByName("MdSession").Length == 0)
                return null;

            List<Session> openSessions = new List<Session>();
            List<string> mdSessions = new List<string>();

            string wmiQuery = string.Format("select CommandLine from Win32_Process where Name='{0}'", "MdSession.exe");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
            ManagementObjectCollection retObjectCollection = searcher.Get();
            foreach (ManagementObject retObject in retObjectCollection) //the OLE Server name of the MD session is a guid that we can parse from the command line args of the MdSession
            {
                string commandLine = retObject["CommandLine"].ToString();
                string guid = commandLine.Substring(commandLine.LastIndexOf('"') + 2);
                mdSessions.Add(guid.Remove(guid.Length - 2));
            }
            foreach (string guid in mdSessions)
            {
                try
                {
                    openSessions.Add((Session)Microsoft.VisualBasic.Interaction.GetObject(guid, null));
                }
                catch
                {
                    continue;
                }
            }

            for (int dudeIndex = 1; dudeIndex <= 10; dudeIndex++)
            {
                try
                {
                    openSessions.Add((Session)Microsoft.VisualBasic.Interaction.GetObject(string.Format("RIBMGEN{0}", dudeIndex), null));
                }
                catch
                {
                    continue;
                }
            }

            try
            {
                openSessions.Add((Session)Microsoft.VisualBasic.Interaction.GetObject("RIBMGEN", null));
            }
            catch
            {
                //No session found continue
            }

            try
            {
                openSessions.Add((Session)Microsoft.VisualBasic.Interaction.GetObject("RIBM", null));
            }
            catch
            {
                //No session found continue
            }

            Session selectedSession = null;
            if (openSessions.Count == 0)
                return null;
            else if (openSessions.Count == 1)
                return openSessions[0];
            else
            {
                StringBuilder message = new StringBuilder(string.Format("Which session do you want to connect to?{0}{0}", Environment.NewLine));
                for (int count = 1; count < openSessions.Count + 1; count++)
                {
                    message.AppendFormat("{0} - {1}{2}", count.ToString(), openSessions[(count - 1)].Caption, Environment.NewLine);
                }
                int selection = 0;
                DialogResult result = DialogResult.Cancel;
                while (result != DialogResult.OK)
                {
                    using (var input = new InputBox<NumericTextBox>(message.ToString(), "Select a Session"))
                    {
                        result = input.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            selection = int.Parse(input.InputControl.Text);
                        }

                        try
                        {
                            selectedSession = openSessions[selection - 1];
                        }
                        catch
                        {
                            MessageBox.Show("That was not a valid selection please try again.");
                            result = DialogResult.Cancel;
                        }
                    }
                }

            }

            return selectedSession;
        }

        public static Session OpenExistingSession(Flag flags)
        {
            return OpenExistingSession(flags, "RIBMGEN");
        }

        static object sessionOpenerLock = new object();
        public static Session OpenExistingSession(Flag flags, string oleName)
        {
            if (flags == Flag.OpenSession)
            {
                return ConnectToOpenSession();
            }
            Session ses;
            lock (sessionOpenerLock)
            {
                Proc.Start("GenericSession");

                //pause for 3 secondsso the session is ready to handle commands
                Thread.Sleep(new TimeSpan(0, 0, 3));

                while (true)
                {
                    try
                    {
                        ses = (Session)Microsoft.VisualBasic.Interaction.GetObject("RIBMGEN", null);
                        ses.OLEServerName = string.Format("GENRIBM{0}", Guid.NewGuid().ToBase64String());

                        List<string> macroData = new List<string>();
                        if (flags == Flag.MasterBatchScript)
                        {
                            macroData.Add("MasterBatchScript");
                        }
                        if (flags == Flag.Jams)
                        {
                            macroData.Add("JAMS");
                        }

                        ses.MacroData = string.Join(",", macroData.ToArray());

                        ses.BDTIgnoreScrollLock = 1;

                        break;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(new TimeSpan(0, 0, 2));
                    }
                }
            }
            return ses;
        }

        public virtual bool CheckForText(int row, int column, params string[] text)
        {
            foreach (string textItem in text)
            {
                if (string.Compare(ReflectionSession.GetDisplayText(row, column, textItem.Length), textItem, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual void CloseSession()
        {
            //ReflectionSession.Quit();
            ReflectionSession.Exit();
        }

        public void EnterText(string text)
        {
            ReflectionSession.TransmitANSI(text);
        }

        public virtual void FastPath(string input)
        {
            Hit(Key.Clear);
            PutText(1, 1, input, Key.Enter, false);
        }

        public Coordinate GetCurrentCoordinate()
        {
            return new Coordinate() { Column = ReflectionSession.CursorColumn, Row = ReflectionSession.CursorRow };
        }

        public string GetDisplayText(int row, int column, int length)
        {
            return ReflectionSession.GetDisplayText(row, column, length);
        }

        public virtual string GetText(int row, int col, int length)
        {
            return ReflectionSession.GetDisplayText(row, col, length).Trim();
        }

        public string GetTextRemoveUnderscores(int row, int col, int length)
        {
            return GetText(row, col, length).Replace("_", "");
        }

        public Coordinate FindText(string text)
        {
            if (ReflectionSession.FindText(text, 1, 1, null) == 0)
            {
                return null;
            }

            return new Coordinate() { Column = ReflectionSession.FoundTextColumn, Row = ReflectionSession.FoundTextRow };
        }

        public Coordinate FindText(string text, int startRow, int startColumn)
        {
            if (ReflectionSession.FindText(text, startRow, startColumn, null) == 0)
            {
                return null;
            }

            return new Coordinate() { Column = ReflectionSession.FoundTextColumn, Row = ReflectionSession.FoundTextRow };
        }

        public bool Hit(Key keyToHit, int numberOfTimes)
        {
            for (int count = 1; count <= numberOfTimes; count++)
            {
                if (keyToHit == Key.None)
                    return false;
                ReflectionSession.TransmitTerminalKey((int)keyToHit);

                ReflectionSession.WaitForEvent(1, "30", "0", 1, 1);
            }
            return true;
        }

        public virtual bool Hit(Key keyToHit)
        {
            return Hit(keyToHit, 1);
        }

        public void HitChar(char key)
        {
            ReflectionSession.TransmitANSI(key.ToString());
            ReflectionSession.WaitForEvent(1, "30", "0", 1, 1);
        }

        public bool Login(string userId, string password) { return Login(userId, password, DataAccessHelper.CurrentRegion); }
        public bool Login(string userId, string password, DataAccessHelper.Region region) { return Login(userId, password, region, false); }
        public bool Login(string userId, string password, DataAccessHelper.Region region, bool useVuk3)
        {
            if (region == DataAccessHelper.Region.None)
            {
                throw new ArgumentException("You must specify a region when calling the Login function.", "region");
            }

            try
            {
                if (MessageCode != "ON004" && MessageCode != "ON09A") //no userid entered, exceeded maximum login attempts
                {
                    ReflectionSession.WaitForDisplayString(">", "0:0:30", 16, 10);
                    string logonText = DataAccessHelper.TestMode ? "QTOR" : "PHEAA";
                    PutText(16, 12, logonText, Key.Enter);
                }

                ReflectionSession.WaitForDisplayString("USERID", "0:0:30", 20, 8);

                PutText(20, 18, userId);
                PutText(20, 40, password, Key.Enter);
            }
            catch (COMException)//We are assuming when a COM exception happens the session is dead, and cannot be used.
            {
                return false;
            }
            catch (Exception)
            {
                LogOut();
                return Login(userId, password, region);
            }

            //return false if credentials are rejected
            if (CheckForText(20, 8, "USERID==>"))
            {
                return false;
            }

            if (DataAccessHelper.TestMode)
            {
                string regionLabel = "_ RS/UT";

                if (region == DataAccessHelper.Region.CornerStone)
                {
                    if (useVuk3)
                        regionLabel = "_ K3/FD";
                    else
                        regionLabel = "_ K1/FD";
                }
                else if (region == DataAccessHelper.Region.Pheaa)
                {
                    regionLabel = "_ CB/FD";
                }

                ReflectionSession.FindText(regionLabel, 3, 5, null);
                PutText(ReflectionSession.FoundTextRow, ReflectionSession.FoundTextColumn, "X", Key.Enter);
                Hit(Key.F10);

            }
            else if (region == DataAccessHelper.Region.Pheaa)
            {
                Hit(Key.F10);
            }
            else if (region == DataAccessHelper.Region.CornerStone)
            {
                FastPath("STUPKU");
                Hit(Key.F10);
            }

            string blah = UserId;
            FastPath("TX3Z/ITX1J");

            return CheckForText(1, 72, "TXX1K");
        }

        public void LogOut()
        {
            if (CheckForText(16, 2, "LOGON ==>"))
                return;
            Hit(Key.Clear);
            PutText(1, 2, "LOG", Key.Enter);
        }

        public void PauseForInsert()
        {
            ReflectionSession.WaitForTerminalKey(329, "1:00:00");
            Hit(Key.Insert);
        }

        public void PutTextAndWait(int row, int column, string text, Key keyToHit, int rowWait, int columnWait, string textWait)
        {
            PutText(row, column, text);
            WaitForText(rowWait, columnWait, textWait);
        }

        public virtual void PutText(int row, int column, string text, Key keyToHit, bool blankFieldFirst)
        {
            if (ReflectionSession.GetFieldAttributes(row, column) == 1)
            {
                throw new Exception(string.Format(@"Attempted to write ""{0}"" into row {1} column {2}, but that location on the screen is not writeable.", text, row, column));
            }

            const int MaxTextLength = 260;
            if (!text.IsNullOrEmpty() && text.Length > MaxTextLength)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Format("The script has been asked to enter more text than the session will allow. The session allows {0} characters to be written at once, but the requested text length is {1} characters.", MaxTextLength, text.Length));
                builder.AppendLine();
                builder.Append("The requested text follows:");
                builder.AppendLine();
                builder.Append(text);
                throw new Exception(builder.ToString());
            }

            ReflectionSession.MoveCursor(row, column);

            if (blankFieldFirst)
            {
                Hit(Key.EndKey);
            }

            ReflectionSession.TransmitANSI(text);

            if (keyToHit != Key.None)
            {
                Hit(keyToHit);
            }
        }

        public void PutText(int row, int column, string text)
        {
            PutText(row, column, text, Key.None);
        }

        public void PutText(int row, int column, string text, Key keyToHit)
        {
            PutText(row, column, text, keyToHit, false);
        }

        public void PutText(int row, int column, string text, bool blankFieldFirst)
        {
            PutText(row, column, text, Key.None, blankFieldFirst);
        }
        /// <summary>
        /// Continually check for the specified text.  Returns false if the text does not appear within 60 seconds.
        /// </summary>
        public bool WaitForText(int row, int column, string text)
        {
            return WaitForText(row, column, text, 60);
        }
        /// <summary>
        /// Continually check for the specified text.  Returns false if the text does not appear within the given time limit.
        /// </summary>
        public bool WaitForText(int row, int column, string text, int seconds)
        {
            int milliseconds = seconds * 1000;
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.ElapsedMilliseconds <= milliseconds)
                if (CheckForText(row, column, text)) return true;
            return false;
        }

        /// <summary>
        /// Wait for the specified Information to be displayed on the screen.
        /// </summary>
        /// <param name="waitFor"></param>
        public void Wait(Screen waitFor)
        {
            WaitForText(waitFor.Row, waitFor.Column, waitFor.Text);
        }

        public bool Check(Screen checkFor)
        {
            return CheckForText(checkFor.Row, checkFor.Column, checkFor.Text);
        }

        /// <summary>
        /// Checks 23,2 for the specified Message Code.
        /// </summary>
        /// <param name="code">The 5-digit message code</param>
        /// <returns>Return true if the message code was found</returns>
        public bool CheckForMessage(string code)
        {
            return CheckForText(23, 2, code);
        }

        /// <summary>
        /// Returns the message starting at 23, 2
        /// </summary>
        /// <returns></returns>
        public string Message
        {
            get
            {
                return GetText(23, 2, 80);
            }
        }

        /// <summary>
        /// Returns the message starting at 22, 3
        /// </summary>
        public string AltMessage
        {
            get
            {
                return GetText(22, 3, 80);
            }
        }

        /// <summary>
        /// Returns only the 5-digit message code starting at 23, 2
        /// </summary>
        public string MessageCode
        {
            get
            {
                return GetText(23, 2, 5);
            }
        }

        /// <summary>
        /// Returns only the 5-digit message code starting at 22, 3
        /// </summary>
        public string AltMessageCode
        {
            get
            {
                return GetText(22, 3, 5);
            }
        }

        /// <summary>
        /// Return the 5-digit screen code from 1, 72
        /// </summary>
        public string ScreenCode
        {
            get
            {
                return GetText(1, 67, 11);
            }
        }

        public bool WaitForScreenCode(string code)
        {
            return WaitForText(1, 72, code);
        }

        public void Stup()
        {
            Stup(DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
        }

        public void Stup(DataAccessHelper.Region region, DataAccessHelper.Mode mode)
        {
            string fastPathText = string.Empty;

            if (region == DataAccessHelper.Region.Uheaa)
            {
                if (mode != DataAccessHelper.Mode.Live)
                    fastPathText = "STUPQ0RS";
                else
                    fastPathText = "STUPUT";
            }
            else if (region == DataAccessHelper.Region.CornerStone)
            {
                if (mode != DataAccessHelper.Mode.Live)
                    fastPathText = "STUPVUK3";
                else
                    fastPathText = "STUPKU";
            }
            FastPath(fastPathText);
            if (region == DataAccessHelper.Region.CornerStone)
                Hit(Key.F10);
        }

        public string ReAssignQueueTask(string queue, string subqueue, string currentUser, string reAssignUserId)
        {
            FastPath("TX3Z/CTX6J");
            PutText(7, 42, queue, true);
            PutText(8, 42, subqueue, true);
            PutText(9, 42, "", true);
            PutText(13, 42, currentUser, Key.Enter, true);
            if (ScreenCode != "TXX6O")
                return Message;
            PutText(8, 15, reAssignUserId, Key.Enter, true);
            return Message;
        }

        public bool BorrowerHasQueue(string ssn, string queue)
        {
            FastPath("TX3Z/ITX6T" + ssn);
            //PutText(6, 41, ssn, ReflectionInterface.Key.Enter);
            if (ScreenCode == "TXX6U") return false;  //queue task does not exist
            for (int row = 7; MessageCode != "90007"; row += 5)
            {
                if (row > 17)
                {
                    row = 2;
                    Hit(ReflectionInterface.Key.F8);
                    continue;
                }

                if (CheckForText(row, 8, queue))
                    return true;
            }

            return false;
        }

        /// <param name="maximumArcAgeInDays">If specified, only returns true if the arc was created less than the given amount of days ago</param>
        /// <returns></returns>
        public bool BorrowerHasArc(string accountNumber, string arc, int? maximumArcAgeInDays)
        {
            if (accountNumber.Length != 10)
                throw new Exception("Arc Lookup must use an Account Number, not an SSN or otherwise.");
            FastPath("TX3Z/ITD2A");
            PutText(4, 16, accountNumber);
            PutText(11, 65, arc, Key.Enter);
            if (ScreenCode == "TDX2B") return false; //screen didn't change, so arc doesn't exist
            if (ScreenCode == "TDX2C") //more than one arc, select the most recent one
                PutText(7, 2, "X", Key.Enter);
            if (maximumArcAgeInDays.HasValue)
            {
                DateTime? creationDate = GetText(13, 31, 8).ToDateNullable();
                if (creationDate.HasValue)
                    return (DateTime.Today - creationDate.Value).TotalDays <= maximumArcAgeInDays.Value;
            }
            return true;
        }

        public DataAccessHelper.Region GetCurrentRegion()
        {
            FastPath("TX3Z/ITX1J");
            if (CheckForText(1, 38, "UHEAAFED")) return DataAccessHelper.Region.CornerStone;
            if (CheckForText(1, 39, "UHEAA")) return DataAccessHelper.Region.Uheaa;
            return DataAccessHelper.Region.None;
        }

        /// <summary>
        /// Checks to see if a borrower is in Onelink
        /// </summary>
        /// <returns>True if the borrower is in Onelink.  False if they are not.</returns>
        public bool BorrowerExistsInOnelink(string ssn)
        {
            FastPath("LP22I" + ssn);
            return CheckForText(1, 69, "DEMOGRAPHICS");
        }

        public bool ValidateRegion(DataAccessHelper.Region region, bool showMessage)
        {
            bool regionIsValid;
            FastPath("TX3Z/ITX1J");

            switch (region)
            {
                case DataAccessHelper.Region.CornerStone:
                    regionIsValid = CheckForText(1, 38, "UHEAAFED");
                    break;
                case DataAccessHelper.Region.Uheaa:
                    regionIsValid = CheckForText(1, 39, "UHEAA");
                    break;
                case DataAccessHelper.Region.Pheaa:
                    regionIsValid = CheckForText(1, 38, "FEDERAL");
                    break;
                default:
                    regionIsValid = false;
                    break;
            }

            if (!regionIsValid && showMessage)
            {
                string message = string.Format("You must be in the {0} region to use this script.", region);
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return regionIsValid;
        }

        public bool ValidateRegion(DataAccessHelper.Region region)
        {
            return ValidateRegion(region, true);
        }

        public bool ATD42AllLoans(string ssnOrAccountNum, string arc, string comment)
        {
            if (DataAccessHelper.CurrentRegion != DataAccessHelper.Region.Pheaa)//Only use this in the PHEAA region
                return false;
            FastPath("TX3Z/ATD42" + ssnOrAccountNum);
            if (!CheckForText(1, 72, "TDX43"))
                return false;
            //Find the ARC
            Coordinate arcLocation = FindText(arc);
            while (arcLocation == null)
            {
                Hit(Key.F8);
                if (CheckForText(23, 2, "90007"))
                    return false;
                arcLocation = FindText(arc);
            }

            //Select the ARC
            PutText(arcLocation.Row, arcLocation.Column - 5, "01", Key.Enter);
            //Exit the function if the selection screen is not displayed
            if (!CheckForText(1, 72, "TDX44"))
                return false;

            for (int row = 11; !CheckForText(23, 2, "90007"); row++)
            {
                if (row > 18)
                {
                    Hit(Key.F8);
                    if (CheckForText(23, 2, "90003"))
                        return false;
                    else
                        row = 10;
                }

                if (CheckForText(row, 18, "_"))
                    PutText(row, 18, "X");
            }


            //Check to see if there was a selection made
            if (CheckForText(23, 2, "01490") || CheckForText(23, 2, "01764"))
                return false;

            Hit(Key.F4);
            PutText(21, 2, comment);
            Hit(Key.Enter);

            //Check to make sure the comment was successful
            if (CheckForText(23, 2, "02860"))
                return true;
            else
                return false;

        }

        public bool HasOpenLoanOnOneLINK(string ssn)
        {
            List<string> openLoanIndicators = new List<string> { "CR", "AL", "DA", "FB", "IA", "ID", "IG", "IM", "RP", "UA", "UB" };
            FastPath("LG02I;" + ssn);
            if (CheckForText(1, 60, "LOAN APPLICATION MENU"))
            {
                //Nothing found on LG02
                return false;
            }
            else if (CheckForText(1, 58, "LOAN APPLICATION SELECT"))
            {
                //Selection screen was encountered
                int row = 10;
                while (!CheckForText(22, 3, "46004 NO MORE DATA TO DISPLAY"))
                {
                    int lg02Page;
                    if (!int.TryParse(GetText(2, 73, 2), out lg02Page))
                    {
                        LogRun.AddNotification("Could not parse int from lg02 in HasOpenLoanOneLINK", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                        row++;
                        if (CheckForText(row, 3, "S", " "))
                        {
                            //Page forward
                            Hit(Key.F8);
                            row = 10;
                        }
                        continue;
                    }
                    if (CheckForText(row, 75, openLoanIndicators.ToArray()) || HasOpenClaimPaid("LG02I;", ssn, lg02Page, row))
                    {
                        return true;
                    }
                    row++;
                    if (CheckForText(row, 3, "S", " "))
                    {
                        //Page forward
                        Hit(Key.F8);
                        row = 10;
                    }
                }
            }
            else
            {
                //Target screen on LG02 was encountered
                //go to LG10
                FastPath("LG10I" + ssn);
                if (CheckForText(1, 74, "DISPLAY"))
                {
                    //Target screen. Check the status of each loan
                    int lg10Page;
                    if (!int.TryParse(GetText(2, 73, 2), out lg10Page))
                    {
                        LogRun.AddNotification("Could not parse int from lg10 in HasOpenLoanOneLINK", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                        return false;
                    }
                    if (CheckForText(11, 59, openLoanIndicators.ToArray()) || HasOpenClaimPaid("LG10I", ssn, lg10Page, 11))
                    {
                        return true;
                    }
                }
                else if (CheckForText(1, 75, "SELECT"))
                {
                    //Multiple loan holders. Select each one in turn.
                    int row = 7;
                    do
                    {
                        PutText(19, 15, GetText(row, 6, 1), Key.Enter);
                        int lg10Page;
                        if (!int.TryParse(GetText(2, 73, 2), out lg10Page))
                        {
                            LogRun.AddNotification("Could not parse int from lg10 in HasOpenLoanOneLINK", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                            Hit(Key.F12);
                            row++;
                            continue;
                        }
                        if (CheckForText(11, 59, openLoanIndicators.ToArray()) || HasOpenClaimPaid("LG10I", GetText(1, 9, 16), lg10Page, 11))
                        {
                            return true;
                        }
                        Hit(Key.F12);
                        row++;
                    } while (!CheckForText(row, 6, " "));
                }
            }
            return false;
        }

        /// <summary>
        /// The HasOpenClaimPaid function is called by HasOpenLoanOnOneLink from LG02 or LG10 to
        /// see if a loan's status is 'CP' (Claim Paid) and has an open status on LC05.
        /// </summary>
        public bool HasOpenClaimPaid(string screen, string ssn, int lgPage, int row)
        {
            //Still on LG02 or LG10, check the loan status to see if it's 'CP'
            if (screen == "LG02I;" && !CheckForText(row, 75, "CP"))
            {
                //This loan isn't a 'CP' status, so no need to check LC05
                return false;
            }
            else if (screen == "LG10I" && !CheckForText(row, 59, "CP"))
            {
                //This loan isn't a 'CP' status, so no need to check LC05
                return false;
            }
            else if (screen == "LG02I;" && CheckForText(row, 75, "CP"))
            {
                //Select this loan to go to the detail screen
                PutText(21, 13, GetText(row, 2, 2), Key.Enter);
                //Get the loan ID and check LC05 for an open status.
                string loanId = GetLoanInfoFromLG02().Cluid;
                return LC05ShowsOpenStatus(ssn, screen, lgPage, loanId);
            }
            else if (screen == "LG10I" && CheckForText(row, 59, "CP"))
            {
                //Get the loan ID and check LC05 for an open status.
                string loanId = GetText(row, 4, 19);
                return LC05ShowsOpenStatus(ssn, screen, lgPage, loanId);
            }
            else
            {
                //Bad screen input
                return false;
            }
        }

        /// <summary>
        /// Get loan information from LG02.
        /// You mustt be on a LG02 target screen before calling this method.
        /// </summary>
        /// <returns>
        /// An LG02LoanInfo object poplated with the pertinent informtaion from LG02,
        /// or null if the current screen is not a recognized LG02 target screen.
        /// </returns>
        public LG02LoanInfo GetLoanInfoFromLG02()
        {
            if (CheckForText(1, 56, "CONSOLIDATION APPLICATION"))
            {
                return new LG02LoanInfo
                {
                    Cluid = GetText(5, 31, 19),
                    LoanHolderCode = GetText(11, 46, 6),
                    LoanServicerCode = GetText(11, 71, 6),
                    LoanType = "CL",
                    StudentName = string.Empty,
                    StudentSsn = string.Empty
                };
            }
            else if (CheckForText(1, 60, "PLUS LOAN APPLICATION"))
            {
                LG02LoanInfo loanInfo = new LG02LoanInfo
                {
                    LoanType = "PL",
                    StudentName = GetText(9, 8, 9),
                    StudentSsn = GetText(9, 18, 30)
                };
                if (CheckForText(5, 62, "DT"))
                {
                    //Common
                    loanInfo.Cluid = GetText(2, 35, 19);
                    loanInfo.LoanHolderCode = GetText(18, 42, 6);
                    loanInfo.LoanServicerCode = GetText(18, 64, 6);
                }
                else if (CheckForText(5, 62, "BWR"))
                {
                    //Pre-Common
                    loanInfo.Cluid = GetText(2, 33, 19);
                    loanInfo.LoanHolderCode = GetText(19, 44, 6);
                    loanInfo.LoanServicerCode = GetText(19, 73, 6);
                }
                else
                {
                    MessageBox.Show("The LG02 screen displayed is not recognized. Contact Systems Support for assistance.", "Screen Not Recognized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }
                return loanInfo;
            }
            else if (CheckForText(1, 60, "PLUS MASTER PROM NOTE"))
            {
                LG02LoanInfo loanInfo = new LG02LoanInfo
                {
                    Cluid = GetText(3, 30, 19),
                    LoanHolderCode = GetText(18, 41, 6),
                    LoanServicerCode = GetText(18, 64, 6)
                };
                if (CheckForText(10, 4, "PLUS GRADUATE LOAN"))
                {
                    loanInfo.StudentName = string.Empty;
                    loanInfo.StudentSsn = string.Empty;
                    loanInfo.LoanType = "GB";
                }
                else if (CheckForText(10, 4, "STD"))
                {
                    loanInfo.StudentName = GetText(10, 18, 30);
                    loanInfo.StudentSsn = GetText(10, 8, 9);
                    loanInfo.LoanType = "PL";
                }
                else
                {
                    MessageBox.Show("The LG02 screen displayed is not recognized.  Contact Systems Support for assistance.", "Screen Not Recognized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }
                return loanInfo;
            }
            else if (CheckForText(1, 61, "SLS LOAN APPLICATION"))
            {
                return new LG02LoanInfo
                {
                    Cluid = GetText(2, 33, 19),
                    LoanHolderCode = GetText(19, 44, 6),
                    LoanServicerCode = GetText(19, 73, 6),
                    LoanType = "SL",
                    StudentName = string.Empty,
                    StudentSsn = string.Empty
                };
            }
            else if (CheckForText(1, 56, "STAFFORD MASTER PROM NOTE"))
            {
                LG02LoanInfo loanInfo = new LG02LoanInfo
                {
                    Cluid = GetText(3, 32, 19),
                    LoanHolderCode = GetText(18, 49, 6),
                    LoanServicerCode = GetText(18, 73, 6),
                    StudentName = string.Empty,
                    StudentSsn = string.Empty
                };
                if (CheckForText(4, 59, "SUB"))
                {
                    loanInfo.LoanType = "SF";
                }
                else
                {
                    loanInfo.LoanType = "SU";
                }
                return loanInfo;
            }
            else if (CheckForText(1, 56, "STAFFORD LOAN APPLICATION"))
            {
                LG02LoanInfo loanInfo = new LG02LoanInfo
                {
                    Cluid = GetText(3, 33, 19),
                    LoanHolderCode = GetText(18, 49, 6),
                    LoanServicerCode = GetText(18, 73, 6),
                    StudentName = string.Empty,
                    StudentSsn = string.Empty
                };
                if (CheckForText(4, 59, "SUB"))
                {
                    loanInfo.LoanType = "SF";
                }
                else
                {
                    loanInfo.LoanType = "SU";
                }
                return loanInfo;
            }
            else
            {
                MessageBox.Show("The LG02 screen displayed is not recognized.  Contact Systems Support for assistance.", "Screen Not Recognized", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private bool LC05ShowsOpenStatus(string ssn, string screen, int lgPage, string loanId)
        {
            FastPath("LC05I" + ssn);
            //Check each loan to see if it matches the LoanID from LG02 or LG10
            int LC05Record = 1;
            do
            {
                //Loans are listed one every three lines, starting on line 7 (four per page).
                string selection = GetText(LC05Record * 3 + 4, 3, 1);
                PutText(21, 13, selection, Key.Enter);
                //Page down twice to get to the page with the loan ID.
                Hit(Key.F10);
                Hit(Key.F10);
                //Check the loan ID here against what we got from LG02 or LG10.
                if (CheckForText(3, 13, loanId))
                {
                    bool foundOpenStatus = false;
                    Hit(Key.F10);
                    bool isSatisfied = CheckForText(4, 10, "04");
                    bool isDefaultedButNotTransferred = CheckForText(4, 10, "03") && CheckForText(19, 73, "MM");
                    if (isSatisfied || isDefaultedButNotTransferred)
                    {
                        //Open status found.
                        foundOpenStatus = true;
                    }
                    else
                    {
                        //If '03' statis was not found, or the loan was transferred, then the loan isn't open
                        //return the script to LG02 or LG10 on the last page it was looking at.
                        FastPath(screen + ssn);
                        do
                        {
                            Hit(Key.F8);
                        } while (int.Parse(GetText(2, 74, 1)) != lgPage);
                    }
                    //End of logic for a matching loan ID. Whatever the outcome, if the ID matched, we're done here.
                    return foundOpenStatus;
                }
                else
                {
                    //Start of logic for a non-match on the loan ID.
                    //In this case, we need to back out to LC05...
                    Hit(Key.F12);
                    //and look for a match on the next loan.
                    LC05Record++;
                    //Before restarting the loop, check to see if this is the last record on the page.
                    if (LC05Record == 5)
                    {
                        //There are only four records per page, so a record number 5 means next page.
                        Hit(Key.F8);
                        LC05Record = 1;
                    }

                }

            } while (!CheckForText(LC05Record * 3 + 4, 3, " "));
            //If we com out of LC05 with no matches, then it's safe to say there's no open status associated with the loan.
            return false;
        }

        public bool InvalidateCompassEmail(string ssn, string emailType)
        {
            List<string> codes = new List<string>() { "33", "05", "AA" };
            FastPath($"TX3ZCTX1J;{ssn}");
            Hit(Key.F2);
            Hit(Key.F10);
            foreach (string code in codes)
            {
                PutText(10, 14, emailType, Key.Enter);
                PutText(9, 20, code);
                PutText(12, 14, "N");
                if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone && emailType.ToLower() != "c")
                {
                    PutText(21, 59, "N");
                    PutText(21, 65, GetText(10, 14, 1));
                }
                PutText(11, 17, DateTime.Now.ToString("MMddyy"), Key.Enter);
                if (emailType.ToLower() != "c" || CheckForText(23, 2, "01005"))
                    break;
            }
            //Try using the BB code when the email fails to invalidate if the email is type C
            if(!CheckForText(23, 2, "01005") && emailType.ToLower() == "c")
            {
                PutText(10, 14, emailType, Key.Enter);
                PutText(9, 20, "BB");
                PutText(12, 14, "N");
                PutText(11, 17, DateTime.Now.ToString("MMddyy"), Key.Enter);
            }
            return CheckForText(23, 2, "01005");
        }

        public bool InvalidateOnelinkEmail(string ssn)
        {
            FastPath($"LP22C{ssn}");
            PutText(18, 56, "N");
            PutText(3, 9, "K", Key.Enter);
            Hit(Key.F6);
            return WaitForText(22, 3, "49000");
        }

    }
}
