using MDIntermediary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using System.Dynamic;
using System.Drawing;
using System.Windows.Forms;
using static MauiDUDE.DemographicsUI;

namespace MauiDUDE
{
    public class HomePageAndDemographicsOnlyFlow : BaseUserSelectedFlow
    {
        private bool _demosOnly;
        private bool _callsMode;
        public Borrower _uheaaBorrower;
        public DemographicsUI _demosForm;
        private ReflectionInterface ri;

        public HomePageAndDemographicsOnlyFlow(bool demosOnly, bool callsMode, ReflectionInterface ri)
        {
            _demosOnly = demosOnly;
            _callsMode = callsMode;
            this.ri = ri;
        }

        public override void Process()
        {
            bool flowComplete = false; //allows dude to handle an incoming call without going back to the home page
            bool uheaaBorrowerIsOneLink = false;

            Thread demosThread;
            TimeSpan threadWaitTime = new TimeSpan(0, 0, 1);

            while (!flowComplete) //this loop allows the incoming and previous borrower logic to work.
            {
                flowComplete = true; //assume that the flow will complete. this only changes if the user selected the incoming call/previous borrower option on the home page

                demosThread = new Thread(new ParameterizedThreadStart(ShowDemographics));
                demosThread.SetApartmentState(ApartmentState.STA);

                //create object to handle queue information
                AcpQueueInfo tempACPQueueData = new AcpQueueInfo();
                //check if user provided an SSN or account number
                if (SSN.Length == 0)
                {
                    //no account number or SSN provided on Main Menu
                    //check to see if the user is logged into ACP and is either using the dialer or working a queue task
                    //TESTIT: (3) verify is getting ACP queue and region if user is working a queue task
                    SSN = HomePageSessionInteractionCoordinator.DoPreBorrowerCreationProcessing(tempACPQueueData);
                    if (SSN.Trim().Length == 0)
                    {
                        WhoaDUDE.ShowWhoaDUDE("Please provide DUDE with an SSN or account number to use.", "Borrower Identifier Not Provided");
                        return;
                    }
                }

                //!!!NOTE the variables in these actions are not synchronized.
                //Only one copy of each action should run at the same time.
                //use SSN for string S
                Action<string> lookupCompassUheaaBorrower = (string s) =>
                {
                    //try to get borrower from uheaa warehouse
                    try
                    {
                        _uheaaBorrower = DataAccess.DA.GetMinimizedUheaaCompassBorrowerFromWarehouse(s);
                    }
                    catch (BorrowerNotFoundInWarehouseException ex)
                    {
                        _uheaaBorrower = null;
                    }
                };

                //creating a new thread accepting a string as a parameter to the start call
                var operationUheaa = new ParameterizedThreadStart(obj => lookupCompassUheaaBorrower((string)obj));
                Thread uheaaLookupThread = new Thread(operationUheaa);
                uheaaLookupThread.Start(SSN);
                uheaaLookupThread.Join();

                //TRY THIS:
                //warn the user and return to the main menu if the borrower was not found anywhere
                if (_uheaaBorrower == null)
                {
                    try
                    {
                        _uheaaBorrower = DataAccess.DA.GetFFELPOneLINKBorrowerFromWarehouse(SSN);
                        uheaaBorrowerIsOneLink = true;
                    }
                    catch (BorrowerNotFoundInWarehouseException ex)
                    {
                        if (DataAccess.DA.HasAP03Record(SSN, DataAccessHelper.Region.Uheaa))
                        {
                            WhoaDUDE.ShowWhoaDUDE("Complete Loan borrower – pre-disbursement – forward call to 801-266-7538", "Complete Loan Borrower");
                        }
                        else
                        {
                            WhoaDUDE.ShowWhoaDUDE("The borrower was not found.", "Borrower Not Found");
                        }

                    }
                }

                //do call forwarding calculations for UHEAA borrowers
                bool continueToUseUheaaHomePage = false;
                if (_uheaaBorrower != null)
                {
                    CallForwardingCalculator.CalculateCallForwardingResults(_uheaaBorrower, DataAccessHelper.Region.Uheaa);
                    continueToUseUheaaHomePage = _uheaaBorrower.ContinueToUseHomePage;
                }

                string previousBorrowerFullName = "";
                string previousBorrowerSsn = "";

                //process forwarding information for UHEAA loans if the homepage won't be displayed
                if (_uheaaBorrower != null && !continueToUseUheaaHomePage)
                {
                    //Call can't be handled by the agent. Must be forwarded.

                    //NOTE: the demographic information was removed from the OneLINKDemosAndCallForwarding from but this is still necessary to get the borrower's name
                    //      this may go away depending on the extent to which OneLINK is eventually removed
                    //collect demos from OneLINK
                    _uheaaBorrower.UpdatedDemographics = new Demographics();
                    try
                    {
                        OneLINKDemographicsProcessor olDemosProcessor = new OneLINKDemographicsProcessor();
                        olDemosProcessor.Populate(_uheaaBorrower);
                    }
                    catch (Exception ex)
                    {
                        //catch any parse error if user isn't on onelink
                    }
                    //bring name information up to the borrower level
                    if (!_uheaaBorrower.UpdatedDemographics.FirstName.IsNullOrEmpty())
                    {
                        _uheaaBorrower.FirstName = _uheaaBorrower.UpdatedDemographics.FirstName;
                        _uheaaBorrower.LastName = _uheaaBorrower.UpdatedDemographics.LastName;
                        _uheaaBorrower.MI = _uheaaBorrower.UpdatedDemographics.MI;
                        _uheaaBorrower.FullName = _uheaaBorrower.UpdatedDemographics.Name;
                        //bring DOB information up to the borrower level
                        _uheaaBorrower.DOB = _uheaaBorrower.UpdatedDemographics.DOB;
                    }

                    if (DataAccess.DA.HasAP03Record(SSN, DataAccessHelper.Region.Uheaa))
                    {
                        WhoaDUDE.ShowWhoaDUDE("Complete Loan borrower – pre-disbursement – forward call to 801-266-7538", "Complete Loan Borrower");
                    }
                    else
                    {
                        //call forwarding information to the user
                        var olDemosAndCallForwarding = new OneLINKDemosAndCallForwarding(_uheaaBorrower);
                        olDemosAndCallForwarding.BackColor = Color.FromArgb(123, 193, 252);
                        olDemosAndCallForwarding.ShowDialog();

                        //make note of borrower for previous borrower list
                        previousBorrowerFullName = _uheaaBorrower.FullName;
                        previousBorrowerSsn = _uheaaBorrower.SSN;
                    }
                }

                if(continueToUseUheaaHomePage)
                {
                    //uheaa home page processing
                    if(_uheaaBorrower != null)
                    {
                        try
                        {
                            //now borrower object is created go ahead and populate the queue information collected before it's creation
                            if(_uheaaBorrower.AcpResponses == null)
                            {
                                _uheaaBorrower.AcpResponses = new AcpResponses();
                            }
                            _uheaaBorrower.AcpResponses.Queue = tempACPQueueData;

                            //move data down to the demographics level
                            if(_uheaaBorrower.HasPendingDisbursement == "Y")
                            {
                                //if borrower has pending disbursement then a PO box isn't allowed
                                _uheaaBorrower.CompassDemographics.POBoxAllowed = false;
                            }
                            else
                            {
                                //if borrower doesn't have a pending disbursement then a PO box is allowed
                                _uheaaBorrower.CompassDemographics.POBoxAllowed = true;
                            }

                            //make note of borrower for previous borrower list
                            previousBorrowerFullName = _uheaaBorrower.FullName;
                            previousBorrowerSsn = _uheaaBorrower.SSN;
                        }
                        catch(Exception ex)
                        {
                            if(DataAccess.DA.HasAP03Record(SSN, DataAccessHelper.Region.Uheaa))
                            {
                                WhoaDUDE.ShowWhoaDUDE("Complete Loan borrower – pre-disbursement – forward call to 801-266-7538", "Complete Loan Borrower");
                            }
                            else
                            {
                                WhoaDUDE.ShowWhoaDUDE("DUDE's forwarding information says the borrower has loans at UHEAA but the borrower was not found.", "Borrower Not Found");
                            }
                        }
                    }

                    //add borrower to previous borrower list
                    MainMenu.AddPreviousBorrower(new PreviousBorrower() { Name = previousBorrowerFullName, SSN = previousBorrowerSsn });

                    //display the 411 form
                    BrwInfo411Processor.Show411Form(_uheaaBorrower, false);

                    //show demographic form
                    var _demosForm = new DemographicsUI(_uheaaBorrower, _callsMode, ri);

                    
                    if (_demosForm.PopulateFrm())
                    {
                        
                        demosThread.Start(_demosForm); //start thread to handle demographics while more data is collected by DUDE
                        //form handle needs to be created before an invoke is called and with the form being on the other thread DUDE needs to wait for that thread to accomplish that
                        while (!_demosForm.IsHandleCreated)
                        {
                            Thread.Sleep(100);
                        }
                        //be sure that the demographic form has focus (this is how you handle cross thread calls to UIs)
                        _demosForm.Invoke(_demosForm.SelectDemographics);
                    }

                    if(!_demosOnly)
                    {
                        List<IHomePage> homePageList = new List<IHomePage>();
                        UheaaHomePage uheaaHomePageMdiChild = null;
                        HomePageMdi homePageMdi = null;

                        Action loadExtendedBorrowerInformation = () =>
                        {
                            //Load extended borrower information
                            //In the future this can be parallelized if necessary, not doing now since it should not be necessary.
                            if(_uheaaBorrower != null && !uheaaBorrowerIsOneLink)
                            {
                                _uheaaBorrower = DataAccess.DA.GetUheaaCompassBorrowerFromWarehouse((UheaaBorrower)_uheaaBorrower, _uheaaBorrower.SSN);
                            }
                        };
                        Thread loadExtendedBorrowerThread = new Thread(new ThreadStart(loadExtendedBorrowerInformation));
                        loadExtendedBorrowerThread.Start();

                        while (demosThread.IsAlive)
                        {
                            Application.DoEvents();
                            Thread.Sleep(threadWaitTime);
                        }                        

                        bool loopForDemos = true; //allows dude to switch between demographics and homepage
                        //join the home page creation thread
                        loadExtendedBorrowerThread.Join();

                        AlertCoordinator.CoordinateAlertsCreation((UheaaBorrower)_uheaaBorrower);

                        while (loopForDemos) //this loop allows the user to continually switch back and forth between the home page and demos
                        {
                            loopForDemos = false; //assume looping is over, this will be updated to loop again if update demos is clicked on the home page

                            //exit functionality if demos form back button is clicked
                            if(_demosForm.BackButtonClicked)
                            {
                                return; //if back button is clicked then go back to main menu
                            }
                            
                            if(homePageMdi == null)
                            {
                                if (_uheaaBorrower != null)
                                {
                                    uheaaHomePageMdiChild = new UheaaHomePage((UheaaBorrower)_uheaaBorrower);
                                    homePageList.Add(uheaaHomePageMdiChild);
                                }

                                homePageMdi = new HomePageMdi(homePageList);                                
                            }
                            homePageMdi.ShowDialog();

                            if(homePageMdi.UserChosenPathFromHP == HomePageMdi.UserChosenPathFromHomePage.UpdateDemographics)
                            {
                                loopForDemos = true; // loop back to the home page when done on demographics
                                if(_demosForm.ShowDialog() == DialogResult.Abort)
                                {
                                    homePageMdi.UserChosenPathFromHP = HomePageMdi.UserChosenPathFromHomePage.BackToMainMenu;
                                    break;
                                }
                            }
                        }

                        if (homePageMdi.UserChosenPathFromHP == HomePageMdi.UserChosenPathFromHomePage.IncomingCall)
                        {
                            flowComplete = false;
                            SSN = homePageMdi.SSN;
                        }
                        else if(homePageMdi.UserChosenPathFromHP == HomePageMdi.UserChosenPathFromHomePage.SaveAndContinue)
                        {
                            //only do update when save and continue is clicked
                            _demosForm.UpdateSystem();
                            //do save and continue processing

                            if(_uheaaBorrower != null)
                            {
                                uheaaHomePageMdiChild.SaveAndContinueProcessing();
                            }

                            if(BrwInfo411Processor.Info411Changed)
                            {
                                BrwInfo411Processor.SaveChangesToSystems();
                            }
                        }
                    }
                    else
                    {
                        //demos only
                        while(demosThread.IsAlive)
                        {
                            Application.DoEvents();
                            Thread.Sleep(threadWaitTime);
                        }
                        //exit functionality if demos form back button is clicked
                        if(_demosForm.BackButtonClicked)
                        {
                            return; //if back button is clicked then go back to main menu
                        }

                        _demosForm.UpdateSystem();
                    }
                }
            }
        }

        //run demos thread while last little tid bits of data are collected for the system
        private void ShowDemographics(object demos)
        {
            ((DemographicsUI)demos).ShowDialog();
        }

        private void LoadBorrowerAndHomePageInBackground()
        {

        }
    }
}
