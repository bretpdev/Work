using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace TRDPRTYRES
{
    class CompassProcessing
    {
        public bool HasOpenLoans { get; set; }
        public ReflectionInterface RI { get; set; }
        public DataAccess DA { get; set; }

        private enum RefDecision
        {
            AddNewRef,
            ModifyExsistingRef
        }

        public CompassProcessing(ReflectionInterface ri, BorReferenceInfo bData, DataAccess da)
        {
            RI = ri;
            DA = da;
            HasOpenLoans = DA.HasOpenLoans(bData.Ssn);
        }

        public bool? AddCompassReference(BorReferenceInfo bData)
        {
            if (!HasOpenLoans)
                return false;

            if (!bData.RefIsAuthed)
            {
                if (!Dialog.Info.YesNo("Has the 3rd party Authorization been approved for this reference?", "Test"))
                {
                    bData.RefIsAuthed = false;

                    if (!Dialog.Info.OkCancel("The borrower reference record may be added to the system, but the reference will not be approved for 3rd party authorization, and a 3rd Party Authorization Denial letter will be generated.", "NotApproved"))
                        return null;
                }
                else
                    bData.RefIsAuthed = true;
            }

            ReferenceInfoFrm infoFrm = new ReferenceInfoFrm(bData, false, DA);
            if (infoFrm.ShowDialog() == DialogResult.OK)
            {
                List<References> refData = DA.GetCompassReferences(bData.Ssn);
                bool possibleMatch = false;
                DialogResult result = DialogResult.None;
                foreach (var item in refData)
                {
                    string name = $"{bData.RefFirstName} {bData.RefLastName}";
                    if (name.ToUpper() == item.ReferenceName.ToUpper() && bData.ReferenceId.IsNullOrEmpty())
                    {
                        possibleMatch = true;
                        break;
                    }
                }
                if (!possibleMatch && bData.ReferenceId.IsNullOrEmpty())
                    result = DialogResult.Ignore; //Adding a new reference
                if (possibleMatch)
                {
                    PossibleMatchRefSelection refFrm = new PossibleMatchRefSelection(refData);
                    result = refFrm.ShowDialog();
                    if (result == DialogResult.OK)
                        LoadReferenceData(bData, refFrm.Rdata);
                }
                if (result == DialogResult.Ignore)
                    AddTheReference(bData, RefDecision.AddNewRef);
                else
                    AddTheReference(bData, RefDecision.ModifyExsistingRef);
                AddActivityComments(bData);
                return true;
            }
            return null;
        }

        private void LoadReferenceData(BorReferenceInfo bData, References rdata)
        {
            bData.ReferenceId = rdata.ReferenceId.Trim();
            bData.RefFirstName = rdata.FirstName.Trim();
            bData.RefMI = rdata.MiddleInitial.Trim();
            bData.RefLastName = rdata.LastName.Trim();
            bData.Address1 = rdata.Address1.Trim();
            bData.Address2 = rdata.Address2.Trim();
            bData.City = rdata.City.Trim();
            bData.State = rdata.State.Trim();
            bData.Zip = rdata.ZipCode.Trim();
            bData.HomePhone = rdata.HomePhone.Trim();
            bData.HomePhoneExt = rdata.HomeExt.Trim();
            bData.OtherPhone = rdata.AltPhone.Trim();
            bData.OtherPhoneExt = rdata.AltExt.Trim();
            bData.Foreign = rdata.ForeignPhone.Trim();
            bData.ForeignExt = rdata.ForeignExt.Trim();
            bData.Email = rdata.EmailAddress.Trim();
            bData.SourceCode = rdata.SourceCode;
            bData.RelationshipCode = rdata.RelationshipCode;
        }

        private void AddActivityComments(BorReferenceInfo bData)
        {
            AddComment(bData.Ssn, "M1REF");
            AddComment(bData.Ssn, bData.RefIsAuthed ? "X3RDC" : "X3RDD");
        }

        private void AddComment(string ssn, string arc)
        {
            ArcData arcData = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = ssn,
                Arc = arc,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = "",
                ScriptId = ThirdPartyAuthorization.Script
            };
            ArcAddResults result = arcData.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding ARC: {arc} on borrower account: {ssn}; EX: {result.Ex}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok(message);
            }
        }

        private void AddTheReference(BorReferenceInfo bData, RefDecision whatToDo)
        {
            if (whatToDo == RefDecision.AddNewRef)
                AddNewReference(bData);
            UpdateRelationship(bData, whatToDo);
            UpdateAuthorization(bData, whatToDo);
            AddRefAddress(bData, whatToDo);
            if (bData.HomePhone.IsPopulated())
                AddRefPhone(bData.NewReferenceID, bData.HomePhone, bData.HomePhoneExt, bData.SourceCode);
            if (bData.OtherPhone.IsPopulated())
                AddRefPhone(bData.NewReferenceID, bData.OtherPhone, bData.OtherPhoneExt, bData.SourceCode, true);
            if (bData.Foreign.IsPopulated())
                AddRefPhone(bData.NewReferenceID, bData.Foreign, bData.ForeignExt, bData.SourceCode, false, true);
            if (bData.Email.IsPopulated())
                AddRefEmail(bData);
        }

        private void AddNewReference(BorReferenceInfo bData)
        {
            RI.FastPath("TX3Z/ATX1JR");
            RI.PutText(4, 6, bData.RefLastName, true);
            RI.PutText(4, 34, bData.RefFirstName, true);
            RI.PutText(4, 53, bData.RefMI, true);
            RI.PutText(7, 11, bData.Ssn);
            RI.PutText(8, 49, bData.RefIsAuthed ? "Y" : "N");
            if (bData.RefIsAuthed)
                RI.PutText(8, 33, $"{DateTime.Now:MMddyy}");
        }

        private void UpdateRelationship(BorReferenceInfo bData, RefDecision whatToDo)
        {
            if (whatToDo == RefDecision.ModifyExsistingRef)
            {
                RI.FastPath($"TX3Z/CTX1JR;{(bData.NewReferenceID.IsPopulated() ? bData.NewReferenceID : bData.ReferenceId)}");
                RI.Hit(F6);
            }
            RI.PutText(8, 15, bData.RelationshipCode);
            if (whatToDo == RefDecision.ModifyExsistingRef)
            {
                RI.Hit(Enter);
                if (RI.MessageCode != "01094")
                {
                    string message = $"There was an error updating the reference relationship to borrower for reference: {bData.NewReferenceID}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    Dialog.Error.Ok(message);
                }
            }
        }

        private void UpdateAuthorization(BorReferenceInfo bData, RefDecision whatToDo)
        {
            if (whatToDo == RefDecision.ModifyExsistingRef)
            {
                RI.FastPath($"TX3Z/CTX1JR;{(bData.NewReferenceID.IsPopulated() ? bData.NewReferenceID : bData.ReferenceId)}");
                RI.Hit(F6);
            }
            if ((bData.RefIsAuthed && RI.GetText(8, 49, 1).IsIn("N", "_")) || (!bData.RefIsAuthed && RI.GetText(8, 49, 1).IsIn("Y", "_")))
            {
                RI.PutText(8, 49, bData.RefIsAuthed ? "Y" : "N");
                RI.PutText(8, 33, $"{DateTime.Now.Date:MMddyy}", true);
                if (whatToDo == RefDecision.ModifyExsistingRef)
                {
                    RI.Hit(Enter);
                    if (RI.MessageCode != "01094")
                    {
                        string message = $"There was an error updating the authorization for reference: {bData.NewReferenceID}";
                        RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        Dialog.Error.Ok(message);
                    }
                }
            }
        }

        private void AddRefAddress(BorReferenceInfo bData, RefDecision whatToDo)
        {
            if (whatToDo == RefDecision.ModifyExsistingRef)
            {
                RI.FastPath($"TX3Z/CTX1JR;{(bData.NewReferenceID.IsPopulated() ? bData.NewReferenceID : bData.ReferenceId)}");
                RI.Hit(F6, 2);
                RI.PutText(10, 32, $"{DateTime.Now:MMddyy}");
                RI.PutText(11, 55, "Y");
            }
            RI.PutText(11, 10, bData.Address1, true);
            RI.PutText(12, 10, bData.Address2, true);
            RI.PutText(14, 8, bData.City, true);
            RI.PutText(14, 32, bData.State, true);
            RI.PutText(14, 40, bData.Zip, Enter, true);
            bData.NewReferenceID = RI.GetText(1, 11, 9);
            if (!RI.MessageCode.IsIn("01096", "01004"))
            {
                string message = $"There was an error updating the address for reference: {bData.NewReferenceID}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok(message);
            }
        }

        private void AddRefPhone(string refId, string phone, string ext, string source, bool isOther = false, bool isForeign = false)
        {
            RI.FastPath($"TX3Z/CTX1JR;{refId}");
            RI.Hit(F6, 3);
            if (isOther)
            {
                RI.PutText(16, 14, "A");
                RI.Hit(Enter);
            }
            RI.PutText(16, 20, "U");
            RI.PutText(16, 30, "N");
            if (!isForeign)
            {
                RI.PutText(17, 14, phone);
                RI.PutText(17, 40, ext, true);
            }
            else
            {
                RI.PutText(18, 24, "", true);
                RI.PutText(18, 36, "", true);
                RI.PutText(18, 15, phone, true);
                RI.PutText(18, 53, ext, true);
            }
            RI.PutText(19, 14, source);
            RI.PutText(16, 45, $"{DateTime.Now:MMddyy}");
            RI.PutText(17, 54, "Y");
            RI.PutText(16, 78, "A", Enter);

            if (!RI.MessageCode.IsIn("01097", "01100"))
            {
                string message = $"There was an error updating the {(isOther ? "other " : "")}phone for reference: {refId}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok(message);
            }
        }

        private void AddRefEmail(BorReferenceInfo bData)
        {
            RI.FastPath($"TX3Z/CTX1JR;{bData.NewReferenceID}");
            RI.Hit(F2);
            RI.Hit(F10);
            RI.PutText(9, 20, bData.SourceCode, true);
            RI.PutText(11, 17, DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString(), true);
            RI.PutText(11, 20, DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString(), true);
            RI.PutText(11, 23, DateTime.Now.Year.ToString().SafeSubString(2, 2), true);
            RI.PutText(12, 14, "Y");
            RI.PutText(14, 10, bData.Email, Enter, true);
        }
    }
}