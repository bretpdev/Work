using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.IO;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace NDNUMINVAL
{
    public class NDNUMINVAL
    {
        public ProcessLogRun PLR { get; set; }
        public DataAccess DA { get; set; }
        static readonly object locker = new object();

        public string ScriptId
        {
            get { return "NDNUMINVAL"; }
        }

        public static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "Noble Phone Invalidation", false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            return new NDNUMINVAL().Process();
        }

        /// <summary>
        /// Main processing method.  Spawns up 3 threads to process each region.
        /// </summary>
        public int Process()
        {
            PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DA = new DataAccess(PLR);
#if DEBUG
            List<NobleData> batchRecordsOneLink = DA.LoadNobleData(DataAccess.Region.OneLink);
            List<NobleData> batchRecordsUheaaCompass = DA.LoadNobleData(DataAccess.Region.UheaaCompass);
            string uhFilePath = EnterpriseFileSystem.GetPath($"{ScriptId}_UH", DataAccessHelper.Region.Uheaa);
            string olFilePath = EnterpriseFileSystem.GetPath($"{ScriptId}_OL", DataAccessHelper.Region.Uheaa);

            foreach (NobleData record in batchRecordsUheaaCompass)
            {
                File.AppendAllText(uhFilePath, record.ToString());
                File.AppendAllText(uhFilePath, "\r\n");
            }
            foreach (NobleData record in batchRecordsOneLink)
            {
                File.AppendAllText(olFilePath, record.ToString());
                File.AppendAllText(olFilePath, "\r\n");
            }
            Console.ReadKey();
#endif
            int OneLinkReturn = 0;
            int ProcessUheaaReturn = 0;
            OneLinkReturn = ProcessOneLink();
            ProcessUheaaReturn = ProcessUheaaCompass();
            ProcessLogger.LogEnd(PLR.ProcessLogId);
            return OneLinkReturn + ProcessUheaaReturn; //If any failed this will return non-zero, indicating an issue to JAMS
        }

        /// <summary>
        /// This method opens a connection to a reflection interface, Loads up the OneLink Call data, and calls InvalidatePhoneNumber.
        /// </summary>
        private int ProcessOneLink()
        {
            return ProcessGenericCompass<OneLinkInvalidator>(DataAccess.Region.OneLink, "BatchUheaa");
        }

        /// <summary>
        /// This method opens a connection to a reflection interface, Loads up the Uheaa Compass Call data, and calls InvalidatePhoneNumber.
        /// </summary>
        private int ProcessUheaaCompass()
        {
            return ProcessGenericCompass<CompassInvalidator>(DataAccess.Region.UheaaCompass, "BatchUheaa");
        }

        private int ProcessGenericCompass<T>(DataAccess.Region region, string loginType) where T : _BaseInvalidator, new()
        {
            ReflectionInterface ri = null;
            BatchProcessingHelper login = null;
            
            try
            {
#if DEBUG
                    Console.WriteLine("Logging in to " + region.ToString());
                    Console.ReadKey();
#endif
                    ri = new ReflectionInterface();
                    login = BatchProcessingLoginHelper.Login(PLR, ri, ScriptId, loginType);
                    if (login == null)
                    {
                        PLR.AddNotification("Unable to find a usable " + loginType + " login.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        ri.CloseSession();
                        return 1;
                    }
                T invalidator = new T();
                invalidator.Initialize(PLR, ri, login.UserName, DA);

                List<NobleData> recordsToProcess = DA.LoadNobleData(region);
                foreach (NobleData record in recordsToProcess)
                {
                    StringHelper.Sanitize(record, true, false);
                    invalidator.InvalidatePhoneNumber(record);
                }
            }
            finally
            {
                if (ri != null)
                    ri.CloseSession();
                if (login != null)
                    BatchProcessingHelper.CloseConnection(login);
            }
            return 0;
        }
    }
}
