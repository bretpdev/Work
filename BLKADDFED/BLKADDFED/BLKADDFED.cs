using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace BLKADDFED
{
    public class BLKADDFED : FedScriptBase
    {
        private MDBorrower _mdBorrower;

        public BLKADDFED(ReflectionInterface ri)
            : base(ri, "BLKADDFED", Region.CornerStone)
        {
        }

        public BLKADDFED(ReflectionInterface ri, MDBorrower borrower, int runNumber)
            : base(ri, "BLKADDFED", Region.CornerStone, borrower, runNumber)
        {
            _mdBorrower = borrower;
        }

        public override void Main()
        {
            string accountID;

            //warn the user and end if they don't have authorization to run the script
            if (!DataAccess.IsAuthorizedInActiveDirectory(RI.TestMode, "AddressPhoneBlock"))
            {
                MessageBox.Show("You don't have authorization to run this script.", "Authorization Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                EndDLLScript();
            }

            //main processing
            do
            {
                //prompt the user for the account ID if the script was not called by Maui DUDE
                if (!CalledByMauiDUDE)
                {
                    string startupMessage = "Enter the account number (10 characters) or the reference ID (9 characters). Click OK to continue or click Cancel to quit.";
                    accountID = InputBox.ShowDialog(startupMessage, "Block Address/Phone - FED").UserProvidedText;
                    if (accountID == string.Empty) { EndDLLScript(); }
                }
                else
                {
                    accountID = _mdBorrower.SSN;
                }

                //lists of addresses and phone numbers
                List<BorrowerAddress> brwAddresses = new List<BorrowerAddress>();
                List<BorrowerPhone> brwPhones = new List<BorrowerPhone>();

                //get addresses
                List<SystemBorrowerDemographics> brwDemographics = new List<SystemBorrowerDemographics>();
                Boolean borrowerIsFound = false;
                string ssn = string.Empty;
                string regardsToId = string.Empty;
                TD22.RegardsTo regardsToCode = TD22.RegardsTo.None;

                //this loop allows the user to keep trying if they enter the account id incorrectly
                do
                {
                    try
                    {
                        brwDemographics = Common.GetListOfDemographicsFromTX1J(RI, accountID, Common.ACSKeyLineAddressType.Legal, false);
                        if (Check4Text(1, 9, "B")) //TX1J
                        {
                            ssn = (from p in brwDemographics select p.SSN).FirstOrDefault();
                            regardsToId = ssn;
                            regardsToCode = TD22.RegardsTo.Borrower;
                        }
                        else
                        {
                            ssn = GetText(7, 11, 11).Replace(" ", "");
                            regardsToId = (from p in brwDemographics select p.SSN).FirstOrDefault();
                            switch (GetText(1, 9, 1))
                            {
                                case "E":
                                    regardsToCode = TD22.RegardsTo.Endorser;
                                    break;
                                case "R":
                                    regardsToCode = TD22.RegardsTo.Reference;
                                    break;
                                default:
                                    regardsToCode = TD22.RegardsTo.Borrower;
                                    break;
                            }
                        }
                        //004 84 1205

                        borrowerIsFound = true;
                    }
                    catch
                    {
                        string errorMessage = "The person ID was not found in the CornerStone region.  Reenter the account number (10 characters) or the reference ID (9 characters) and then click OK to continue or click Cancel to quit.";
                        accountID = InputBox.ShowDialog(errorMessage, "Block Address/Phone - FED").UserProvidedText;
                        if (accountID == string.Empty) { EndDLLScript(); }
                    }
                } while (!borrowerIsFound);

                //convert SystemBorrowerDemographics object to native BorrowerAddress object to add indicator of addresses selected
                foreach (SystemBorrowerDemographics i in brwDemographics)
                {
                    brwAddresses.Add(new BorrowerAddress(i));
                }

                //get home phone numbers
                List<Phone> brwHomePhones = new List<Phone>();
                brwHomePhones = Common.GetListOfPhonesFromTX1J(RI, accountID, Common.PhoneType.Home);
                foreach (Phone i in brwHomePhones)
                {
                    brwPhones.Add(new BorrowerPhone(i));
                }

                //get alternate phone numbers
                List<Phone> brwAltPhones;
                brwAltPhones = Common.GetListOfPhonesFromTX1J(RI, accountID, Common.PhoneType.Alternate);
                foreach (Phone i in brwAltPhones)
                {
                    brwPhones.Add(new BorrowerPhone(i));
                }

                //get work phone numbers
                List<Phone> brwWorkPhones;
                brwWorkPhones = Common.GetListOfPhonesFromTX1J(RI, accountID, Common.PhoneType.Work);
                foreach (Phone i in brwWorkPhones)
                {
                    brwPhones.Add(new BorrowerPhone(i));
                }

                //display the form for the user to review and select records
                SelectRecords frmSelectRecords = new SelectRecords(brwAddresses, brwPhones);
                frmSelectRecords.ShowDialog();
                if (frmSelectRecords.Result == DialogResult.Cancel) { EndDLLScript(); }

                //get the selected address to put in the comment
                BorrowerAddress commentAddress;
                commentAddress = (from p in brwAddresses where p.Selected select p).FirstOrDefault();

                //build and add the comment for the KOADD (address) activity record
                string comment = string.Empty;
                if (commentAddress != null)
                {
                    comment = string.Format("{0},{1},{2},{3},{4},{5},{6}", commentAddress.Address1, commentAddress.Address2, commentAddress.City, commentAddress.State, commentAddress.Zip, commentAddress.Country, frmSelectRecords.Comments);
                    if (!ATD22ByBalance(ssn, "K0ADD", comment, "BLKADDFED", false, regardsToId, regardsToCode, regardsToId).Equals(Q.Common.CompassCommentScreenResults.CommentAddedSuccessfully))
                    {
                        MessageBox.Show("Unable to add activity record.", "Unable to Add Activity Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        EndDLLScript();
                    }
                }

                //add a separate ARC for each phone
                foreach (BorrowerPhone i in brwPhones)
                {
                    //build the comment for the KOPHN (phone) activity record
                    comment = string.Empty;

                    if (i.Selected)
                    {
                        if (i.DomesticAreaCode.Length != 0)
                        {
                            comment = string.Format("{0}{1}{2}{3},{4}", i.DomesticAreaCode, i.DomesticPrefix, i.DomesticLineNumber, i.Extension, frmSelectRecords.Comments);
                        }
                        else
                        {
                            comment = string.Format("{0}{1}{2}{3},{4}", i.ForeignCountryCode, i.ForeignCityCode, i.ForeignLocalNumber, i.Extension, frmSelectRecords.Comments);
                        }
                    }

                    //add the comment for the KOPHN (phone) activity record
                    if (comment.Length != 0)
                    {
                        if (!ATD22ByBalance(ssn, "K0PHN", comment, "BLKADDFED", false, regardsToId, regardsToCode, regardsToId).Equals(Q.Common.CompassCommentScreenResults.CommentAddedSuccessfully))
                        {
                            MessageBox.Show("Unable to add activity record.", "Unable to Add Activity Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            EndDLLScript();
                        }
                    }
                }

                //loop to prompt the user for the next account unless called by Maui DUDE
            } while (!CalledByMauiDUDE);
        }//public override void Main
    }//public class BLKADDFED
}
