using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.IO;
using Uheaa.Common;

namespace NoblePhoneInvalidation
{
	public class NoblePhoneInvalidation
	{
		public ProcessLogData LogData { get; set; }

		static readonly object locker = new object();

		public string ScriptId
		{
			get	{ return "NDNUMINVAL"; }
		}

		public static int Main(string[] args)
		{
			if (!DataAccessHelper.StandardArgsCheck(args, "Noble Phone Invalidation"))
				return 1;

			DataAccessHelper.CurrentRegion = DataAccessHelper.Region.None;
			new NoblePhoneInvalidation().Process();
			return 0;
		}

		/// <summary>
		/// Main processing method.  Spawns up 3 threads to process each region.
		/// </summary>
		public void Process()
		{
			LogData = ProcessLogger.RegisterApplication(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(),false);
#if DEBUG
			List<NobleData> batchRecordsCornerStoneCompass = DataAccess.LoadNobleData(DataAccess.Region.CornerStoneCompass, LogData);
			List<NobleData> batchRecordsOneLink = DataAccess.LoadNobleData(DataAccess.Region.OneLink, LogData);
			List<NobleData> batchRecordsUheaaCompass = DataAccess.LoadNobleData(DataAccess.Region.UheaaCompass, LogData);
			foreach (NobleData record in batchRecordsCornerStoneCompass)
			{
				File.AppendAllText(@"T:\CornerStoneCompassNobleData.txt", record.ToString());
				File.AppendAllText(@"T:\CornerStoneCompassNobleData.txt", "\r\n");	
			}
			foreach (NobleData record in batchRecordsUheaaCompass)
			{
				File.AppendAllText(@"T:\UheaaCompassNobleData.txt", record.ToString());
				File.AppendAllText(@"T:\UheaaCompassNobleData.txt", "\r\n");	
			}
			foreach (NobleData record in batchRecordsOneLink)
			{
				File.AppendAllText(@"T:\OneLinkNobleData.txt", record.ToString());
				File.AppendAllText(@"T:\OneLinkNobleData.txt", "\r\n");	
			}
			Console.ReadKey();
#endif
			var oneLink = Task.Factory.StartNew(() => ProcessOneLink());
		    var uheaaCompass = Task.Factory.StartNew(() => ProcessUheaaCompass());
			var cornerStoneCompass = Task.Factory.StartNew(() => ProcessCornerStoneCompass());
			Task.WhenAll(oneLink,uheaaCompass,cornerStoneCompass).Wait();
			ProcessLogger.LogEnd(LogData.ProcessLogId);
		}

		/// <summary>
		/// This method opens a connection to a reflection interface, Loads up the CornerStone Call data, and calls InvalidatePhoneNumber.
		/// </summary>
		private void ProcessCornerStoneCompass()
		{
			BatchProcessingHelper cornerStoneCompassLogin = BatchProcessingHelper.GetNextAvailableId(ScriptId,"BatchCornerstone");
			CompassInvalidator cornerStoneCompass = new CompassInvalidator(LogData);
			//used to make sure logins are done individually
			lock (locker)
			{
				if (!cornerStoneCompass.Login(cornerStoneCompassLogin, DataAccessHelper.Region.CornerStone))
				{
					ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Login to CornerStone Failed using ID: {0} Closing Session.", cornerStoneCompassLogin.UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical);
					cornerStoneCompass.RI.CloseSession();
					return;
				}
#if DEBUG
				Console.WriteLine("Logging in to Cornerstone Compass");
				Console.ReadKey();
#endif
			}
			List<NobleData> batchRecordsCornerStoneCompass = DataAccess.LoadNobleData(DataAccess.Region.CornerStoneCompass, LogData);
			foreach (NobleData record in batchRecordsCornerStoneCompass)
			{
				StringHelper.Sanitize(record, true, false);
				cornerStoneCompass.InvalidatePhoneNumber(record);
			}
			cornerStoneCompass.RI.CloseSession();
			cornerStoneCompass.ErrorReportCornerStone.Publish();
		}

		/// <summary>
		/// This method opens a connection to a reflection interface, Loads up the OneLink Call data, and calls InvalidatePhoneNumber.
		/// </summary>
		private void ProcessOneLink()
		{
			BatchProcessingHelper oneLinkLogin = BatchProcessingHelper.GetNextAvailableId(ScriptId, "BatchUheaa");
			Console.WriteLine(string.Format("Attempting to login to Onelink session using userID {0}", oneLinkLogin.UserName));
			OneLinkInvalidator oneLink = new OneLinkInvalidator(LogData);
			//used to make sure logins are done individually
			lock (locker)
			{
				if (!oneLink.Login(oneLinkLogin))
				{
					ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Login to Onelink Failed using ID: {0} Closing Session.", oneLinkLogin.UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical);
					oneLink.RI.CloseSession();
					return;
				}
#if DEBUG
				Console.WriteLine("Logging in to OneLink");
				Console.ReadKey();
#endif
			}
			List<NobleData> batchRecordsOneLink = DataAccess.LoadNobleData(DataAccess.Region.OneLink, LogData);
			foreach (NobleData record in batchRecordsOneLink)
			{
				StringHelper.Sanitize(record, true, false);
				oneLink.InvalidatePhoneNumber(record);
			}
			oneLink.RI.CloseSession();
			oneLink.ErrorReportOneLink.Publish();
		}

		/// <summary>
		/// This method opens a connection to a reflection interface, Loads up the Uheaa Compass Call data, and calls InvalidatePhoneNumber.
		/// </summary>
		private void ProcessUheaaCompass()
		{
			BatchProcessingHelper uheaaCompassLogin = BatchProcessingHelper.GetNextAvailableId(ScriptId, "BatchUheaa");
			CompassInvalidator uheaaCompass = new CompassInvalidator(LogData);
			//used to make sure logins are done individually
			lock (locker)
			{
				if (!uheaaCompass.Login(uheaaCompassLogin, DataAccessHelper.Region.Uheaa))
				{
					ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Login to Uheaa Compass Failed using ID: {0} Closing Session.", uheaaCompassLogin.UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical);
					uheaaCompass.RI.CloseSession();
					return;
				}
#if DEBUG
				Console.WriteLine("Logging in to Uheaa Compass");
				Console.ReadKey();
#endif
			}
			List<NobleData> batchRecordsUheaaCompass = DataAccess.LoadNobleData(DataAccess.Region.UheaaCompass, LogData);
			foreach (NobleData record in batchRecordsUheaaCompass)
			{
				StringHelper.Sanitize(record, true, false);
				uheaaCompass.InvalidatePhoneNumber(record);
			}
			uheaaCompass.RI.CloseSession();
			uheaaCompass.ErrorReportUheaa.Publish();
		}
	}
}
