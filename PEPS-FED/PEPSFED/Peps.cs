using System;
using System.Collections.Generic;
using System.IO;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.Scripts;
using Efs = Uheaa.Common.DataAccess.EnterpriseFileSystem;
using Microsoft.SqlServer.Dts.Runtime;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Net;
using System.ComponentModel;
using System.Threading;
using Ionic.Zip;
using System.Linq;
using System.Threading.Tasks;

namespace PEPSFED
{
    public class Peps
    {
        private List<string> FileNames;
        private List<string> Files;
        private List<string> FilesNotProcessed;
        private Queue<AffiliationData> AffiliationRecords { get; set; }
        private Queue<ClosureData> ClosureRecords { get; set; }
        private Queue<ContactData> ContactRecords { get; set; }
        private Queue<DetailData> DetailRecords { get; set; }
        private Queue<IdentiferData> IdentiferRecords { get; set; }
        private Queue<OtherAddressData> OtherAddressRecords { get; set; }
        private Queue<ProgramData> ProgramRecords { get; set; }
        private DataAccess DA { get; set; }
        private Args Args { get; set; }

        public Peps(Args args)
        {
            Args = args;
            DA = new DataAccess();
            //create lists of files and files names of files the script processes
            FileNames = new List<string>();
            FileNames.Add("HEADER");//0
            FileNames.Add("DETAIL");//1
            FileNames.Add("CONTACT");//2
            FileNames.Add("PROGRAM");//3
            FileNames.Add("SCHIDS");//4
            FileNames.Add("COA");//6
            FileNames.Add("OTHERADD");//09
            FileNames.Add("CLOSURE");//11

            Files = new List<string>();
            foreach (string f in FileNames)
            {
                Files.Add((Args.LoadFrom ?? Efs.TempFolder) + f + ".txt");
            }

            //create a list of files the user downloads but that the script does not process (they just get archived)
            FilesNotProcessed = new List<string>();
            FilesNotProcessed.Add("DIRLOAN");
            FilesNotProcessed.Add("AAGENCY");
            FilesNotProcessed.Add("DEFAULT");
            FilesNotProcessed.Add("EXSITE");
            FilesNotProcessed.Add("FEDSCHCD");
            FilesNotProcessed.Add("TRAILER");
            FilesNotProcessed.Add("SCHFILE");
        }

        public int Process()
        {
            List<int> returnValues = new List<int>();
            if (!Args.SkipFileLoad)
            {
                if (string.IsNullOrWhiteSpace(Args.LoadFrom))
                    DownLoadFiles();
                returnValues.Add(LoadFiles(Args.LoadFrom));
            }
            if (!Args.SkipProcessing)
            {
                LoadQueues();
                CreateThreadsAndProcess();
            }
            return returnValues.Sum();
        }

        private void CreateThreadsAndProcess()
        {
            var login = BatchProcessingHelper.GetNextAvailableId(Program.ScriptId, "BatchCornerStone");

            ReflectionInterface ri = new ReflectionInterface();
            var log = BatchProcessingLoginHelper.Login(Program.PLR, ri, Program.ScriptId, "BatchCornerStone");
            if (log == null)
            {
                string message = string.Format("Unable to login with ID: {0}", login.UserName);
                if (Program.ApplicationArgs.ShowPrompts)
                    Dialog.Error.Ok(message);
                Console.WriteLine(message);
                return;
            }

            var obj = GetNextQueue();
            while (obj != null)
            {
                ProcessObject(obj, ri);
                obj = GetNextQueue();

            }
        }

        private void ProcessObject(ObjectBase obj, ReflectionInterface ri)
        {
            if (obj.GetType() == typeof(AffiliationData))
                new AffiliationUpdater(DA).UpdateSystem((AffiliationData)obj);
            else if (obj.GetType() == typeof(ClosureData))
                new ClosureUpdater(DA).UpdateSystem(ri, (ClosureData)obj);
            else if (obj.GetType() == typeof(ContactData))
                new ContactUpdater(DA).UpdateSystem(ri, (ContactData)obj);
            else if (obj.GetType() == typeof(DetailData))
                new DetailUpdater(DA).UpdateSystem(ri, (DetailData)obj);
            else if (obj.GetType() == typeof(IdentiferData))
                new IdentifierUpdater(DA).UpdateSystem(ri, (IdentiferData)obj);
            else if (obj.GetType() == typeof(OtherAddressData))
                new OtherAddressUpdater(DA).UpdateSystem(ri, (OtherAddressData)obj);
            else if (obj.GetType() == typeof(ProgramData))
                new ProgramUpdater(DA).UpdateSystem(ri, (ProgramData)obj);
            else
                throw new Exception(string.Format("Unknown object type encountered.  Type: {0}", obj.GetType()));
        }

