using System;
using System.Collections.Generic;
using System.Linq;

namespace INCIDENTRP
{
    public class Incident
    {
        //Valid values for Type, according to the LST_IncidentType table:
        public const string ACCESS_CONTROL = "Access Control";
        public const string DATA_ENTRY = "Data Entry";
        public const string DISPOSAL_DESTRUCTION = "Disposal/Destruction";
        public const string ELECTRONIC_MAIL_DELIVERY = "Electronic Mail/Delivery";
        public const string FAX = "Fax";
        public const string ODD_COMPUTER_BEHAVIOR = "Odd Computer Behavior";
        public const string PHYSICAL_DAMAGE_LOSS_THEFT = "Physical Damage/Loss/Theft";
        public const string REGULAR_MAIL_DELIVERY = "Regular Mail/Delivery";
        public const string SCANS_PROBES = "Scans/Probes";
        public const string SYSTEM_OR_NETWORK_UNAVAILABLE = "System or Network Unavailable";
        public const string TELEPHONE = "Telephone";
        public const string UNAUTHORIZED_PHYSICAL_ACCESS = "Unauthorized Physical Access";
        public const string UNAUTHORIZED_SYSTEM_ACCESS = "Unauthorized System Access";
        public const string VIOLATION_OF_ACCEPTABLE_USE = "Violation of Acceptable Use/Rules of Behavior";

        //Valid values for Priority, according to the LST_IncidentPriority table:
        public const string NONE = "";
        public const string CRITICAL = "Critical";
        public const string HIGH = "High";
        public const string LOW = "Low";
        public const string MEDIUM = "Medium";
        public const string UNSURE = "Unsure";

        //Valid values for Cause, according to the LST_IncidentCause table (note that NONE is also valid here):
        public const string BATCH_SCRIPT_ERROR = "Batch Script Error";
        public const string BORROWER_ACTION = "Borrower Action";
        public const string BORROWER_RELATIVE = "Borrower Relative";
        public const string ENDORSER_ACTION = "Endorser Action";
        public const string FEDEX_DELIVERED_TO_WRONG_ADDRESS = "FEDEX Delivered to Wrong Address";
        public const string INCORRECT_SAS_QUERY = "Incorrect SAS Query";
        public const string LENDER_ACTION = "Lender Action";
        public const string MAIL_SERVICES_STATE_MAIL = "Mail Services--State Mail";
        public const string SCHOOL_ACTION = "School Action";
        public const string SOCIAL_ENGINEERING_TECHNIQUE = "Social Engineering Technique";
        public const string UPS_DELIVERED_TO_WRONG_ADDRESS = "UPS Delivered to Wrong Address";
        public const string USPS_DELIVERED_TO_WRONG_ADDRESS = "USPS Delivered to Wrong Address";
        public const string VENDOR_PROVIDED_INCORRECT_ADDRESS = "Vendor Provided Incorrect Address";

        //Valid values for DataInvolved, according to the LST_DataInvolved table (note that NONE is also valid here):
        public const string AGENCY_DATA = "Agency Data";
        public const string AGENCY_EMPLOYEE_HR_DATA = "Agency Employee (HR) Data";
        public const string BORROWER_DATA = "Borrower Data";
        public const string THIRD_PARTY_DATA = "Third Party Data";

        public string Priority { get; set; }
        public string Cause { get; set; }
        public bool BorrowerSsnAndDobAreVerified { get; set; }
        public string Location { get; set; }
        public string Narrative { get; set; }
        public Notifier Notifier { get; set; }
        public Reporter Reporter { get; set; }

