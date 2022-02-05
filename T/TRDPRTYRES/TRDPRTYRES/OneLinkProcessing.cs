using System;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Dialog;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace TRDPRTYRES
{
    public class OneLinkProcessing
    {
        public ReflectionInterface RI { get; set; }
        public DataAccess DA { get; set; }

        public OneLinkProcessing(ReflectionInterface ri, DataAccess da)
        {
            RI = ri;
            DA = da;
        }

        public bool? AddOrModifyReference(BorReferenceInfo bData)
        {
            if (!Info.YesNo("Has the 3rd party Authorization been approved for this reference?", ""))
            {
                bData.RefIsAuthed = false;

                if (!Info.OkCancel("The borrower reference record may be added to the system, but the reference will not be approved for 3rd party authorization, and a 3rd Party Authorization Denial letter will be generated.", "NotApproved"))
                    return null;
            }
            else
                bData.RefIsAuthed = true;

            ReferenceInfoFrm infoFrm = new ReferenceInfoFrm(bData, true, DA);
            if (infoFrm.ShowDialog() == DialogResult.OK)
            {
                if (bData.ReferenceId.IsNullOrEmpty())
                {
                    RI.FastPath("LP2CA" + bData.Ssn);
                    PutTextOnLp2c(bData);

                    if (bData.NewReferenceID.IsNullOrEmpty())
                        return false;
                    if (Info.YesNo("Do you wish to add any additional comments?", "Additional Comments"))
                    {
                        AdditionalComments cmts = new AdditionalComments(bData);
                        cmts.ShowDialog();
                    }

                    string lp50Comment = bData.AdditionalComments ?? "";
                    AddComment(bData.Ssn, "AM", "10", "M1REF", lp50Comment);
                }
                else if (bData.ReferenceId.IsPopulated())
                {
                    RI.FastPath("LP2CC" + bData.Ssn);
                    if (!SelectReference(bData.ReferenceId))
                        return false;
                    PutTextOnLp2c(bData);
                }

                if (bData.BorHasValidAddr)
                    AddComment(bData.Ssn, "LT", "03", bData.RefIsAuthed ? "X3RDC" : "X3RDD", "");
                return true;
            }
            return null;
        }

        private bool SelectReference(string referenceId)
        {
            int row = 6;
            while (RI.AltMessageCode != "46004")
            {
                if (RI.CheckForText(row, 68, referenceId))
                {
                    RI.PutText(21, 13, RI.GetText(row, 3, 1), Enter, true);
                    return true;
                }
                row += 3;
                if (row > 15)
                {
                    RI.Hit(F8);
                    row = 6;
                }
            }
            Info.Ok($"The reference {referenceId} was not found for this borrower in LP2C. The update is not possible.");
            return false;
        }

        private void AddComment(string ssn, string type, string contact, string code, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = ssn,
                ActivityContact = contact,
                ActivityType = type,
                Arc = code,
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                Comment = comment,
                ScriptId = ThirdPartyAuthorization.Script
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding an {code} comment in LP50 for account: {ssn}; EX: {result.Ex}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Error.Ok(message);
            }
        }

        private void PutTextOnLp2c(BorReferenceInfo bData)
        {
            RI.PutText(4, 5, bData.RefLastName, true);
            RI.PutText(4, 44, bData.RefFirstName, true);
            RI.PutText(4, 60, bData.RefMI, true);
            RI.PutText(5, 9, bData.SourceCode, true);
            RI.PutText(6, 15, bData.RelationshipCode, true);
            RI.PutText(6, 51, $"{(bData.RefIsAuthed ? "Y" : "N")}", true);
            RI.PutText(8, 9, bData.Address1, true);
            RI.PutText(9, 9, bData.Address2, true);
            RI.PutText(10, 9, bData.City, true);
            RI.PutText(10, 52, bData.State, true);
            RI.PutText(10, 60, bData.Zip, true);
            RI.PutText(13, 16, bData.HomePhone, true);
            RI.PutText(13, 31, bData.HomePhoneExt, true);
            RI.PutText(14, 16, bData.OtherPhone, true);
            RI.PutText(14, 31, bData.OtherPhoneExt, true);
            RI.PutText(15, 18, bData.Foreign, true);
            RI.PutText(8, 53, "Y");
            RI.PutText(17, 9, bData.Email, Enter, true);
            RI.Hit(F6);
            if (RI.AltMessageCode == "48003")
                bData.NewReferenceID = RI.GetText(3, 14, 12);
        }
    }
}