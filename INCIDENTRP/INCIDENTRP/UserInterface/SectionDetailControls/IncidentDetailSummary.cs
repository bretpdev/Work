using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace INCIDENTRP
{
    partial class IncidentDetailSummary : BaseDetail
    {
        private Ticket Ticket { get; set; }

        public IncidentDetailSummary(Ticket ticket)
        {
            InitializeComponent();
            Ticket = ticket;
            ShowSummary();
        }

        public override void CheckValidity()
        {
            bool isValid = true;
            if (isValid != _isValidated)
            {
                _isValidated = isValid;
            }

            //This method happens to be called at a convenient point for updating the summary, so do that here.
            btnSubmit.Visible = Ticket.Incident.IsComplete();
            ShowSummary();
        }

        private void ShowSummary()
        {
            pnlSummary.Controls.Clear();
            pnlSummary.Controls.AddRange(GetIncidentReporterSummaryItems());
            pnlSummary.Controls.Add(new SummaryItem());
            pnlSummary.Controls.AddRange(GetIncidentDetailSummaryItems());
            pnlSummary.Controls.Add(new SummaryItem());
            pnlSummary.Controls.AddRange(GetIncidentTypeSummaryItems());
            pnlSummary.Controls.Add(new SummaryItem());
            pnlSummary.Controls.AddRange(GetDataInvolvedSummaryItems());
            pnlSummary.Controls.Add(new SummaryItem());
            pnlSummary.Controls.AddRange(GetCauseSummaryItems());
            pnlSummary.Controls.Add(new SummaryItem());
            pnlSummary.Controls.AddRange(GetNarrativeOfIncidentSummaryItems());
            pnlSummary.Controls.Add(new SummaryItem());
            pnlSummary.Controls.AddRange(GetActionsTakenSummaryItems());
        }


        private Control[] GetIncidentReporterSummaryItems()
        {
            List<Control> summaryItems = new List<Control>();
            summaryItems.Add(new SummaryItem("Incident Reporter", true, true));
            string reporterName = "";
            string reporterEmailAddress = "";
            if (Ticket.Incident.Reporter.User != null)
            {
                reporterName = Ticket.Incident.Reporter.User.ToString();
                reporterEmailAddress = Ticket.Incident.Reporter.User.EmailAddress;
            }
            summaryItems.Add(new SummaryItem("* Name: " + reporterName, false, !string.IsNullOrEmpty(reporterName)));
            string reporterBusinessUnit = "";
            if (Ticket.Incident.Reporter.BusinessUnit != null)
                reporterBusinessUnit = Ticket.Incident.Reporter.BusinessUnit.Name;
            summaryItems.Add(new SummaryItem("* Business Unit: " + reporterBusinessUnit, false, !string.IsNullOrEmpty(reporterBusinessUnit)));
            summaryItems.Add(new SummaryItem("* E-mail Address: " + reporterEmailAddress, false, !string.IsNullOrEmpty(reporterEmailAddress)));
            summaryItems.Add(new SummaryItem("* Phone Number: " + Ticket.Incident.Reporter.PhoneNumber, false, !string.IsNullOrEmpty(Ticket.Incident.Reporter.PhoneNumber)));
            summaryItems.Add(new SummaryItem("* Location: " + Ticket.Incident.Reporter.Location, false, true));
            summaryItems.Add(new SummaryItem("* Priority: " + Ticket.Incident.Priority, false, true));
            return summaryItems.ToArray();
        }

        private Control[] GetIncidentDetailSummaryItems()
        {
            List<Control> summaryItems = new List<Control>();
            summaryItems.Add(new SummaryItem("Incident Detail", true, true));
            summaryItems.Add(new SummaryItem("* Functional Area: " + Ticket.FunctionalArea, false, true));
            summaryItems.Add(new SummaryItem("* Incident Date: " + Ticket.IncidentDateTime.ToString("MM/dd/yyyy"), false, true));
            summaryItems.Add(new SummaryItem("* Incident Time: " + Ticket.IncidentDateTime.ToString("hh:mm tt"), false, true));
            summaryItems.Add(new SummaryItem("* Physical Location of Incident: " + Ticket.Incident.Location, false, true));
            string method = Ticket.Incident.Notifier.Method;
            if (method == Notifier.OTHER)
                method += $" ({Ticket.Incident.Notifier.OtherMethod})";
            summaryItems.Add(new SummaryItem("* Notifier Method: " + method, false, !string.IsNullOrEmpty(Ticket.Incident.Notifier.Method)));
            string type = Ticket.Incident.Notifier.Type;
            if (type == Notifier.OTHER)
                type += $" ({Ticket.Incident.Notifier.OtherType})";
            summaryItems.Add(new SummaryItem("* Notified By: " + type, false, !string.IsNullOrEmpty(Ticket.Incident.Notifier.Type)));
            summaryItems.Add(new SummaryItem("* Notifier Name: " + Ticket.Incident.Notifier.Name, false, !string.IsNullOrEmpty(Ticket.Incident.Notifier.Name)));
            summaryItems.Add(new SummaryItem("* Notifier E-mail Address: " + Ticket.Incident.Notifier.EmailAddress, false, true));
            summaryItems.Add(new SummaryItem("* Notifier Phone Number: " + Ticket.Incident.Notifier.PhoneNumber, false, !string.IsNullOrEmpty(Ticket.Incident.Notifier.PhoneNumber)));
            string relationship = Ticket.Incident.Notifier.Relationship;
            if (relationship == Notifier.OTHER)
                relationship += $" ({Ticket.Incident.Notifier.OtherRelationship})";
            summaryItems.Add(new SummaryItem("* Notifier Relationship: " + relationship, false, true));
            return summaryItems.ToArray();
        }

        private Control[] GetIncidentTypeSummaryItems()
        {
            List<Control> summaryItems = new List<Control>();
            summaryItems.Add(new SummaryItem("Incident Type", true, true));
            summaryItems.Add(new SummaryItem("* Incident Type: " + Ticket.Incident.Type, false, true));
            switch (Ticket.Incident.Type)
            {
                case Incident.ACCESS_CONTROL:
                    if (Ticket.Incident.AccessControlIncident.ImproperAccessWasGranted)
                        summaryItems.Add(new SummaryItem("* Improper Access Granted", false, true));
                    if (Ticket.Incident.AccessControlIncident.SystemAccessWasNotTerminatedOrModified)
                        summaryItems.Add(new SummaryItem("* System Access NOT Terminated or Modified", false, true));
                    if (Ticket.Incident.AccessControlIncident.PhysicalAccessWasNotTerminatedOrModified)
                        summaryItems.Add(new SummaryItem("* Physical Access NOT Terminated or Modified", false, true));
                    break;
                case Incident.DATA_ENTRY:
                    if (Ticket.Incident.DataEntryIncident.IncorrectInformationWasAdded)
                        summaryItems.Add(new SummaryItem("* Incorrect Information Added", false, true));
                    if (Ticket.Incident.DataEntryIncident.InformationWasIncorrectlyChanged)
                        summaryItems.Add(new SummaryItem("* Information Incorrectly Changed", false, true));
                    if (Ticket.Incident.DataEntryIncident.InformationWasIncorrectlyDeleted)
                        summaryItems.Add(new SummaryItem("* Information Incorrectly Deleted", false, true));
                    break;
                case Incident.DISPOSAL_DESTRUCTION:
                    if (Ticket.Incident.DisposalOrDestructionIncident.ElectronicMediaRecordsWereDestroyedInError)
                        summaryItems.Add(new SummaryItem("* Electronic Media Records Destroyed in Error", false, true));
                    if (Ticket.Incident.DisposalOrDestructionIncident.ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod)
                        summaryItems.Add(new SummaryItem("* Electronic Media Records Destroyed Using Incorrect Method/Technique", false, true));
                    if (Ticket.Incident.DisposalOrDestructionIncident.MicrofilmWithRecordsWasDestroyedInError)
                        summaryItems.Add(new SummaryItem("* Microfilm with Records Destroyed in Error", false, true));
                    if (Ticket.Incident.DisposalOrDestructionIncident.MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod)
                        summaryItems.Add(new SummaryItem("* Microfilm with Records Destroyed Using Incorrect Method/Technique", false, true));
                    if (Ticket.Incident.DisposalOrDestructionIncident.PaperRecordsWereDestroyedInError)
                        summaryItems.Add(new SummaryItem("* Paper Records Destroyed in Error", false, true));
                    if (Ticket.Incident.DisposalOrDestructionIncident.PaperRecordsWereDestroyedUsingIncorrectMethod)
                        summaryItems.Add(new SummaryItem("* Paper Records Destroyed Using Incorrect Method/Technique", false, true));
                    break;
                case Incident.ELECTRONIC_MAIL_DELIVERY:
                    bool emailDetailIsValid = !string.IsNullOrEmpty(Ticket.Incident.ElectronicMailDeliveryIncident.Detail);
                    if (Ticket.Incident.ElectronicMailDeliveryIncident.EmailAddressWasDisclosed)
                    {
                        summaryItems.Add(new SummaryItem("* E-mail Addresses of Others Disclosed", false, true));
                        summaryItems.Add(new SummaryItem("* Addresses Disclosed: " + Ticket.Incident.ElectronicMailDeliveryIncident.Detail, false, emailDetailIsValid));
                    }
                    if (Ticket.Incident.ElectronicMailDeliveryIncident.FtpTransmissionWasSentToIncorrectDestination)
                    {
                        summaryItems.Add(new SummaryItem("* FTP Transmission Sent to Incorrect Destination", false, true));
                        summaryItems.Add(new SummaryItem("* IP Address or DNS of FTP Transmission: " + Ticket.Incident.ElectronicMailDeliveryIncident.Detail, false, emailDetailIsValid));
                    }
                    if (Ticket.Incident.ElectronicMailDeliveryIncident.IncorrectAttachmentContainedPii)
                    {
                        summaryItems.Add(new SummaryItem("* Incorrect Attachment Containing PII", false, true));
                        summaryItems.Add(new SummaryItem("* E-mail Address: " + Ticket.Incident.ElectronicMailDeliveryIncident.Detail, false, emailDetailIsValid));
                    }
                    if (Ticket.Incident.ElectronicMailDeliveryIncident.EmailWasDeliveredToIncorrectAddress)
                    {
                        summaryItems.Add(new SummaryItem("* Incorrect E-mail Address", false, true));
                        summaryItems.Add(new SummaryItem("* E-mail Address: " + Ticket.Incident.ElectronicMailDeliveryIncident.Detail, false, emailDetailIsValid));
                    }
                    break;
                case Incident.FAX:
                    summaryItems.Add(new SummaryItem("* Fax #: " + Ticket.Incident.FaxIncident.FaxNumber, false, !string.IsNullOrEmpty(Ticket.Incident.FaxIncident.FaxNumber)));
                    summaryItems.Add(new SummaryItem("* Recipient: " + Ticket.Incident.FaxIncident.Recipient, false, !string.IsNullOrEmpty(Ticket.Incident.FaxIncident.Recipient)));
                    if (Ticket.Incident.FaxIncident.IncorrectDocumentsWereFaxed)
                        summaryItems.Add(new SummaryItem("* Incorrect Documents Faxed", false, true));
                    if (Ticket.Incident.FaxIncident.FaxNumberWasIncorrect)
                        summaryItems.Add(new SummaryItem("* Incorrect Fax Number", false, true));
                    break;
                case Incident.ODD_COMPUTER_BEHAVIOR:
                    if (Ticket.Incident.OddComputerBehaviorIncident.EmailPhishingOrHoax)
                        summaryItems.Add(new SummaryItem("* E-mail Phishing/Hoax", false, true));
                    if (Ticket.Incident.OddComputerBehaviorIncident.DenialOfService)
                        summaryItems.Add(new SummaryItem("* Participation in Denial of Service Attack", false, true));
                    if (Ticket.Incident.OddComputerBehaviorIncident.UnexplainedAttemptToWriteToSystemFiles)
                        summaryItems.Add(new SummaryItem("* Unexplained Attempt to Write to System Files or Changes in System Files", false, true));
                    if (Ticket.Incident.OddComputerBehaviorIncident.UnexplainedModificationOrDeletionOfDate)
                        summaryItems.Add(new SummaryItem("* Unexplained Modification or Deletion of Date", false, true));
                    if (Ticket.Incident.OddComputerBehaviorIncident.UnexplainedModificationToFileLengthOrDate)
                        summaryItems.Add(new SummaryItem("* Unexplained Modifications to File Lengths and/or Dates", false, true));
                    if (Ticket.Incident.OddComputerBehaviorIncident.UnexplainedNewFilesOrUnfamiliarFileNames)
                        summaryItems.Add(new SummaryItem("* Unexplained New Files or Unfamiliar File Names", false, true));
                    if (Ticket.Incident.OddComputerBehaviorIncident.Malware)
                        summaryItems.Add(new SummaryItem("* Virus/Worm/Malware", false, true));
                    break;
                case Incident.PHYSICAL_DAMAGE_LOSS_THEFT:
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DesktopWasDamaged)
                    {
                        summaryItems.Add(new SummaryItem("* Desktop Damaged", false, true));
                        if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DataWasEncrypted)
                            summaryItems.Add(new SummaryItem("* Data Was Encrypted", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Data Was NOT Encrypted", false, true));
                    }
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DesktopWasLost)
                    {
                        summaryItems.Add(new SummaryItem("* Desktop Lost", false, true));
                        if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DataWasEncrypted)
                            summaryItems.Add(new SummaryItem("* Data Was Encrypted", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Data Was NOT Encrypted", false, true));
                    }
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DesktopWasStolen)
                    {
                        summaryItems.Add(new SummaryItem("* Desktop Stolen", false, true));
                        if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DataWasEncrypted)
                            summaryItems.Add(new SummaryItem("* Data Was Encrypted", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Data Was NOT Encrypted", false, true));
                    }
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.LaptopWasDamaged)
                    {
                        summaryItems.Add(new SummaryItem("* Laptop Damaged", false, true));
                        if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DataWasEncrypted)
                            summaryItems.Add(new SummaryItem("* Data Was Encrypted", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Data Was NOT Encrypted", false, true));
                    }
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.LaptopWasLost)
                    {
                        summaryItems.Add(new SummaryItem("* Laptop Lost", false, true));
                        if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DataWasEncrypted)
                            summaryItems.Add(new SummaryItem("* Data Was Encrypted", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Data Was NOT Encrypted", false, true));
                    }
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.LaptopWasStolen)
                    {
                        summaryItems.Add(new SummaryItem("* Laptop Stolen", false, true));
                        if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DataWasEncrypted)
                            summaryItems.Add(new SummaryItem("* Data Was Encrypted", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Data Was NOT Encrypted", false, true));
                    }
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.MicrofilmWithRecordsContainingPiiWasLost)
                        summaryItems.Add(new SummaryItem("* Microfilm with Records Containing PII Lost", false, true));
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.MicrofilmWithRecordsContainingPiiWasStolen)
                        summaryItems.Add(new SummaryItem("* Microfilm with Records Containing PII Stolen", false, true));
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.MobileComunicationDeviceWasLost)
                    {
                        summaryItems.Add(new SummaryItem("* Mobile Communication Device Lost", false, true));
                        if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DataWasEncrypted)
                            summaryItems.Add(new SummaryItem("* Data Was Encrypted", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Data Was NOT Encrypted", false, true));
                    }
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.MobileComunicationDeviceWasStolen)
                    {
                        summaryItems.Add(new SummaryItem("* Mobile Communication Device Stolen", false, true));
                        if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DataWasEncrypted)
                            summaryItems.Add(new SummaryItem("* Data Was Encrypted", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Data Was NOT Encrypted", false, true));
                    }
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.PaperRecordWithPiiWasLost)
                        summaryItems.Add(new SummaryItem("* Paper Record with PII Lost", false, true));
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.PaperRecordWithPiiWasStolen)
                        summaryItems.Add(new SummaryItem("* Paper Record with PII Stolen", false, true));
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.RemovableMediaWithPiiWasLost)
                    {
                        summaryItems.Add(new SummaryItem("* Removable Electronic Media with PII Lost", false, true));
                        if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DataWasEncrypted)
                            summaryItems.Add(new SummaryItem("* Data Was Encrypted", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Data Was NOT Encrypted", false, true));
                    }
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.RemovableMediaWithPiiWasStolen)
                    {
                        summaryItems.Add(new SummaryItem("* Removable Electronic Media with PII Stolen", false, true));
                        if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.DataWasEncrypted)
                            summaryItems.Add(new SummaryItem("* Data Was Encrypted", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Data Was NOT Encrypted", false, true));
                    }
                    if (Ticket.Incident.PhysicalDamageLossOrTheftIncident.WindowOrDoorWasDamaged)
                        summaryItems.Add(new SummaryItem("* Window/Door Damage", false, true));
                    break;
                case Incident.REGULAR_MAIL_DELIVERY:
                    if (!string.IsNullOrEmpty(Ticket.Incident.RegularMailDeliveryIncident.Problem))
                    {
                        if (Ticket.Incident.RegularMailDeliveryIncident.Problem == RegularMailDeliveryIncident.INCORRECT_ADDRESS)
                            summaryItems.Add(new SummaryItem("* Mail Delivered to Incorrect Address--Opened", false, true));
                        else
                            summaryItems.Add(new SummaryItem("* Incorrect Documents Contained in Envelope/Package and Opened", false, true));
                        summaryItems.Add(new SummaryItem("* Address 1: " + Ticket.Incident.RegularMailDeliveryIncident.Address1, false, !string.IsNullOrEmpty(Ticket.Incident.RegularMailDeliveryIncident.Address1)));
                        summaryItems.Add(new SummaryItem("* Address 2: " + Ticket.Incident.RegularMailDeliveryIncident.Address2, false, true));
                        summaryItems.Add(new SummaryItem("* City: " + Ticket.Incident.RegularMailDeliveryIncident.City, false, !string.IsNullOrEmpty(Ticket.Incident.RegularMailDeliveryIncident.City)));
                        summaryItems.Add(new SummaryItem("* State: " + Ticket.Incident.RegularMailDeliveryIncident.State, false, !string.IsNullOrEmpty(Ticket.Incident.RegularMailDeliveryIncident.State)));
                        summaryItems.Add(new SummaryItem("* ZIP: " + Ticket.Incident.RegularMailDeliveryIncident.Zip, false, !string.IsNullOrEmpty(Ticket.Incident.RegularMailDeliveryIncident.Zip)));
                        if (Ticket.Incident.RegularMailDeliveryIncident.Problem == RegularMailDeliveryIncident.INCORRECT_ADDRESS)
                            summaryItems.Add(new SummaryItem("* Tracking Number: " + Ticket.Incident.RegularMailDeliveryIncident.TrackingNumber, false, true));
                    }
                    break;
                case Incident.SCANS_PROBES:
                    if (Ticket.Incident.ScanOrProbeIncident.UnauthorizedProgramOrSnifferDevice)
                        summaryItems.Add(new SummaryItem("* Operation of an Unauthorized Program or Sniffer Device", false, true));
                    if (Ticket.Incident.ScanOrProbeIncident.PrioritySystemAlarmOrIndicationFromIds)
                        summaryItems.Add(new SummaryItem("* Priority System Alarm or Indication from IDS (Not a False Positive)", false, true));
                    if (Ticket.Incident.ScanOrProbeIncident.UnauthorizedPortScan)
                        summaryItems.Add(new SummaryItem("* Unauthorized Port Scanning", false, true));
                    if (Ticket.Incident.ScanOrProbeIncident.UnauthroizedVulnerabilityScan)
                        summaryItems.Add(new SummaryItem("* Unauthorized Vulnerability Scanning", false, true));
                    break;
                case Incident.SYSTEM_OR_NETWORK_UNAVAILABLE:
                    if (Ticket.Incident.SystemOrNetworkUnavailableIncident.DenialOrDisruptionOfService)
                        summaryItems.Add(new SummaryItem("* Denial/Disruption of Service", false, true));
                    if (Ticket.Incident.SystemOrNetworkUnavailableIncident.UnableToLogIntoAccount)
                        summaryItems.Add(new SummaryItem("* Inability to Log into an Account", false, true));
                    break;
                case Incident.TELEPHONE:
                    if (Ticket.Incident.TelephoneIncident.RevealedInformationOnVoicemail)
                        summaryItems.Add(new SummaryItem("* Revealed Information on Voicemail", false, true));
                    if (Ticket.Incident.TelephoneIncident.RevealedInformationToUnauthorizedIndividual)
                    {
                        summaryItems.Add(new SummaryItem("* Revealed Information to Unauthorized Individual", false, true));
                        summaryItems.Add(new SummaryItem("* Name of Unauthorized Individual: " + Ticket.Incident.TelephoneIncident.UnauthorizedIndividual, false, Ticket.Incident.TelephoneIncident.UnauthorizedIndividual.IsPopulated()));
                    }
                    break;
                case Incident.UNAUTHORIZED_PHYSICAL_ACCESS:
                    if (Ticket.Incident.UnauthorizedPhysicalAccessIncident.AccessAccountingDiscrepancy)
                        summaryItems.Add(new SummaryItem("* Access Accounting Discrepancies", false, true));
                    if (Ticket.Incident.UnauthorizedPhysicalAccessIncident.BuildingBreakIn)
                        summaryItems.Add(new SummaryItem("* Building Break-In/Trespassing", false, true));
                    if (Ticket.Incident.UnauthorizedPhysicalAccessIncident.Piggybacking)
                        summaryItems.Add(new SummaryItem("* Piggybacking/Tailgating", false, true));
                    if (Ticket.Incident.UnauthorizedPhysicalAccessIncident.SuspiciousEntryInAccessLog)
                        summaryItems.Add(new SummaryItem("* Suspicious Entries in Access Logs", false, true));
                    if (Ticket.Incident.UnauthorizedPhysicalAccessIncident.SuspiciousEntryInVideoLog)
                        summaryItems.Add(new SummaryItem("* Suspicious Entries in Video Logs", false, true));
                    if (Ticket.Incident.UnauthorizedPhysicalAccessIncident.UnauthorizedUseOfKeycard)
                        summaryItems.Add(new SummaryItem("* Unauthorized Use of Keycard", false, true));
                    if (Ticket.Incident.UnauthorizedPhysicalAccessIncident.UnexplainedNewKeycard)
                        summaryItems.Add(new SummaryItem("* Unexplained New Keycards", false, true));
                    if (Ticket.Incident.UnauthorizedPhysicalAccessIncident.UnusualTimeOfUsage)
                        summaryItems.Add(new SummaryItem("* Unusual Time of Usage", false, true));
                    break;
                case Incident.UNAUTHORIZED_SYSTEM_ACCESS:
                    if (Ticket.Incident.UnauthorizedSystemAccessIncident.SuspiciousEntryInSystemOrNetworkLog)
                        summaryItems.Add(new SummaryItem("* Suspicious Entries in System or Network Logs", false, true));
                    if (Ticket.Incident.UnauthorizedSystemAccessIncident.SystemAccountDiscrepancy)
                        summaryItems.Add(new SummaryItem("* System Accounting Discrepancies", false, true));
                    if (Ticket.Incident.UnauthorizedSystemAccessIncident.UnauthorizedUseOfUserCredentials)
                        summaryItems.Add(new SummaryItem("* Unauthorized Use of User Credentials", false, true));
                    if (Ticket.Incident.UnauthorizedSystemAccessIncident.UnexplainedNewUserAccount)
                        summaryItems.Add(new SummaryItem("* Unexplained New User Accounts", false, true));
                    if (Ticket.Incident.UnauthorizedSystemAccessIncident.UnusualTimeOfUsage)
                        summaryItems.Add(new SummaryItem("* Unusual Time of Usage", false, true));
                    break;
                case Incident.VIOLATION_OF_ACCEPTABLE_USE:
                    if (Ticket.Incident.ViolationOfAcceptableUseIncident.AccessKeycardWasShared)
                        summaryItems.Add(new SummaryItem("* Access Keycard Shared", false, true));
                    if (Ticket.Incident.ViolationOfAcceptableUseIncident.MisuseOfSystemResourcesByValidUser)
                        summaryItems.Add(new SummaryItem("* Misuse of System Resources by Valid Users", false, true));
                    if (Ticket.Incident.ViolationOfAcceptableUseIncident.UserSystemCredentialsWereShared)
                        summaryItems.Add(new SummaryItem("* User System Credentials Shared", false, true));
                    break;
            }
            return summaryItems.ToArray();
        }

        private Control[] GetDataInvolvedSummaryItems()
        {
            List<Control> summaryItems = new List<Control>();
            summaryItems.Add(new SummaryItem("Data Involved", true, true));
            if (!string.IsNullOrEmpty(Ticket.Incident.DataInvolved))
                summaryItems.Add(new SummaryItem("* Data Involved: " + Ticket.Incident.DataInvolved, false, true));
            switch (Ticket.Incident.DataInvolved)
            {
                case Incident.AGENCY_DATA:
                    if (Ticket.Incident.AgencyDataInvolved.AccountingOrAdministrativeRecordsWereReleased)
                        summaryItems.Add(new SummaryItem("* Accounting or Administrative Records", false, true));
                    if (Ticket.Incident.AgencyDataInvolved.ClosedSchoolRecordsWereReleased)
                        summaryItems.Add(new SummaryItem("* Closed School Records", false, true));
                    if (Ticket.Incident.AgencyDataInvolved.ConfidentialCaseFilesWereReleased)
                        summaryItems.Add(new SummaryItem("* Confidential Case Files", false, true));
                    if (Ticket.Incident.AgencyDataInvolved.ContractInformationWasReleased)
                        summaryItems.Add(new SummaryItem("* Contract Information", false, true));
                    if (Ticket.Incident.AgencyDataInvolved.OperationsReportsWereReleased)
                        summaryItems.Add(new SummaryItem("* Operations Reports", false, true));
                    if (Ticket.Incident.AgencyDataInvolved.ProposalAndLoanPurchaseRequestsWereReleased)
                        summaryItems.Add(new SummaryItem("* Proposal and Loan Purchase Requests", false, true));
                    if (Ticket.Incident.AgencyDataInvolved.UespParticipantRecordsWereReleased)
                        summaryItems.Add(new SummaryItem("* UESP Participant Records", false, true));
                    if (Ticket.Incident.AgencyDataInvolved.OtherInformationWasReleased)
                        summaryItems.Add(new SummaryItem("* " + Ticket.Incident.AgencyDataInvolved.OtherInformation, false, true));
                    break;
                case Incident.AGENCY_EMPLOYEE_HR_DATA:
                    summaryItems.Add(new SummaryItem("* Employee Name: " + Ticket.Incident.AgencyEmployeeDataInvolved.Name, false, true));
                    summaryItems.Add(new SummaryItem("* Employee State: " + Ticket.Incident.AgencyEmployeeDataInvolved.State, false, true));
                    if (Ticket.Incident.AgencyEmployeeDataInvolved.NotifierKnowsEmployee)
                    {
                        summaryItems.Add(new SummaryItem("* Notifer knows the employee", false, true));
                        summaryItems.Add(new SummaryItem("* Relationship to employee: " + Ticket.Incident.AgencyEmployeeDataInvolved.NotifierRelationshipToEmployee, false, true));
                    }
                    else
                    {
                        summaryItems.Add(new SummaryItem("* Notifer does not know the employee", false, true));
                    }
                    if (Ticket.Incident.AgencyEmployeeDataInvolved.DateOfBirthWasReleased)
                        summaryItems.Add(new SummaryItem("* Date of Birth", false, true));
                    if (Ticket.Incident.AgencyEmployeeDataInvolved.EmployeeIdNumberWasReleased)
                        summaryItems.Add(new SummaryItem("* Employee ID Number", false, true));
                    if (Ticket.Incident.AgencyEmployeeDataInvolved.HomeAddressWasReleased)
                        summaryItems.Add(new SummaryItem("* Home Address", false, true));
                    if (Ticket.Incident.AgencyEmployeeDataInvolved.HealthInformationWasReleased)
                        summaryItems.Add(new SummaryItem("* Health Information", false, true));
                    if (Ticket.Incident.AgencyEmployeeDataInvolved.PerformanceInformationWasReleased)
                        summaryItems.Add(new SummaryItem("* Performance Information", false, true));
                    if (Ticket.Incident.AgencyEmployeeDataInvolved.PersonnelFilesWereReleased)
                        summaryItems.Add(new SummaryItem("* Personnel Files", false, true));
                    if (Ticket.Incident.AgencyEmployeeDataInvolved.UnauthorizedReferenceWasReleased)
                        summaryItems.Add(new SummaryItem("* Unauthorized Reference", false, true));
                    break;
                case Incident.BORROWER_DATA:
                    summaryItems.Add(new SummaryItem("* Name: " + Ticket.Incident.BorrowerDataInvolved.Name, false, true));
                    summaryItems.Add(new SummaryItem("* Account Number: " + Ticket.Incident.BorrowerDataInvolved.AccountNumber, false, true));
                    summaryItems.Add(new SummaryItem("* State: " + Ticket.Incident.BorrowerDataInvolved.State, false, true));
                    summaryItems.Add(new SummaryItem("* Verified Borrower Information: " + (Ticket.Incident.BorrowerDataInvolved.BorrowerInformationIsVerified ? "Yes" : "No"), false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.NotifierKnowsPiiOwner)
                    {
                        summaryItems.Add(new SummaryItem("* Notifer knows the PII owner", false, true));
                        summaryItems.Add(new SummaryItem("* Relationship to owner: " + Ticket.Incident.BorrowerDataInvolved.NotifierRelationshipToPiiOwner, false, true));
                    }
                    else
                    {
                        summaryItems.Add(new SummaryItem("* Notifer does not know the PII owner", false, true));
                    }
                    if (Ticket.Incident.BorrowerDataInvolved.AddressWasReleased)
                        summaryItems.Add(new SummaryItem("* Address", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.BankAccountNumbersWereReleased)
                        summaryItems.Add(new SummaryItem("* Bank Account Numbers", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.CreditReportOrScoreWasReleased)
                        summaryItems.Add(new SummaryItem("* Credit Report/Scores", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.DateOfBirthWasReleased)
                        summaryItems.Add(new SummaryItem("* Date of Birth", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.EmployeeIdNumberWasReleased)
                        summaryItems.Add(new SummaryItem("* Employee ID Number", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.EmployerIdNumberWasReleased)
                        summaryItems.Add(new SummaryItem("* Employer ID Number", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.LoanAmountsOrBalancesWereReleased)
                        summaryItems.Add(new SummaryItem("* Loan Amounts/Balances", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.LoanApplicationsWereReleased)
                        summaryItems.Add(new SummaryItem("* Loan Applications", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.LoanIdsOrNumbersWereReleased)
                        summaryItems.Add(new SummaryItem("* Loan IDs or Numbers", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.LoanPaymentHistoriesWereReleased)
                        summaryItems.Add(new SummaryItem("* Loan Payment Histories", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.MedicalOrConditionalDisabilityWasReleased)
                        summaryItems.Add(new SummaryItem("* Medical/Conditional Disability", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.PayoffAmountsWereReleased)
                        summaryItems.Add(new SummaryItem("* Payoff Amounts", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.PhoneNumberWasReleased)
                        summaryItems.Add(new SummaryItem("* Phone Number", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.PromissoryNotesWereReleased)
                        summaryItems.Add(new SummaryItem("* Promissory Notes", false, true));
                    if (Ticket.Incident.BorrowerDataInvolved.SocialSecurityNumbersWereReleased)
                        summaryItems.Add(new SummaryItem("* Social Security Numbers", false, true));
                    break;
                case Incident.THIRD_PARTY_DATA:
                    summaryItems.Add(new SummaryItem("* Name: " + Ticket.Incident.ThirdPartyDataInvolved.Name, false, true));
                    summaryItems.Add(new SummaryItem("* Account Number: " + Ticket.Incident.ThirdPartyDataInvolved.AccountNumber, false, true));
                    summaryItems.Add(new SummaryItem("* Third Party State: " + Ticket.Incident.ThirdPartyDataInvolved.State, false, true));
                    if (Ticket.Incident.ThirdPartyDataInvolved.NotifierKnowsPiiOwner)
                    {
                        summaryItems.Add(new SummaryItem("* Notifer knows the PII owner", false, true));
                        summaryItems.Add(new SummaryItem("* Relationship to owner: " + Ticket.Incident.ThirdPartyDataInvolved.NotifierRelationshipToPiiOwner, false, true));
                    }
                    else
                    {
                        summaryItems.Add(new SummaryItem("* Notifer does not know the PII owner", false, true));
                    }
                    if (Ticket.Incident.ThirdPartyDataInvolved.SocialSecurityNumbersWereReleased)
                        summaryItems.Add(new SummaryItem("* Social Security Numbers", false, true));
                    if (Ticket.Incident.ThirdPartyDataInvolved.LoanIdsOrNumbersWereReleased)
                        summaryItems.Add(new SummaryItem("* Loan IDs or Numbers", false, true));
                    if (Ticket.Incident.ThirdPartyDataInvolved.LoanAmountsOrBalancesWereReleased)
                        summaryItems.Add(new SummaryItem("* Loan Amounts/Balances", false, true));
                    if (Ticket.Incident.ThirdPartyDataInvolved.LoanPaymentHistoriesWereReleased)
                        summaryItems.Add(new SummaryItem("* Loan Payment Histories", false, true));
                    if (Ticket.Incident.ThirdPartyDataInvolved.PayoffAmountsWereReleased)
                        summaryItems.Add(new SummaryItem("* Payoff Amounts", false, true));
                    if (Ticket.Incident.ThirdPartyDataInvolved.BankAccountNumbersWereReleased)
                        summaryItems.Add(new SummaryItem("* Bank Account Numbers", false, true));
                    if (Ticket.Incident.ThirdPartyDataInvolved.DateOfBirthWasReleased)
                        summaryItems.Add(new SummaryItem("* Date of Birth", false, true));
                    if (Ticket.Incident.ThirdPartyDataInvolved.MedicalOrConditionalDisabilityWasReleased)
                        summaryItems.Add(new SummaryItem("* Medical/Conditional Disability", false, true));
                    break;
            }
            return summaryItems.ToArray();
        }

        private Control[] GetCauseSummaryItems()
        {
            List<Control> summaryItems = new List<Control>();
            summaryItems.Add(new SummaryItem("Cause", true, true));
            if (!string.IsNullOrEmpty(Ticket.Incident.Cause))
                summaryItems.Add(new SummaryItem("* " + Ticket.Incident.Cause, false, true));
            if (Ticket.Incident.Cause == Incident.BORROWER_RELATIVE)
            {
                if (Ticket.Incident.BorrowerSsnAndDobAreVerified)
                    summaryItems.Add(new SummaryItem("* Borrower's SSN/DOB was verified", false, true));
                else
                    summaryItems.Add(new SummaryItem("* Borrower's SSN/DOB was NOT verified", false, true));
            }
            return summaryItems.ToArray();
        }

        private Control[] GetNarrativeOfIncidentSummaryItems()
        {
            List<Control> summaryItems = new List<Control>();
            summaryItems.Add(new SummaryItem("Narrative of Incident", true, true));
            if (!string.IsNullOrEmpty(Ticket.Incident.Narrative))
                summaryItems.Add(new SummaryItem(Ticket.Incident.Narrative, false, true));
            return summaryItems.ToArray();
        }

        private Control[] GetActionsTakenSummaryItems()
        {
            string[] contactActions = { ActionTaken.CONTACTED_IT_INFORMATION_SECURITY_OFFICE, ActionTaken.CONTACTED_LAW_ENFORCEMENT, ActionTaken.NOTIFIED_AFFECTED_INDIVIDUAL };
            List<Control> summaryItems = new List<Control>();
            List<ActionTaken> actionsTaken = Ticket.Incident.ActionsTaken.Where(p => p.ActionWasTaken).ToList();
            summaryItems.Add(new SummaryItem("Actions Taken", true, true));
            foreach (ActionTaken actionTaken in actionsTaken)
            {
                summaryItems.Add(new SummaryItem("* " + actionTaken.Action, false, true));
                if (contactActions.Contains(actionTaken.Action))
                    summaryItems.Add(new SummaryItem("* Person Contacted: " + actionTaken.PersonContacted, false, !string.IsNullOrEmpty(actionTaken.PersonContacted)));
                summaryItems.Add(new SummaryItem("* Date: " + actionTaken.DateTime.ToString("MM/dd/yyyy"), false, true));
                summaryItems.Add(new SummaryItem("* Time: " + actionTaken.DateTime.ToString("hh:mm:ss tt"), false, true));
            }
            return summaryItems.ToArray();
        }
    }
}