        //When an incident type is selected, instantiate an object for only that incident type.
        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                NullifyAllIncidentTypes();
                _type = value;
                switch (_type)
                {
                    case ACCESS_CONTROL:
                        AccessControlIncident = new AccessControlIncident();
                        break;
                    case DATA_ENTRY:
                        DataEntryIncident = new DataEntryIncident();
                        break;
                    case DISPOSAL_DESTRUCTION:
                        DisposalOrDestructionIncident = new DisposalOrDestructionIncident();
                        break;
                    case ELECTRONIC_MAIL_DELIVERY:
                        ElectronicMailDeliveryIncident = new ElectronicMailDeliveryIncident();
                        break;
                    case FAX:
                        FaxIncident = new FaxIncident();
                        break;
                    case ODD_COMPUTER_BEHAVIOR:
                        OddComputerBehaviorIncident = new OddComputerBehaviorIncident();
                        break;
                    case PHYSICAL_DAMAGE_LOSS_THEFT:
                        PhysicalDamageLossOrTheftIncident = new PhysicalDamageLossOrTheftIncident();
                        break;
                    case REGULAR_MAIL_DELIVERY:
                        RegularMailDeliveryIncident = new RegularMailDeliveryIncident();
                        break;
                    case SCANS_PROBES:
                        ScanOrProbeIncident = new ScanOrProbeIncident();
                        break;
                    case SYSTEM_OR_NETWORK_UNAVAILABLE:
                        SystemOrNetworkUnavailableIncident = new SystemOrNetworkUnavailableIncident();
                        break;
                    case TELEPHONE:
                        TelephoneIncident = new TelephoneIncident();
                        break;
                    case UNAUTHORIZED_PHYSICAL_ACCESS:
                        UnauthorizedPhysicalAccessIncident = new UnauthorizedPhysicalAccessIncident();
                        break;
                    case UNAUTHORIZED_SYSTEM_ACCESS:
                        UnauthorizedSystemAccessIncident = new UnauthorizedSystemAccessIncident();
                        break;
                    case VIOLATION_OF_ACCEPTABLE_USE:
                        ViolationOfAcceptableUseIncident = new ViolationOfAcceptableUseIncident();
                        break;
                }
            }
        }
        public AccessControlIncident AccessControlIncident;
        public DataEntryIncident DataEntryIncident;
        public DisposalOrDestructionIncident DisposalOrDestructionIncident;
        public ElectronicMailDeliveryIncident ElectronicMailDeliveryIncident;
        public FaxIncident FaxIncident;
        public OddComputerBehaviorIncident OddComputerBehaviorIncident;
        public PhysicalDamageLossOrTheftIncident PhysicalDamageLossOrTheftIncident;
        public RegularMailDeliveryIncident RegularMailDeliveryIncident;
        public ScanOrProbeIncident ScanOrProbeIncident;
        public SystemOrNetworkUnavailableIncident SystemOrNetworkUnavailableIncident;
        public TelephoneIncident TelephoneIncident;
        public UnauthorizedPhysicalAccessIncident UnauthorizedPhysicalAccessIncident;
        public UnauthorizedSystemAccessIncident UnauthorizedSystemAccessIncident;
        public ViolationOfAcceptableUseIncident ViolationOfAcceptableUseIncident;

        //When a data involved option is selected, instantiate an object for only that option.
        private string _dataInvolved;
        public string DataInvolved
        {
            get
            {
                return _dataInvolved;
            }
            set
            {
                NullifyAllDataInvolvedExcept(_dataInvolved);
                _dataInvolved = value;
                switch (_dataInvolved)
                {
                    case AGENCY_DATA:
                        AgencyDataInvolved = AgencyDataInvolved ?? new AgencyDataInvolved();
                        break;
                    case AGENCY_EMPLOYEE_HR_DATA:
                        AgencyEmployeeDataInvolved = AgencyEmployeeDataInvolved ?? new AgencyEmployeeHrDataInvolved();
                        break;
                    case BORROWER_DATA:
                        BorrowerDataInvolved = BorrowerDataInvolved ?? new BorrowerDataInvolved();
                        break;
                    case THIRD_PARTY_DATA:
                        ThirdPartyDataInvolved = ThirdPartyDataInvolved ?? new ThirdPartyDataInvolved();
                        break;
                }
            }
        }
        public AgencyDataInvolved AgencyDataInvolved { get; set; }
        public AgencyEmployeeHrDataInvolved AgencyEmployeeDataInvolved { get; set; }
        public BorrowerDataInvolved BorrowerDataInvolved { get; set; }
        public ThirdPartyDataInvolved ThirdPartyDataInvolved { get; set; }
        public List<ActionTaken> ActionsTaken { get; }

        public bool IsComplete()
        {
            //The spec doesn't state that any of the properties on Incident itself are required,
            //so just check member objects.
            if (!Notifier.IsComplete()) return false;
            if (!Reporter.IsComplete()) return false;
            //Type:
            if (AccessControlIncident != null && !AccessControlIncident.IsComplete()) return false;
            if (DataEntryIncident != null && !DataEntryIncident.IsComplete()) return false;
            if (DisposalOrDestructionIncident != null && !DisposalOrDestructionIncident.IsComplete()) return false;
            if (ElectronicMailDeliveryIncident != null && !ElectronicMailDeliveryIncident.IsComplete()) return false;
            if (FaxIncident != null && !FaxIncident.IsComplete()) return false;
            if (OddComputerBehaviorIncident != null && !OddComputerBehaviorIncident.IsComplete()) return false;
            if (PhysicalDamageLossOrTheftIncident != null && !PhysicalDamageLossOrTheftIncident.IsComplete()) return false;
            if (RegularMailDeliveryIncident != null && !RegularMailDeliveryIncident.IsComplete()) return false;
            if (ScanOrProbeIncident != null && !ScanOrProbeIncident.IsComplete()) return false;
            if (SystemOrNetworkUnavailableIncident != null && !SystemOrNetworkUnavailableIncident.IsComplete()) return false;
            if (TelephoneIncident != null && !TelephoneIncident.IsComplete()) return false;
            if (UnauthorizedPhysicalAccessIncident != null && !UnauthorizedPhysicalAccessIncident.IsComplete()) return false;
            if (UnauthorizedSystemAccessIncident != null && !UnauthorizedSystemAccessIncident.IsComplete()) return false;
            if (ViolationOfAcceptableUseIncident != null && !ViolationOfAcceptableUseIncident.IsComplete()) return false;
            //DataInvolved:
            if (AgencyDataInvolved != null && !AgencyDataInvolved.IsComplete()) return false;
            if (AgencyEmployeeDataInvolved != null && !AgencyEmployeeDataInvolved.IsComplete()) return false;
            if (BorrowerDataInvolved != null && !BorrowerDataInvolved.IsComplete()) return false;
            if (ThirdPartyDataInvolved != null && !ThirdPartyDataInvolved.IsComplete()) return false;
            return true;
        }

        public Incident()
        {
            //This constructor should only be used by the Load() method, which must set all of the object's properties.
            ActionsTaken = new List<ActionTaken>();
        }

        //Constructor for Need Help to call.
        public Incident(Reporter reporter)
        {
            Reporter = reporter;
            Priority = NONE;
            Notifier = new Notifier();
            Cause = NONE;
            ActionsTaken = new List<ActionTaken>();
            ActionsTaken.Add(new ActionTaken(ActionTaken.ASKED_CALLER_TO_RETURN_CORRESPONDENCE));
            ActionsTaken.Add(new ActionTaken(ActionTaken.CONTACTED_IT_INFORMATION_SECURITY_OFFICE));
            ActionsTaken.Add(new ActionTaken(ActionTaken.CONTACTED_LAW_ENFORCEMENT));
            ActionsTaken.Add(new ActionTaken(ActionTaken.CORRECTED_DATA));
            ActionsTaken.Add(new ActionTaken(ActionTaken.DELETED_FILES));
            ActionsTaken.Add(new ActionTaken(ActionTaken.LOGGED_OFF_SYSTEM));
            ActionsTaken.Add(new ActionTaken(ActionTaken.NOTIFIED_AFFECTED_INDIVIDUAL));
            ActionsTaken.Add(new ActionTaken(ActionTaken.REBOOTED_SYSTEM));
            ActionsTaken.Add(new ActionTaken(ActionTaken.REMOVED_SYSTEM_FROM_NETWORK));
            ActionsTaken.Add(new ActionTaken(ActionTaken.SHUT_DOWN_SYSTEM));
        }

        //Constructor for DUDE to call.
        public Incident(Reporter reporter, BorrowerDataInvolved borrowerData)
        {
            Reporter = reporter;
            Priority = NONE;
            Notifier = new Notifier();
            Cause = NONE;
            ActionsTaken = new List<ActionTaken>();
            ActionsTaken.Add(new ActionTaken(ActionTaken.ASKED_CALLER_TO_RETURN_CORRESPONDENCE));
            ActionsTaken.Add(new ActionTaken(ActionTaken.CONTACTED_IT_INFORMATION_SECURITY_OFFICE));
            ActionsTaken.Add(new ActionTaken(ActionTaken.CONTACTED_LAW_ENFORCEMENT));
            ActionsTaken.Add(new ActionTaken(ActionTaken.CORRECTED_DATA));
            ActionsTaken.Add(new ActionTaken(ActionTaken.DELETED_FILES));
            ActionsTaken.Add(new ActionTaken(ActionTaken.LOGGED_OFF_SYSTEM));
            ActionsTaken.Add(new ActionTaken(ActionTaken.NOTIFIED_AFFECTED_INDIVIDUAL));
            ActionsTaken.Add(new ActionTaken(ActionTaken.REBOOTED_SYSTEM));
            ActionsTaken.Add(new ActionTaken(ActionTaken.REMOVED_SYSTEM_FROM_NETWORK));
            ActionsTaken.Add(new ActionTaken(ActionTaken.SHUT_DOWN_SYSTEM));
        }

        public static Incident Load(DataAccess dataAccess, long ticketNumber)
        {
            //Load the incident's basic properties.
            Incident incident = dataAccess.LoadIncident(ticketNumber);

            //Load the member objects.
            incident.Notifier = dataAccess.LoadNotifier(ticketNumber, Ticket.INCIDENT);
            incident.Reporter = dataAccess.LoadReporter(ticketNumber, Ticket.INCIDENT);
            incident.ActionsTaken.Add(dataAccess.LoadActionTaken(ticketNumber, Ticket.INCIDENT, ActionTaken.ASKED_CALLER_TO_RETURN_CORRESPONDENCE));
            incident.ActionsTaken.Add(dataAccess.LoadActionTaken(ticketNumber, Ticket.INCIDENT, ActionTaken.CONTACTED_IT_INFORMATION_SECURITY_OFFICE));
            incident.ActionsTaken.Add(dataAccess.LoadActionTaken(ticketNumber, Ticket.INCIDENT, ActionTaken.CONTACTED_LAW_ENFORCEMENT));
            incident.ActionsTaken.Add(dataAccess.LoadActionTaken(ticketNumber, Ticket.INCIDENT, ActionTaken.CORRECTED_DATA));
            incident.ActionsTaken.Add(dataAccess.LoadActionTaken(ticketNumber, Ticket.INCIDENT, ActionTaken.DELETED_FILES));
            incident.ActionsTaken.Add(dataAccess.LoadActionTaken(ticketNumber, Ticket.INCIDENT, ActionTaken.LOGGED_OFF_SYSTEM));
            incident.ActionsTaken.Add(dataAccess.LoadActionTaken(ticketNumber, Ticket.INCIDENT, ActionTaken.NOTIFIED_AFFECTED_INDIVIDUAL));
            incident.ActionsTaken.Add(dataAccess.LoadActionTaken(ticketNumber, Ticket.INCIDENT, ActionTaken.REBOOTED_SYSTEM));
            incident.ActionsTaken.Add(dataAccess.LoadActionTaken(ticketNumber, Ticket.INCIDENT, ActionTaken.REMOVED_SYSTEM_FROM_NETWORK));
            incident.ActionsTaken.Add(dataAccess.LoadActionTaken(ticketNumber, Ticket.INCIDENT, ActionTaken.SHUT_DOWN_SYSTEM));

            //Type and DataInvolved will each only have one item to load,
            //but we don't know up front what it is.
            //Try loading different items until we get something back.
            incident._type = ACCESS_CONTROL;
            incident.AccessControlIncident = dataAccess.LoadAccessControlIncident(ticketNumber);
            if (incident.AccessControlIncident == null)
            {
                incident._type = DATA_ENTRY;
                incident.DataEntryIncident = dataAccess.LoadDataEntryIncident(ticketNumber);
                if (incident.DataEntryIncident == null)
                {
                    incident._type = DISPOSAL_DESTRUCTION;
                    incident.DisposalOrDestructionIncident = dataAccess.LoadDisposalOrDestructionIncident(ticketNumber);
                    if (incident.DisposalOrDestructionIncident == null)
                    {
                        incident._type = ELECTRONIC_MAIL_DELIVERY;
                        incident.ElectronicMailDeliveryIncident = dataAccess.LoadElectronicMailDeliveryIncident(ticketNumber);
                        if (incident.ElectronicMailDeliveryIncident == null)
                        {
                            incident._type = FAX;
                            incident.FaxIncident = dataAccess.LoadFaxIncident(ticketNumber);
                            if (incident.FaxIncident == null)
                            {
                                incident._type = ODD_COMPUTER_BEHAVIOR;
                                incident.OddComputerBehaviorIncident = dataAccess.LoadOddComputerBehaviorIncident(ticketNumber);
                                if (incident.OddComputerBehaviorIncident == null)
                                {
                                    incident._type = PHYSICAL_DAMAGE_LOSS_THEFT;
                                    incident.PhysicalDamageLossOrTheftIncident = dataAccess.LoadPhysicalDamageLossOrTheftIncident(ticketNumber);
                                    if (incident.PhysicalDamageLossOrTheftIncident == null)
                                    {
                                        incident._type = REGULAR_MAIL_DELIVERY;
                                        incident.RegularMailDeliveryIncident = dataAccess.LoadRegularMailDeliveryIncident(ticketNumber);
                                        if (incident.RegularMailDeliveryIncident == null)
                                        {
                                            incident._type = SCANS_PROBES;
                                            incident.ScanOrProbeIncident = dataAccess.LoadScanOrProbeIncident(ticketNumber);
                                            if (incident.ScanOrProbeIncident == null)
                                            {
                                                incident._type = SYSTEM_OR_NETWORK_UNAVAILABLE;
                                                incident.SystemOrNetworkUnavailableIncident = dataAccess.LoadSystemOrNetworkUnavailableIncident(ticketNumber);
                                                if (incident.SystemOrNetworkUnavailableIncident == null)
                                                {
                                                    incident._type = TELEPHONE;
                                                    incident.TelephoneIncident = dataAccess.LoadTelephoneIncident(ticketNumber);
                                                    if (incident.TelephoneIncident == null)
                                                    {
                                                        incident._type = UNAUTHORIZED_PHYSICAL_ACCESS;
                                                        incident.UnauthorizedPhysicalAccessIncident = dataAccess.LoadUnauthorizedPhysicalAccessIncident(ticketNumber);
                                                        if (incident.UnauthorizedPhysicalAccessIncident == null)
                                                        {
                                                            incident._type = UNAUTHORIZED_SYSTEM_ACCESS;
                                                            incident.UnauthorizedSystemAccessIncident = dataAccess.LoadUnauthorizedSystemAccessIncident(ticketNumber);
                                                            if (incident.UnauthorizedSystemAccessIncident == null)
                                                            {
                                                                incident._type = VIOLATION_OF_ACCEPTABLE_USE;
                                                                incident.ViolationOfAcceptableUseIncident = dataAccess.LoadViolationOfAcceptableUseIncident(ticketNumber);
                                                                if (incident.ViolationOfAcceptableUseIncident == null)
                                                                {
                                                                    incident._type = null;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            incident._dataInvolved = AGENCY_DATA;
            incident.AgencyDataInvolved = dataAccess.LoadAgencyDataInvolved(ticketNumber);
            if (incident.AgencyDataInvolved == null)
            {
                incident._dataInvolved = AGENCY_EMPLOYEE_HR_DATA;
                incident.AgencyEmployeeDataInvolved = dataAccess.LoadAgencyEmployeeHrDataInvolved(ticketNumber);
                if (incident.AgencyEmployeeDataInvolved == null)
                {
                    incident._dataInvolved = BORROWER_DATA;
                    incident.BorrowerDataInvolved = dataAccess.LoadBorrowerDataInvolved(ticketNumber);
                    if (incident.BorrowerDataInvolved == null)
                    {
                        incident._dataInvolved = THIRD_PARTY_DATA;
                        incident.ThirdPartyDataInvolved = dataAccess.LoadThirdPartyDataInvolved(ticketNumber);
                        if (incident.ThirdPartyDataInvolved == null)
                        {
                            incident._dataInvolved = null;
                        }
                    }
                }
            }

            return incident;
        }

        public void Save(DataAccess dataAccess, long ticketNumber)
        {
            //Save our own basic properties first.
            dataAccess.SaveIncident(this, ticketNumber);

            //Call on our member objects to save themselves.
            foreach (ActionTaken actionTaken in ActionsTaken)
                actionTaken.Save(dataAccess, ticketNumber, Ticket.INCIDENT);
            Notifier.Save(dataAccess, ticketNumber, Ticket.INCIDENT);
            Reporter.Save(dataAccess, ticketNumber, Ticket.INCIDENT);

            //For properties that can only have one object initialized, save the one that is initialized and delete the others.
            //Type:
            if (AccessControlIncident == null)
                dataAccess.DeleteAccessControlIncident(ticketNumber);
            else
                AccessControlIncident.Save(dataAccess, ticketNumber);
            if (DataEntryIncident == null)
                dataAccess.DeleteDataEntryIncident(ticketNumber);
            else
                DataEntryIncident.Save(dataAccess, ticketNumber);
            if (DisposalOrDestructionIncident == null)
                dataAccess.DeleteDisposalOrDestructionIncident(ticketNumber);
            else
                DisposalOrDestructionIncident.Save(dataAccess, ticketNumber);
            if (ElectronicMailDeliveryIncident == null)
                dataAccess.DeleteElectronicMailDeliveryIncident(ticketNumber);
            else
                ElectronicMailDeliveryIncident.Save(dataAccess, ticketNumber);
            if (FaxIncident == null)
                dataAccess.DeleteFaxIncident(ticketNumber);
            else
                FaxIncident.Save(dataAccess, ticketNumber);
            if (OddComputerBehaviorIncident == null)
                dataAccess.DeleteOddComputerBehaviorIncident(ticketNumber);
            else
                OddComputerBehaviorIncident.Save(dataAccess, ticketNumber);
            if (PhysicalDamageLossOrTheftIncident == null)
                dataAccess.DeletePhysicalDamageLossOrTheftIncident(ticketNumber);
            else
                PhysicalDamageLossOrTheftIncident.Save(dataAccess, ticketNumber);
            if (RegularMailDeliveryIncident == null)
                dataAccess.DeleteRegularMailDeliveryIncident(ticketNumber);
            else
                RegularMailDeliveryIncident.Save(dataAccess, ticketNumber);
            if (ScanOrProbeIncident == null)
                dataAccess.DeleteScanOrProbeIncident(ticketNumber);
            else
                ScanOrProbeIncident.Save(dataAccess, ticketNumber);
            if (SystemOrNetworkUnavailableIncident == null)
                dataAccess.DeleteSystemOrNetworkUnavailableIncident(ticketNumber);
            else
                SystemOrNetworkUnavailableIncident.Save(dataAccess, ticketNumber);
            if (TelephoneIncident == null)
                dataAccess.DeleteTelephoneIncident(ticketNumber);
            else
                TelephoneIncident.Save(dataAccess, ticketNumber);
            if (UnauthorizedPhysicalAccessIncident == null)
                dataAccess.DeleteUnauthorizedPhysicalAccessIncident(ticketNumber);
            else
                UnauthorizedPhysicalAccessIncident.Save(dataAccess, ticketNumber);
            if (UnauthorizedSystemAccessIncident == null)
                dataAccess.DeleteUnauthorizedSystemAccessIncident(ticketNumber);
            else
                UnauthorizedSystemAccessIncident.Save(dataAccess, ticketNumber);
            if (ViolationOfAcceptableUseIncident == null)
                dataAccess.DeleteViolationOfAcceptableUseIncident(ticketNumber);
            else
                ViolationOfAcceptableUseIncident.Save(dataAccess, ticketNumber);
            //DataInvolved:
            if (AgencyDataInvolved == null)
                dataAccess.DeleteAgencyDataInvolved(ticketNumber);
            else
                AgencyDataInvolved.Save(dataAccess, ticketNumber);
            if (AgencyEmployeeDataInvolved == null)
                dataAccess.DeleteAgencyEmployeeHrDataInvolved(ticketNumber);
            else
                AgencyEmployeeDataInvolved.Save(dataAccess, ticketNumber);
            if (BorrowerDataInvolved == null)
                dataAccess.DeleteBorrowerDataInvolved(ticketNumber);
            else
                BorrowerDataInvolved.Save(dataAccess, ticketNumber);
            if (ThirdPartyDataInvolved == null)
                dataAccess.DeleteThirdPartyDataInvolved(ticketNumber);
            else
                ThirdPartyDataInvolved.Save(dataAccess, ticketNumber);
        }

        private void NullifyAllIncidentTypes()
        {
            AccessControlIncident = null;
            DataEntryIncident = null;
            DisposalOrDestructionIncident = null;
            ElectronicMailDeliveryIncident = null;
            FaxIncident = null;
            OddComputerBehaviorIncident = null;
            PhysicalDamageLossOrTheftIncident = null;
            RegularMailDeliveryIncident = null;
            ScanOrProbeIncident = null;
            SystemOrNetworkUnavailableIncident = null;
            TelephoneIncident = null;
            UnauthorizedPhysicalAccessIncident = null;
            UnauthorizedSystemAccessIncident = null;
            ViolationOfAcceptableUseIncident = null;
        }

        private void NullifyAllDataInvolvedExcept(string dataInvolved)
        {
            if (dataInvolved != AGENCY_DATA)
                AgencyDataInvolved = null;
            if (dataInvolved != AGENCY_EMPLOYEE_HR_DATA)
                AgencyEmployeeDataInvolved = null;
            if (dataInvolved != BORROWER_DATA)
                BorrowerDataInvolved = null;
            if (dataInvolved != THIRD_PARTY_DATA)
                ThirdPartyDataInvolved = null;
        }
    }
}