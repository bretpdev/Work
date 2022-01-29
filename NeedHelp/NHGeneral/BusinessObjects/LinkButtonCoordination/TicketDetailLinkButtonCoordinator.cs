using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHGeneral
{
    internal class TicketDetailLinkButtonCoordinator
    {

        public const string EDIT_TEXT = "Edit";
        public const string SAVE_TEXT = "Save";
        public const string HOLD_TEXT = "Hold";
        public const string RELEASE_TEXT = "Release";

        public static readonly List<string> TICKET_TYPES_THAT_EVERYONE_CAN_SUBMIT;
        public const string SUBMIT_ACCESS_KEY = "Need Help Submit";

        static TicketDetailLinkButtonCoordinator()
        {
            TICKET_TYPES_THAT_EVERYONE_CAN_SUBMIT = new List<string>(new string[] { "FAC", "SASR", "BAC" });
        }

        /// <summary>
        /// Changes enabling and disabling based off flow step/useraccess/ticket set to be edited or locked/etc.  This code requires the edit/save button text to be set appropriately before it is called.
        /// </summary>
        /// <param name="buttonObject"></param>
        public static bool CalculateLinkButtonAppearanceAndStatus(DataAccess dataAccess, SqlUser user, TicketDetailLinkButtonData buttonObject)
        {
			string system = "Need Help General Help";

            try
            {
                //calculate correct button text for hold/release and change status buttons
                CalculateHoldReleaseAndChangeStatusLinkButtonText(dataAccess, buttonObject);
            }
            catch (Exception)
            {
                return false;
            }
            //check if ticket is completed/withdrawn or open
            if (buttonObject.StatusChanger.Text.Length == 0) //either the ticket was completed or withdrawn
            {
                //completed or withdrawn
                buttonObject.Hold.Enabled = false;
                buttonObject.Return.Enabled = false;
                buttonObject.StatusChanger.Enabled = false;
                buttonObject.Withdraw.Enabled = false;
                buttonObject.UpdateLink.Enabled = true;
            }
            else
            {
                //still open
                buttonObject.Hold.Enabled = true;
                buttonObject.Return.Enabled = true;
                buttonObject.StatusChanger.Enabled = true;
                buttonObject.Withdraw.Enabled = true;
                buttonObject.UpdateLink.Enabled = true;
            }

            //if ticket is on hold
            if (buttonObject.Hold.Text == RELEASE_TEXT)
            {
                //on hold
                //be sure the actions that shouldn't be allowed when ticket is on hold aren't allowed
                buttonObject.Return.Enabled = false;
                buttonObject.StatusChanger.Enabled = false;
                buttonObject.Withdraw.Enabled = false;
            }

            if (buttonObject.TheTicket.Data.TheTicketData.Status != Ticket.ON_HOLD_STATUS_TEXT && buttonObject.TheTicket.Data.TheTicketData.Status != Ticket.WITHDRAWN_STATUS_TEXT) //ticket isn't on hold
            {
                //if ticket type can be submitted by everyone then don't remove access.
                if (((from t in TICKET_TYPES_THAT_EVERYONE_CAN_SUBMIT
                      where t == buttonObject.TheTicket.Data.TheTicketData.TicketCode
                      select t).Count() <= 0) || buttonObject.TheTicket.Data.TheFlowStep.AccessKey != SUBMIT_ACCESS_KEY)
                {
                    //check if user whould have access to perform the flow step
                    if (buttonObject.TheTicket.Data.TheFlowStep.AccessAlsoBasedOffBusinessUnit)
                    {
                        //include business unit in access logic
                        if (dataAccess.HasAccess(buttonObject.TheTicket.Data.TheFlowStep.AccessKey, system, user) == false)
                            buttonObject.StatusChanger.Enabled = false; //user shouldn't have access
                    }
                    else
                    {
                        //only need to check for access regardless of business unit stuff
                        if (dataAccess.HasAccess(buttonObject.TheTicket.Data.TheFlowStep.AccessKey, system, user) == false)
                            buttonObject.StatusChanger.Enabled = false; //user shouldn't have access
                    }
                }
            }

            return true;

        }

        //changes text on hold/release button and change status button
        private static void CalculateHoldReleaseAndChangeStatusLinkButtonText(DataAccess dataAccess, TicketDetailLinkButtonData buttonObject)
        {
            if (buttonObject.TheTicket.Data.TheTicketData.Status != Ticket.ON_HOLD_STATUS_TEXT) //ticket isn't on hold
            {
                buttonObject.Hold.Text = TicketDetailLinkButtonCoordinator.HOLD_TEXT;
                if (buttonObject.TheTicket.Data.TheTicketData.Status != Ticket.WITHDRAWN_STATUS_TEXT)
                    buttonObject.StatusChanger.Text = buttonObject.TheTicket.Data.TheFlowStep.ControlDisplayText;
                else
                    buttonObject.StatusChanger.Text = string.Empty;
            }
            else //Ticket is on hold
            {
                buttonObject.Hold.Text = TicketDetailLinkButtonCoordinator.RELEASE_TEXT;
                //get button text for previous status
                List<FlowStep> steps = dataAccess.GetStepsForSpecifiedFlow(buttonObject.TheTicket.Data.TheFlowData.FlowID);
                try
                {
                    FlowStep previousFlowStep = (from s in steps
                                                 where s.Status == buttonObject.TheTicket.Data.TheTicketData.PreviousStatus
                                                 select s).FirstOrDefault();
                    buttonObject.StatusChanger.Text = previousFlowStep.ControlDisplayText;
                }
                catch (Exception)
                {
                    string previousStatus = dataAccess.GetPreviousStatusFromHistoryTable(buttonObject.TheTicket.Data.TheTicketData.TicketID);
                    FlowStep previousFlowStep = (from s in steps
                                                 where s.Status == previousStatus
                                                 select s).FirstOrDefault();
                    buttonObject.StatusChanger.Text = buttonObject.StatusChanger.Text = previousFlowStep.ControlDisplayText;
                }

            }
        }

    }
}