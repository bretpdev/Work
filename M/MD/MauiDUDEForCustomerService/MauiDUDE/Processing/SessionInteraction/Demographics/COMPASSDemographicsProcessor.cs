using MDIntermediary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace MauiDUDE
{
    public class COMPASSDemographicsProcessor : ParentSystemDemographics
    {
        public const string invalidNumber = "8015551212";
        private ReflectionInterface RI;

        public COMPASSDemographicsProcessor() : base()
        {
            RI = SessionInteractionComponents.RI;
        }

        public override void Populate(Borrower borrower)
        {
            //is the borrower located on COMPASS
            RI.FastPath($"TX3Z/ITX1J;{borrower.SSN}");
            borrower.CompassDemographics = new Demographics();
            if(RI.CheckForText(1,71,"TXX1R"))
            {
                borrower.CompassDemographics.SSN = borrower.SSN;
                borrower.CompassDemographics.Name = RI.GetText(4, 34, 14) + " " + RI.GetText(4, 6, 24);
                borrower.CompassDemographics.FirstName = RI.GetText(4, 34, 14);
                borrower.CompassDemographics.LastName = RI.GetText(4, 6, 24);
                borrower.MI = RI.GetText(4, 53, 1);
                borrower.CompassDemographics.CLAccNum = RI.GetText(3, 34, 12).Replace(" ", "");
                borrower.CompassDemographics.DOB = RI.GetText(20, 6, 10).Replace(" ", "/");
                borrower.CompassDemographics.Addr1 = RI.GetText(11, 10, 30).Replace("_", "");
                borrower.CompassDemographics.Addr2 = RI.GetText(12, 10, 30).Replace("_", "");
                borrower.CompassDemographics.SPAddrVerDt = RI.GetText(10, 32, 8).ToDate().ToString("MM/dd/yyyy"); //Microsoft.VisualBasic.Format(CDate(Replace(GetText(10, 32, 8), " ", "/")), "Short Date");
                borrower.CompassDemographics.SPAddrInd = RI.GetText(11, 55, 1);
                borrower.CompassDemographics.City = RI.GetText(14, 8, 20).Replace("_", "");
                borrower.CompassDemographics.State = RI.GetText(14, 32, 2).Replace("_", "");
                borrower.CompassDemographics.State = borrower.CompassDemographics.State.Replace("_", "");
                borrower.CompassDemographics.Zip = RI.GetText(14, 40, 9).Replace("_", "");
                borrower.CompassDemographics.HomePhoneNum = (RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4)).Replace("_", "");
                borrower.CompassDemographics.HomePhoneExt = RI.GetText(17, 40, 5).Replace("_", "");
                borrower.CompassDemographics.HomePhoneMBL = RI.GetText(16, 20, 1);
                borrower.CompassDemographics.HomePhoneConsent = RI.GetText(16, 30, 1);

                if(RI.GetText(16,45,2) != "__")
                {
                    borrower.CompassDemographics.HomePhoneVerificationDate = RI.GetText(16, 45, 8).ToDate().ToString("MM/dd/yyyy");
                }

                borrower.CompassDemographics.HomePhoneValidityIndicator = RI.GetText(17, 54, 1).Replace("_", "");
                RI.Hit(Key.F6);
                RI.Hit(Key.F6);
                RI.Hit(Key.F6);

                RI.PutText(16, 14, "A", Key.Enter);
                borrower.CompassDemographics.OtherPhoneNum = (RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4)).Replace("_", "");
                borrower.CompassDemographics.OtherPhoneExt = RI.GetText(17, 40, 5).Replace("_", "");

                if(RI.GetText(16,45,2) != "__")
                {
                    borrower.CompassDemographics.OtherPhoneVerificationDate = RI.GetText(16, 45, 8).ToDate().ToString("MM/dd/yyyy");
                    borrower.CompassDemographics.OtherPhoneMBL = RI.GetText(16, 20, 1);
                    borrower.CompassDemographics.OtherPhoneConsent = RI.GetText(16, 30, 1);
                }

                borrower.CompassDemographics.OtherPhoneMBL = RI.GetText(17, 54, 1).Replace("_", "");

                RI.PutText(16, 14, "W", Key.Enter);
                borrower.CompassDemographics.OtherPhone2Num = (RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4)).Replace("_", "");
                borrower.CompassDemographics.OtherPhone2Ext = RI.GetText(17, 40, 5).Replace("_", "");

                if(RI.GetText(16,45,2) != "__")
                {
                    borrower.CompassDemographics.OtherPhone2VerificationDate = RI.GetText(16, 45, 8).ToDate().ToString("MM/dd/yyyy");
                    borrower.CompassDemographics.UPOther2MBL = RI.GetText(16, 20, 1);
                    borrower.CompassDemographics.OtherPhone2Consent = RI.GetText(16, 30, 1);
                }

                borrower.CompassDemographics.OtherPhone2ValidityIndicator = RI.GetText(17, 54, 1).Replace("_", "");

                //DWDOWN: update this code to get the alternate (other) and work (other 2) email addresses
                if(!RI.CheckForText(24,55,"F10=EML"))
                {
                    RI.Hit(Key.F2);
                }

                RI.Hit(Key.F10);
                borrower.CompassDemographics.Email = RI.GetText(14, 10, 60).Trim().TrimEnd('_').TrimStart('_');
                
                if(RI.GetText(11,17,2) != "__")
                {
                    borrower.CompassDemographics.SPEmailVerDt = RI.GetText(11, 17, 8).ToDate().ToString("MM/dd/yyyy");
                }

                borrower.CompassDemographics.SPEmailInd = RI.GetText(12, 14, 1).Replace("_", "");
                borrower.CompassDemographics.FoundOnSystem = true;
            }
            else
            {
                borrower.CompassDemographics.FoundOnSystem = false;
            }
        }

        public override void Update(string source, UpdateDemoCompassIndicators systemsUpdateIndicators, bool isSchool, Demographics demosForUpdating, DemographicVerifications demographicVerifications, MDBorrowerDemographics altAddress)
        {
            if(source == "25")
            {
                source = "31";
            }

            //do alt address adding and invalidate first functionality
            COMPASSInvalidateFirstAndAlternateAddrProc(source, systemsUpdateIndicators.PhoneNoPhoneIndicator, demographicVerifications, altAddress, isSchool, systemsUpdateIndicators.Address, demosForUpdating);
            RI.FastPath($"TX3Z/CTX1J;{demosForUpdating.SSN}");
            //dont update COMPASS if error 01019 is returned
            if(RI.CheckForText(23,2,"01019"))
            {
                return;
            }

            //switch to address info
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            if(isSchool) //if the contact was a school
            {
                if(!RI.CheckForText(8,18," "))
                {
                    //Only populate source if blank
                    RI.PutText(8, 18, source);
                    RI.PutText(9, 18, "02");
                }
                RI.PutText(10, 32, RI.GetText(10, 32, 2));
                RI.PutText(10, 35, RI.GetText(10, 35, 2));
                RI.PutText(10, 38, RI.GetText(10, 38, 2));
                RI.PutText(11, 55, RI.GetText(11, 55, 1), Key.Enter);
            }

            //address is valid OR address is not valid and the address entered by the user matches the system address
            if(demographicVerifications.Address != VerificationSelection.NoChange && demographicVerifications.Address != VerificationSelection.RefusedNoChange) //update the address validity indicator if the address was changed or verified
            {
                if(RI.CheckForText(8,13,"CODE"))
                {
                    RI.PutText(8, 18, source);
                }
                if(RI.CheckForText(11, 55, "N") && !demographicVerifications.Address.IsValid())
                {
                    RI.PutText(10, 32, RI.GetText(10, 32, 2));
                    RI.PutText(10, 35, RI.GetText(10, 35, 2));
                    RI.PutText(10, 38, RI.GetText(10, 38, 2));
                }
                else
                {
                    RI.PutText(10, 32, DateTime.Today.ToString("MM/dd/yy").Replace("/", ""));
                }
                if (demographicVerifications.Address.IsValid())
                {
                    RI.PutText(11, 55, "Y");
                }
                else
                {
                    RI.PutText(11, 55, "N");
                }
                if(systemsUpdateIndicators.Address)
                {
                    UpdateCOMPASSAddress(demosForUpdating);
                }
                RI.Hit(Key.Enter);
                if(!RI.CheckForText(23,2, "01096 ADDRESS DATA UPDATED"))
                {
                    Demographics.AddressSaved = false;
                }

            }

            RI.Hit(Key.F6);

            //Invalidate all phone numbers and set them to 8015551212 if No Phone checked
            if(systemsUpdateIndicators.PhoneNoPhoneIndicator)
            {
                //Home phone
                ClearForeignFields();
                RI.PutText(17, 14, invalidNumber, Key.EndKey);
                RI.PutText(16, 20, "U");
                RI.PutText(16, 30, "N");
                RI.PutText(16, 45, DateTime.Today.ToString("MM/dd/yy").Replace("/", ""));
                RI.PutText(19, 14, source);
                RI.PutText(17, 54, "N", Key.Enter);
                //Other phone
                RI.PutText(16, 14, "A", Key.Enter);
                ClearForeignFields();
                RI.PutText(17, 14, invalidNumber, Key.EndKey);
                RI.PutText(16, 20, "U");
                RI.PutText(16, 30, "N");
                RI.PutText(16, 45, DateTime.Today.ToString("MM/dd/yy").Replace("/", ""));
                RI.PutText(19, 14, source);
                RI.PutText(17, 54, "N", Key.Enter);
                //Work Phone
                RI.PutText(16, 14, "W", Key.Enter);
                ClearForeignFields();
                RI.PutText(17, 14, invalidNumber, Key.EndKey);
                RI.PutText(16, 20, "U");
                RI.PutText(16, 30, "N");
                RI.PutText(16, 45, DateTime.Today.ToString("MM/dd/yy").Replace("/", ""));
                RI.PutText(19, 14, source);
                RI.PutText(17, 54, "N", Key.Enter);
                //Set all the indicators to false to skip the rest of the phone updates
                systemsUpdateIndicators.Phone = false;
                systemsUpdateIndicators.PhoneIndicator = false;
                systemsUpdateIndicators.OtherPhone = false;
                systemsUpdateIndicators.OtherPhoneIndicator = false;
                systemsUpdateIndicators.Other2Phone = false;
                systemsUpdateIndicators.Other2PhoneIndicator = false;

            }
            
            if(demographicVerifications.HomePhone != VerificationSelection.NoChange && demographicVerifications.HomePhone != VerificationSelection.RefusedNoChange)
            {
                UpdatePhone(source, "H", demosForUpdating.HomePhoneNum, demosForUpdating.HomePhoneExt, demosForUpdating.HomePhoneForeignCountry, demosForUpdating.HomePhoneForeignCity, demosForUpdating.HomePhoneForeignLocalNumber, demosForUpdating.HomePhoneMBL, demographicVerifications.HomePhoneConsent, demographicVerifications.HomePhone);
            }
            if(demographicVerifications.OtherPhone != VerificationSelection.NoChange && demographicVerifications.OtherPhone != VerificationSelection.RefusedNoChange)
            {
                UpdatePhone(source, "A", demosForUpdating.OtherPhoneNum, demosForUpdating.OtherPhoneExt, demosForUpdating.OtherPhoneForeignCountry, demosForUpdating.OtherPhoneForeignCity, demosForUpdating.OtherPhoneForeignLocalNumber, demosForUpdating.OtherPhoneMBL, demographicVerifications.OtherPhoneConsent, demographicVerifications.OtherPhone);
            }
            if(demographicVerifications.OtherPhone2 != VerificationSelection.NoChange && demographicVerifications.OtherPhone2 != VerificationSelection.RefusedNoChange)
            {
                UpdatePhone(source, "W", demosForUpdating.OtherPhone2Num, demosForUpdating.OtherPhone2Ext, demosForUpdating.OtherPhone2ForeignCountry, demosForUpdating.OtherPhone2ForeignCity, demosForUpdating.OtherPhone2ForeignLocalNumber, demosForUpdating.OtherPhone2MBL, demographicVerifications.OtherPhone2Consent, demographicVerifications.OtherPhone2);
            }

            //E-mail
            if(demographicVerifications.Email != VerificationSelection.NoChange && demographicVerifications.Email != VerificationSelection.RefusedNoChange)
            {
                Demographics.EmailSaved = UpdatedEmail(source, "H", demosForUpdating.Email, systemsUpdateIndicators.EmailIndicator);
                if(demosForUpdating.EcorrCorrespondence || demosForUpdating.EcorrBilling || demosForUpdating.EcorrTax)
                {
                    Demographics.EmailSaved = UpdatedEmail(source, "C", demosForUpdating.Email, systemsUpdateIndicators.EmailIndicator);
                }
            }
            //Other E-mail
            if(demographicVerifications.OtherEmail != VerificationSelection.NoChange && demographicVerifications.OtherEmail != VerificationSelection.RefusedNoChange)
            {
                Demographics.OtherEmailSaved = UpdatedEmail(source, "A", demosForUpdating.OtherEmail, systemsUpdateIndicators.OtherEmailIndicator);
            }
            //Other E-mail 2
            if(demographicVerifications.OtherEmail2 != VerificationSelection.NoChange && demographicVerifications.OtherEmail2 != VerificationSelection.RefusedNoChange)
            {
                Demographics.OtherEmail2Saved = UpdatedEmail(source, "W", demosForUpdating.OtherEmail2, systemsUpdateIndicators.OtherEmail2Indicator);
            }

            //ECORR
            RI.Hit(Key.F2);
            RI.Hit(Key.F10);
            if(RI.CheckForText(10,14,"C"))
            {
                RI.PutText(9, 20, "41");//address source code
                if(!demosForUpdating.EcorrCorrespondence || !demosForUpdating.EcorrBilling || !demosForUpdating.EcorrTax || (demographicVerifications.Email != VerificationSelection.NoChange && demographicVerifications.Email != VerificationSelection.RefusedNoChange))
                {
                    RI.PutText(11, 17, DateTime.Today.ToString("MMddyy"));
                }
                else
                {
                    RI.PutText(11, 17, RI.GetText(11, 17, 2));
                    RI.PutText(11, 20, RI.GetText(11, 20, 2));
                    RI.PutText(11, 23, RI.GetText(11, 23, 2));
                }
                if(demographicVerifications.Email != VerificationSelection.NoChange && demographicVerifications.Email != VerificationSelection.RefusedNoChange)
                {
                    RI.PutText(12, 14, systemsUpdateIndicators.EmailIndicator ? "Y" : "N"); //Address Valid
                }
                else
                {
                    RI.PutText(12, 14, RI.GetText(12, 14, 1)); //need to re-type for no reason at all
                }
                if(!demosForUpdating.EcorrCorrespondence)
                {
                    RI.PutText(20, 18, "N");
                }
                if(!demosForUpdating.EcorrBilling)
                {
                    RI.PutText(21, 18, "N");
                }
                if(!demosForUpdating.EcorrTax)
                {
                    RI.PutText(21, 35, "N");
                }
                RI.Hit(Key.Enter);
            }
            RI.Hit(Key.F12);
            RI.Hit(Key.F2);
        }

        private void ClearForeignFields()
        {
            RI.PutText(18, 15, "", Key.Enter);
            RI.PutText(18, 24, "", Key.Enter);
            RI.PutText(18, 36, "", Key.Enter);
            RI.PutText(18, 53, "", Key.Enter);
        }

        private void UpdatePhone(string source, string phoneType, string phoneNum, string phoneExt, string foreignCountry, string foreignCity, string foreignLocalNumber, string phoneMbl, bool consent, VerificationSelection verification)
        {
            bool isForeign = !string.IsNullOrWhiteSpace(foreignCountry);
            RI.PutText(16, 14, phoneType, Key.Enter); //enter phone type code (H,A,etc...)
            if(verification == VerificationSelection.ValidWithChangeAndInvalidateFirst)
            {
                RI.PutText(17, 54, "N", Key.Enter); //invalidate first
            }
            //if the phone was verified, it is valid, and it is not blank
            if(verification.IsValid())
            {
                if(!isForeign)
                {
                    if(phoneNum != invalidNumber) //if no phone wasn't selected
                    {
                        RI.PutText(17, 14, phoneNum); //update phone
                        if(phoneExt != "")
                        {
                            RI.PutText(17, 40, phoneExt); //update extension
                            if(phoneExt.Length < 5)
                            {
                                RI.Hit(Key.EndKey);
                            }
                            else
                            {
                                RI.PutText(17, 40, "", Key.EndKey);
                            }
                        }
                    }
                }
                else
                {
                    RI.PutText(18, 15, foreignCountry);
                    RI.PutText(18, 24, foreignCity);
                    RI.PutText(18, 36, foreignLocalNumber);
                }
                if(phoneNum != invalidNumber)
                {
                    RI.PutText(16, 20, phoneMbl == "" ? "U" : phoneMbl); //update MBL, U if none selected
                }
                RI.PutText(16, 45, DateTime.Today.ToString("MMddyy")); //update date
                RI.PutText(19, 14, source);//update source
                RI.PutText(17, 54, "Y"); //check as valid phone
                if(phoneMbl == "L" || consent)
                {
                    RI.PutText(16, 30, "Y");//update consent
                }
                else
                {
                    RI.PutText(16, 30, "N");
                }
            }
            else //if the phone is not valid
            {
                if (RI.CheckForText(17, 54, "Y"))
                {
                    RI.PutText(17, 54, "N");
                    RI.PutText(16, 45, DateTime.Today.ToString("MMddyy")); //update date
                    RI.PutText(19, 14, source); //update source
                }
                else
                {
                    RI.PutText(17, 54, "N");
                    RI.PutText(16, 45, RI.GetText(16, 45, 2)); //update date
                    RI.PutText(16, 48, RI.GetText(16, 48, 2)); //update date
                    RI.PutText(16, 51, RI.GetText(16, 51, 2)); //update date
                    RI.PutText(19, 14, source); //update source
                }
            }

            if(phoneNum != invalidNumber)
            {
                if(RI.CheckForText(17, 67, "_"))
                {
                    RI.PutText(17, 67, "", Key.EndKey);//unmark no phone indicator
                }
            }

            if(!isForeign && !RI.CheckForText(17,14,"_"))//if phone is not blank then clear foreign phone
            {
                RI.PutText(18, 15, "", Key.EndKey);
                RI.PutText(18, 24, "", Key.EndKey);
                RI.PutText(18, 36, "", Key.EndKey);
                RI.PutText(18, 53, "", Key.EndKey);
            }
            else if(isForeign && !RI.CheckForText(18,15,"_"))
            {
                RI.PutText(17, 14, "", Key.EndKey);
                RI.PutText(17, 23, "", Key.EndKey);
                RI.PutText(17, 31, "", Key.EndKey);
                RI.PutText(17, 40, "", Key.EndKey);
            }
            RI.Hit(Key.Enter);
        }

        private bool UpdatedEmail(string source, string emailType, string emailForUpdating, bool emailIndicator)
        {
            bool returnVal = false;
            RI.Hit(Key.F2);
            RI.Hit(Key.F10);

            //check to be sure the COMPASS Email address has something in it if the user blanked out the email
            if(emailForUpdating != "" || !RI.CheckForText(15, 10, "_"))
            {
                RI.PutText(10, 14, emailType, Key.Enter);
                RI.PutText(9, 20, source);
                if (RI.CheckForText(12, 14, "N") && !emailIndicator)
                {
                    RI.PutText(11, 17, RI.GetText(11, 17, 2));
                    RI.PutText(11, 20, RI.GetText(11, 20, 2));
                    RI.PutText(11, 23, RI.GetText(11, 23, 2));
                }
                else
                {
                    RI.PutText(11, 17, DateTime.Today.ToString("MMddyy"));
                }
                if(emailIndicator) //update email
                {
                    RI.PutText(14, 10, emailForUpdating.Trim(), Key.EndKey);
                    if(emailForUpdating.Length < 67)
                    {
                        RI.Hit(Key.EndKey);
                    }
                    RI.PutText(12, 14, "Y");
                }
                else
                {
                    RI.PutText(12, 14, "N");
                }
                RI.Hit(Key.Enter);
                if(!RI.CheckForText(23,2, "01005 RECORD SUCCESSFULLY CHANGED") && !RI.CheckForText(23,2,"01004"))
                {
                    returnVal = false;
                }
                returnVal = true;
            }

            RI.Hit(Key.F12);
            RI.Hit(Key.F2);
            return returnVal;
        }

        private void COMPASSInvalidateFirstAndAlternateAddrProc(string source, bool noPhone, DemographicVerifications demographicVerifications, MDBorrowerDemographics altAddress, bool isSchool, bool updateAddressIndicator, MDBorrowerDemographics demosForUpdating)
        {
            bool doAltAddressProcessing = false;
            if(altAddress != null)
            {
                doAltAddressProcessing = true;
            }

            if(source == "25")
            {
                source = "31";
            }

            if(doAltAddressProcessing || demographicVerifications.HasAnyInvalidateFirstSelections)
            {
                RI.FastPath($"TX3Z/CTX1J;{demosForUpdating.SSN}");
                //don't update COMPASS if error 01019 is returned
                if(RI.CheckForText(23,2,"01019"))
                {
                    return;
                }

                //switch to address info
                RI.Hit(Key.F6);
                RI.Hit(Key.F6);
                //alt address adding
                if(doAltAddressProcessing)
                {
                    if(isSchool)//if the contact was a school
                    {
                        if(!RI.CheckForText(8, 18, " "))
                        {
                            //only populate source if blank
                            RI.PutText(8, 18, source);
                            RI.PutText(9, 18, "02");
                        }
                        RI.PutText(10, 32, RI.GetText(10, 32, 2));
                        RI.PutText(10, 35, RI.GetText(10, 35, 2));
                        RI.PutText(10, 38, RI.GetText(10, 38, 2));
                        RI.PutText(11, 55, RI.GetText(11, 55, 1));
                    }
                    if(RI.CheckForText(8,13,"CODE"))
                    {
                        RI.PutText(8, 18, source);
                    }
                    UpdateCOMPASSAddress(altAddress);//add alt address info
                    RI.PutText(10, 32, DateTime.Today.ToString("MMddyy"));
                    RI.PutText(11, 55, "Y");//address is valid
                    RI.Hit(Key.Enter);
                    RI.Hit(Key.Enter);
                    updateAddressIndicator = true;//so the alt address is over written by the legal address
                }
                //invalidate first
                if(demographicVerifications.HasAnyInvalidateFirstSelections || noPhone)
                {
                    //address
                    if (demographicVerifications.Address == VerificationSelection.ValidWithChangeAndInvalidateFirst)
                    {
                        if(isSchool) //if the contact was a school
                        {
                            if (!RI.CheckForText(8, 18, " "))
                            {
                                //only populate source if blank
                                RI.PutText(8, 18, " ");
                                RI.PutText(9, 18, "02");
                            }
                            RI.PutText(10, 32, RI.GetText(10, 32, 2));
                            RI.PutText(10, 35, RI.GetText(10, 35, 2));
                            RI.PutText(10, 38, RI.GetText(10, 38, 2));
                            RI.PutText(11, 55, RI.GetText(11, 55, 1));
                        }
                        if(RI.CheckForText(8,13,"CODE"))
                        {
                            RI.PutText(8, 18, source);
                        }
                        RI.PutText(10, 32, DateTime.Today.ToString("MMddyy"));
                        RI.PutText(11, 55, "N", Key.Enter);//address is invalid
                    }
                    RI.Hit(Key.F6);
                    //home phone
                    if(demographicVerifications.HomePhone == VerificationSelection.ValidWithChangeAndInvalidateFirst || noPhone)
                    {
                        RI.PutText(16, 14, "H", Key.Enter);
                        RI.PutText(16, 45, DateTime.Today.ToString("MMddyy"));
                        RI.PutText(19, 14, source);
                        RI.PutText(17, 54, "N", Key.Enter); //phone is invalid
                    }
                    //other phone
                    if(demographicVerifications.OtherPhone == VerificationSelection.ValidWithChangeAndInvalidateFirst || noPhone)
                    {
                        RI.PutText(16, 14, "A", Key.Enter);
                        RI.PutText(16, 45, DateTime.Today.ToString());
                        RI.PutText(19, 14, source);
                        RI.PutText(17, 54, "N", Key.Enter); //other phone is invalid
                    }
                    //other other phone
                    if(demographicVerifications.OtherPhone2 == VerificationSelection.ValidWithChangeAndInvalidateFirst || noPhone)
                    {
                        RI.PutText(16, 14, "W", Key.Enter);
                        RI.PutText(16, 45, DateTime.Today.ToString("MMddyy"));
                        RI.PutText(19, 14, source);
                        RI.PutText(17, 54, "N", Key.Enter); //other phone is invalid
                    }
                    //email
                    if(demographicVerifications.Email == VerificationSelection.ValidWithChangeAndInvalidateFirst)
                    {
                        RI.Hit(Key.F2);
                        RI.Hit(Key.F10);
                        RI.PutText(10, 14, "H", Key.Enter);
                        RI.PutText(9, 20, source);
                        RI.PutText(11, 17, DateTime.Today.ToString("MMddyy"));
                        RI.PutText(12, 14, "N", Key.Enter); //email is invalid
                        RI.Hit(Key.F12);
                        RI.Hit(Key.F2);

                        if(demosForUpdating.EcorrCorrespondence || demosForUpdating.EcorrBilling || demosForUpdating.EcorrTax)
                        {
                            RI.Hit(Key.F2);
                            RI.Hit(Key.F10);
                            RI.PutText(10, 14, "C", Key.Enter);
                            RI.PutText(9, 20, source);
                            RI.PutText(11, 17, DateTime.Today.ToString("MMddyy"));
                            RI.PutText(12, 14, "N", Key.Enter); //email is invalid
                            RI.Hit(Key.F12);
                            RI.Hit(Key.F2);
                        }
                    }
                    //other email
                    if(demographicVerifications.OtherEmail == VerificationSelection.ValidWithChangeAndInvalidateFirst)
                    {
                        RI.Hit(Key.F2);
                        RI.Hit(Key.F10);
                        RI.PutText(10, 14, "A", Key.Enter);
                        RI.PutText(9, 20, source);
                        RI.PutText(11, 17, DateTime.Today.ToString("MMddyy"));
                        RI.PutText(12, 14, "N", Key.Enter); //email is invalid
                        RI.Hit(Key.F12);
                        RI.Hit(Key.F2);
                    }
                    //other email 2
                    if(demographicVerifications.OtherEmail2 == VerificationSelection.ValidWithChangeAndInvalidateFirst)
                    {
                        RI.Hit(Key.F2);
                        RI.Hit(Key.F10);
                        RI.PutText(10, 14, "W", Key.Enter);
                        RI.PutText(9, 20, source);
                        RI.PutText(11, 17, DateTime.Today.ToString("MMddyy"));
                        RI.PutText(12, 14, "N", Key.Enter); //email is invalid
                        RI.Hit(Key.F12);
                        RI.Hit(Key.F2);
                    }
                }
            }
        }

        private void UpdateCOMPASSAddress(MDBorrowerDemographics demosForUpdating)
        {
            RI.PutText(11, 10, demosForUpdating.Addr1.Trim());
            if(demosForUpdating.Addr1.Length < 30)
                RI.Hit(Key.EndKey); //all these if statements clear out info not typed over
            RI.PutText(12, 10, demosForUpdating.Addr2.Trim());
            if(demosForUpdating.Addr2.Length < 30)
                RI.Hit(Key.EndKey);
            RI.PutText(13, 10, "", Key.EndKey);
            RI.PutText(14, 8, demosForUpdating.City.Trim());
            if(demosForUpdating.City.Length < 20)
                RI.Hit(Key.EndKey);
            RI.PutText(14, 40, demosForUpdating.Zip.Trim().PadRight(9));

            if(!demosForUpdating.IsForeignAddress)
            {
                RI.PutText(14, 32, demosForUpdating.State.Trim());
                RI.PutText(12, 52, "", true); //blank foreign state
                RI.PutText(13, 52, "", true); //blank foreign country
                RI.PutText(12, 77, "", true);
            }
            else
            {
                RI.PutText(14, 32, "", true); //blank domestic state
                RI.PutText(12, 52, demosForUpdating.ForeignState);
                RI.PutText(13, 52, "", true); //blank foreign country
                RI.PutText(12, 77, MDIntermediary.Country.GetCountryCode(demosForUpdating.Country));
            }
        }
    }
}