        private ObjectBase GetNextQueue()
        {
            if (DetailRecords.Any())
                return DetailRecords.Dequeue();
            else if (ContactRecords.Any())
                return ContactRecords.Dequeue();
            else if (ProgramRecords.Any())
                return ProgramRecords.Dequeue();
            else if (IdentiferRecords.Any())
                return IdentiferRecords.Dequeue();
            else if (AffiliationRecords.Any())
                return AffiliationRecords.Dequeue();
            else if (OtherAddressRecords.Any())
                return OtherAddressRecords.Dequeue();
            else if (ClosureRecords.Any())
                return ClosureRecords.Dequeue();
            else
                return null;
        }

        private void LoadQueues()
        {
            AffiliationRecords = new Queue<AffiliationData>(DA.GetAffiliationData() ?? new List<AffiliationData>());
            ClosureRecords = new Queue<ClosureData>(DA.GetClosureData() ?? new List<ClosureData>());
            ContactRecords = new Queue<ContactData>(DA.GetContactData() ?? new List<ContactData>());
            DetailRecords = new Queue<DetailData>(DA.GetDetailData() ?? new List<DetailData>());
            IdentiferRecords = new Queue<IdentiferData>(DA.GetIdentiferData() ?? new List<IdentiferData>());
            OtherAddressRecords = new Queue<OtherAddressData>(DA.GetOtherAddressData() ?? new List<OtherAddressData>());
            ProgramRecords = new Queue<ProgramData>(DA.GetProgramData() ?? new List<ProgramData>());
        }

        public void DownLoadFiles()
        {
            if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)//we only download new files on Tuesday the process needs to be able to run on other days for recovery
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient webClient = new WebClient();
                string localFile = Path.Combine(Efs.TempFolder, "alltext.zip");
                //string localFile = @"T:\alltext.zip";
                if (File.Exists(localFile))
                    File.Delete(localFile);

                webClient.DownloadFile(new Uri(EnterpriseFileSystem.GetPath("PEPS")), localFile);

                using (var files = ZipFile.Read(localFile))
                {
                    foreach (var file in files)
                        file.Extract(Efs.TempFolder, ExtractExistingFileAction.OverwriteSilently);
                }

                if (File.Exists(localFile))
                    Repeater.TryRepeatedly(() => File.Delete(localFile));
            }
        }

        private int LoadFiles(string loadLocation = null)
        {
            loadLocation = loadLocation ?? Efs.TempFolder;
            if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)//we only download new files on Tuesday the process needs to be able to run on other days for recovery
            {
                //verify file exists and is not empty
                if (!CheckFiles(0))//Header file has to be correct to process files this method will process log the error
                    return 1;

                //Work the file.
                using (StreamReader pepsReader = new StreamReader(Files[0]))
                {
                    //Check the data provider code in the header row.
                    string pepsLine = pepsReader.ReadLine();
                    if (pepsLine.Length < 11 || pepsLine[10] != 'P')
                    {
                        LogError("The data provider code is not 'P'. The script will now end.");
                        return 1;
                    }
                }

                Repeater.TryRepeatedly(() => File.Delete(Files[0]));//we do not need this anymore

                //archive files downloaded by the user but not processed by the script
                for (int i = 0; i < FilesNotProcessed.Count; i++)
                {
                    File.Move(loadLocation + FilesNotProcessed[i] + ".txt", Efs.GetPath("PEPS_Archive") + FilesNotProcessed[i] + DateTime.Now.ToString().Replace("/", "").Replace(":", "") + ".txt");
                }

                for (int i = 1; i < Files.Count; i++)
                {
                    //verify file exists and is not empty
                    if (!CheckFiles(i))
                        continue;
                    LoadFile(Files[i]);
                    File.Move(Files[i], Efs.GetPath("PEPS_Archive") + FileNames[i] + DateTime.Now.ToString().Replace("/", "").Replace(":", "") + ".txt");

                }
            }

            return 0;
        }//Main()

        private bool LoadFile(string fileName)
        {
            string pkgLocation = Path.Combine(Efs.GetPath("PepsDtsxLocation"), Path.GetFileName(fileName).Replace(".txt", ".dtsx"));

            Application MyApp = new Application();
            Package Mypac = MyApp.LoadPackage(pkgLocation, null);
            DTSExecResult Myres = Mypac.Execute();
            //This is to find exact error while executing SSIS package.
            if (Myres == DTSExecResult.Failure)
            {
                string err = "";
                foreach (DtsError local_DtsError in Mypac.Errors)
                {
                    string error = local_DtsError.Description.ToString();
                    err = err + error;
                }
                string message = string.Format("Unable to load file: {0};  Error: {1}", fileName, err);
                LogError(message);
                return false;
            }

            return true;
        }

        //verify file exists and is not empty
        private bool CheckFiles(int i)
        {
            if (!File.Exists(Files[i]))
            {
                LogError(string.Format("Could not find {0}. The script will now end.", Files[i]));
                return false;
            }
            if (new FileInfo(Files[i]).Length == 0)
            {
                LogError(string.Format("The {0} file is empty. The script will now end.", Files[i]));
                return false;
            }

            return true;
        }

        public static void LogError(string message, Exception ex = null)
        {
            Program.PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            if (Program.ApplicationArgs.ShowPrompts)
                Dialog.Error.Ok(message);
        }
    }
}
