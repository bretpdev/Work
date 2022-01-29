using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace INCIDENTRP
{
	partial class DataAccess
	{
		[UsesSproc(IncidentReportingUheaa, "spDeleteAccessControl")]
		public void DeleteAccessControlIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteAccessControl", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteDataEntry")]
		public void DeleteDataEntryIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteDataEntry", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteDisposalOrDestruction")]
		public void DeleteDisposalOrDestructionIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteDisposalOrDestruction", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteElectronicMailDelivery")]
		public void DeleteElectronicMailDeliveryIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteElectronicMailDelivery", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteFax")]
		public void DeleteFaxIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteFax", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteOddComputerBehavior")]
		public void DeleteOddComputerBehaviorIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteOddComputerBehavior", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeletePhysicalDamageLossTheft")]
		public void DeletePhysicalDamageLossOrTheftIncident(long ticketNumber)
		{
			LDA.Execute("spDeletePhysicalDamageLossTheft", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteRegularMailDelivery")]
		public void DeleteRegularMailDeliveryIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteRegularMailDelivery", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteScansProbes")]
		public void DeleteScanOrProbeIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteScansProbes", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteSystemOrNetworkUnavailable")]
		public void DeleteSystemOrNetworkUnavailableIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteSystemOrNetworkUnavailable", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteTelephone")]
		public void DeleteTelephoneIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteTelephone", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteUnauthorizedPhysicalAccess")]
		public void DeleteUnauthorizedPhysicalAccessIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteUnauthorizedPhysicalAccess", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteUnauthorizedSystemAccess")]
		public void DeleteUnauthorizedSystemAccessIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteUnauthorizedSystemAccess", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteViolationOfAcceptableUse")]
		public void DeleteViolationOfAcceptableUseIncident(long ticketNumber)
		{
			LDA.Execute("spDeleteViolationOfAcceptableUse", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spGetAccessControl")]
		public AccessControlIncident LoadAccessControlIncident(long ticketNumber)
		{
			return LDA.ExecuteList<AccessControlIncident>("spGetAccessControl", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetDataEntry")]
		public DataEntryIncident LoadDataEntryIncident(long ticketNumber)
		{
			return LDA.ExecuteList<DataEntryIncident>("spGetDataEntry", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetDisposalDestruction")]
		public DisposalOrDestructionIncident LoadDisposalOrDestructionIncident(long ticketNumber)
		{
			return LDA.ExecuteList<DisposalOrDestructionIncident>("spGetDisposalDestruction", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetElectronicMailDelivery")]
		public ElectronicMailDeliveryIncident LoadElectronicMailDeliveryIncident(long ticketNumber)
		{
			return LDA.ExecuteList<ElectronicMailDeliveryIncident>("spGetElectronicMailDelivery", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetFax")]
		public FaxIncident LoadFaxIncident(long ticketNumber)
		{
			return LDA.ExecuteList<FaxIncident>("spGetFax", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetOddComputerBehavior")]
		public OddComputerBehaviorIncident LoadOddComputerBehaviorIncident(long ticketNumber)
		{
			return LDA.ExecuteList<OddComputerBehaviorIncident>("spGetOddComputerBehavior", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetPhysicalDamageLossTheft")]
		public PhysicalDamageLossOrTheftIncident LoadPhysicalDamageLossOrTheftIncident(long ticketNumber)
		{
			return LDA.ExecuteList<PhysicalDamageLossOrTheftIncident>("spGetPhysicalDamageLossTheft", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetRegularMailDelivery")]
		public RegularMailDeliveryIncident LoadRegularMailDeliveryIncident(long ticketNumber)
		{
			return LDA.ExecuteList<RegularMailDeliveryIncident>("spGetRegularMailDelivery", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetScansProbes")]
		public ScanOrProbeIncident LoadScanOrProbeIncident(long ticketNumber)
		{
			return LDA.ExecuteList<ScanOrProbeIncident>("spGetScansProbes", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetSystemOrNetworkUnavailable")]
		public SystemOrNetworkUnavailableIncident LoadSystemOrNetworkUnavailableIncident(long ticketNumber)
		{
			return LDA.ExecuteList<SystemOrNetworkUnavailableIncident>("spGetSystemOrNetworkUnavailable", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetTelephone")]
		public TelephoneIncident LoadTelephoneIncident(long ticketNumber)
		{
			return LDA.ExecuteList<TelephoneIncident>("spGetTelephone", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetUnauthorizedPhysicalAccess")]
		public UnauthorizedPhysicalAccessIncident LoadUnauthorizedPhysicalAccessIncident(long ticketNumber)
		{
			return LDA.ExecuteList<UnauthorizedPhysicalAccessIncident>("spGetUnauthorizedPhysicalAccess", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetUnauthorizedSystemAccess")]
		public UnauthorizedSystemAccessIncident LoadUnauthorizedSystemAccessIncident(long ticketNumber)
		{
			return LDA.ExecuteList<UnauthorizedSystemAccessIncident>("spGetUnauthorizedSystemAccess", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetViolationOfAcceptableUse")]
		public ViolationOfAcceptableUseIncident LoadViolationOfAcceptableUseIncident(long ticketNumber)
		{
			return LDA.ExecuteList<ViolationOfAcceptableUseIncident>("spGetViolationOfAcceptableUse", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber)).Result.SingleOrDefault();
		}

		[UsesSproc(IncidentReportingUheaa, "spSetAccessControl")]
		public void SaveAccessControlIncident(AccessControlIncident accessControl, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("ImproperAccessWasGranted", accessControl.ImproperAccessWasGranted));
			parms.Add(new SqlParameter("SystemAccessWasNotTerminatedOrModified", accessControl.SystemAccessWasNotTerminatedOrModified));
			parms.Add(new SqlParameter("PhysicalAccessWasNotTerminatedOrModified", accessControl.PhysicalAccessWasNotTerminatedOrModified));
			LDA.Execute("spSetAccessControl", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetDataEntry")]
		public void SaveDataEntryIncident(DataEntryIncident dataEntry, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("IncorrectInformationWasAdded", dataEntry.IncorrectInformationWasAdded));
			parms.Add(new SqlParameter("InformationWasIncorrectlyChanged", dataEntry.InformationWasIncorrectlyChanged));
			parms.Add(new SqlParameter("InformationWasIncorrectlyDeleted", dataEntry.InformationWasIncorrectlyDeleted));
			LDA.Execute("spSetDataEntry", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetDisposalOrDestruction")]
		public void SaveDisposalOrDestructionIncident(DisposalOrDestructionIncident disposal, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("ElectronicMediaRecordsWereDestroyedInError", disposal.ElectronicMediaRecordsWereDestroyedInError));
			parms.Add(new SqlParameter("ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod", disposal.ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod));
			parms.Add(new SqlParameter("MicrofilmWithRecordsWasDestroyedInError", disposal.MicrofilmWithRecordsWasDestroyedInError));
			parms.Add(new SqlParameter("MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod", disposal.MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod));
			parms.Add(new SqlParameter("PaperRecordsWereDestroyedInError", disposal.PaperRecordsWereDestroyedInError));
			parms.Add(new SqlParameter("PaperRecordsWereDestroyedUsingIncorrectMethod", disposal.PaperRecordsWereDestroyedUsingIncorrectMethod));
			LDA.Execute("spSetDisposalOrDestruction", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetElectronicMailDelivery")]
		public void SaveElectronicMailDeliveryIncident(ElectronicMailDeliveryIncident email, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("EmailAddressWasDisclosed", email.EmailAddressWasDisclosed));
			parms.Add(new SqlParameter("FtpTransmissionWasSentToIncorrectDestination", email.FtpTransmissionWasSentToIncorrectDestination));
			parms.Add(new SqlParameter("IncorrectAttachmentContainedPii", email.IncorrectAttachmentContainedPii));
			parms.Add(new SqlParameter("EmailWasDeliveredToIncorrectAddress", email.EmailWasDeliveredToIncorrectAddress));
			parms.Add(new SqlParameter("Detail", email.Detail));
			LDA.Execute("spSetElectronicMailDelivery", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetFax")]
		public void SaveFaxIncident(FaxIncident fax, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("FaxNumber", fax.FaxNumber));
			parms.Add(new SqlParameter("Recipient", fax.Recipient));
			parms.Add(new SqlParameter("IncorrectDocumentsWereFaxed", fax.IncorrectDocumentsWereFaxed));
			parms.Add(new SqlParameter("FaxNumberWasIncorrect", fax.FaxNumberWasIncorrect));
			LDA.Execute("spSetFax", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetOddComputerBehavior")]
		public void SaveOddComputerBehaviorIncident(OddComputerBehaviorIncident behavior, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("EmailPhishingOrHoax", behavior.EmailPhishingOrHoax));
			parms.Add(new SqlParameter("DenialOfService", behavior.DenialOfService));
			parms.Add(new SqlParameter("UnexplainedAttemptToWriteToSystemFiles", behavior.UnexplainedAttemptToWriteToSystemFiles));
			parms.Add(new SqlParameter("UnexplainedModificationOrDeletionOfDate", behavior.UnexplainedModificationOrDeletionOfDate));
			parms.Add(new SqlParameter("UnexplainedModificationToFileLengthOrDate", behavior.UnexplainedModificationToFileLengthOrDate));
			parms.Add(new SqlParameter("UnexplainedNewFilesOrUnfamiliarFileNames", behavior.UnexplainedNewFilesOrUnfamiliarFileNames));
			parms.Add(new SqlParameter("Malware", behavior.Malware));
			LDA.Execute("spSetOddComputerBehavior", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetPhysicalDamageLossTheft")]
		public void SavePhysicalDamageLossOrTheftIncident(PhysicalDamageLossOrTheftIncident physical, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("DataWasEncrypted", physical.DataWasEncrypted));
			parms.Add(new SqlParameter("DesktopWasDamaged", physical.DesktopWasDamaged));
			parms.Add(new SqlParameter("DesktopWasLost", physical.DesktopWasLost));
			parms.Add(new SqlParameter("DesktopWasStolen", physical.DesktopWasStolen));
			parms.Add(new SqlParameter("LaptopWasDamaged", physical.LaptopWasDamaged));
			parms.Add(new SqlParameter("LaptopWasLost", physical.LaptopWasLost));
			parms.Add(new SqlParameter("LaptopWasStolen", physical.LaptopWasStolen));
			parms.Add(new SqlParameter("MicrofilmWithRecordsContainingPiiWasLost", physical.MicrofilmWithRecordsContainingPiiWasLost));
			parms.Add(new SqlParameter("MicrofilmWithRecordsContainingPiiWasStolen", physical.MicrofilmWithRecordsContainingPiiWasStolen));
			parms.Add(new SqlParameter("MobileCommunicationDeviceWasLost", physical.MobileComunicationDeviceWasLost));
			parms.Add(new SqlParameter("MobileCommunicationDeviceWasStolen", physical.MobileComunicationDeviceWasStolen));
			parms.Add(new SqlParameter("PaperRecordWithPiiWasLost", physical.PaperRecordWithPiiWasLost));
			parms.Add(new SqlParameter("PaperRecordWithPiiWasStolen", physical.PaperRecordWithPiiWasStolen));
			parms.Add(new SqlParameter("RemovableMediaWithPiiWasLost", physical.RemovableMediaWithPiiWasLost));
			parms.Add(new SqlParameter("RemovableMediaWithPiiWasStolen", physical.RemovableMediaWithPiiWasStolen));
			parms.Add(new SqlParameter("WindowOrDoorWasDamaged", physical.WindowOrDoorWasDamaged));
			LDA.Execute("spSetPhysicalDamageLossTheft", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetRegularMailDelivery")]
		public void SaveRegularMailDeliveryIncident(RegularMailDeliveryIncident mail, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Problem", mail.Problem));
			parms.Add(new SqlParameter("Address1", mail.Address1));
			parms.Add(new SqlParameter("Address2", mail.Address2));
			parms.Add(new SqlParameter("City", mail.City));
			parms.Add(new SqlParameter("State", mail.State));
			parms.Add(new SqlParameter("Zip", mail.Zip));
			parms.Add(new SqlParameter("TrackingNumber", mail.TrackingNumber));
			LDA.Execute("spSetRegularMailDelivery", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetScansProbes")]
		public void SaveScanOrProbeIncident(ScanOrProbeIncident scan, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("UnauthorizedProgramOrSnifferDevice", scan.UnauthorizedProgramOrSnifferDevice));
			parms.Add(new SqlParameter("PrioritySystemAlarmOrIndicationFromIds", scan.PrioritySystemAlarmOrIndicationFromIds));
			parms.Add(new SqlParameter("UnauthorizedPortScan", scan.UnauthorizedPortScan));
			parms.Add(new SqlParameter("UnauthorizedVulnerabilityScan", scan.UnauthroizedVulnerabilityScan));
			LDA.Execute("spSetScansProbes", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetSystemOrNetworkUnavailable")]
		public void SaveSystemOrNetworkUnavailableIncident(SystemOrNetworkUnavailableIncident unavailable, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("DenialOrDisruptionOfService", unavailable.DenialOrDisruptionOfService));
			parms.Add(new SqlParameter("UnableToLogIntoAccount", unavailable.UnableToLogIntoAccount));
			LDA.Execute("spSetSystemOrNetworkUnavailable", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetTelephone")]
		public void SaveTelephoneIncident(TelephoneIncident phone, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("RevealedInformationOnVoicemail", phone.RevealedInformationOnVoicemail));
			parms.Add(new SqlParameter("RevealedInformationToUnauthorizedIndividual", phone.RevealedInformationToUnauthorizedIndividual));
			parms.Add(new SqlParameter("UnauthorizedIndividual", phone.UnauthorizedIndividual));
			LDA.Execute("spSetTelephone", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetUnauthorizedPhysicalAccess")]
		public void SaveUnauthorizedPhysicalAccessIncident(UnauthorizedPhysicalAccessIncident physical, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("AccessAccountingDiscrepancy", physical.AccessAccountingDiscrepancy));
			parms.Add(new SqlParameter("BuildingBreakIn", physical.BuildingBreakIn));
			parms.Add(new SqlParameter("Piggybacking", physical.Piggybacking));
			parms.Add(new SqlParameter("SuspiciousEntryInAccessLog", physical.SuspiciousEntryInAccessLog));
			parms.Add(new SqlParameter("SuspiciousEntryInVideoLog", physical.SuspiciousEntryInVideoLog));
			parms.Add(new SqlParameter("UnauthorizedUseOfKeycard", physical.UnauthorizedUseOfKeycard));
			parms.Add(new SqlParameter("UnexplainedNewKeycard", physical.UnexplainedNewKeycard));
			parms.Add(new SqlParameter("UnusualTimeOfUsage", physical.UnusualTimeOfUsage));
			LDA.Execute("spSetUnauthorizedPhysicalAccess", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetUnauthorizedSystemAccess")]
		public void SaveUnauthorizedSystemAccessIncident(UnauthorizedSystemAccessIncident system, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("SuspiciousEntryInSystemOrNetworkLog", system.SuspiciousEntryInSystemOrNetworkLog));
			parms.Add(new SqlParameter("SystemAccountDiscrepancy", system.SystemAccountDiscrepancy));
			parms.Add(new SqlParameter("UnauthorizedUseOfUserCredentials", system.UnauthorizedUseOfUserCredentials));
			parms.Add(new SqlParameter("UnexplainedNewUserAccount", system.UnexplainedNewUserAccount));
			parms.Add(new SqlParameter("UnusualTimeOfUsage", system.UnusualTimeOfUsage));
			LDA.Execute("spSetUnauthorizedSystemAccess", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetViolationOfAcceptableUse")]
		public void SaveViolationOfAcceptableUseIncident(ViolationOfAcceptableUseIncident violation, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("AccessKeycardWasShared", violation.AccessKeycardWasShared));
			parms.Add(new SqlParameter("MisuseOfSystemResourcesByValidUser", violation.MisuseOfSystemResourcesByValidUser));
			parms.Add(new SqlParameter("UserSystemCredentialsWereShared", violation.UserSystemCredentialsWereShared));
			LDA.Execute("spSetViolationOfAcceptableUse", IncidentReportingUheaa, parms.ToArray());
		}
	}
}