using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.SqlClient;

namespace UHEAAOperationsTrackingSystems
{
	public class FlowControlDataAccess : DataAccessBase
	{

		public enum MoveDirection
		{
			Up,
			Down
		}

		private const string CALCULATION_OPTION = "Use Listed Calculation";

		#region "Flows"

		/// <summary>
		/// Checks for existence of a flow and adds it to the db if doesn't exist else it throws a FlowAlreadyExistsInDatabaseException.
		/// </summary>
		/// <param name="flowToAdd"></param>
		public static void AddFlow(Flow flowToAdd)
		{
			string selectStr = "EXEC spFLOW_CheckForExistenceOfFlow {0}";
			if (CSYSDataContext.ExecuteQuery<int>(selectStr, flowToAdd.FlowID).First() > 0)
			{
				throw new FlowAlreadyExistsInDatabaseException("Flow already exist in database.");
			}
			string insertStr = "EXEC spFLOW_AddFlow {0}, {1}, {2}, {3}, {4}";
			CSYSDataContext.ExecuteCommand(insertStr, flowToAdd.FlowID, flowToAdd.Description, flowToAdd.ControlDisplayText, flowToAdd.UserInterfaceDisplayIndicator, flowToAdd.System);
		}

		/// <summary>
		/// Gets all Flows from the system.
		/// </summary>
		/// <returns></returns>
		public static List<TextValueOption> GetFlowIDs()
		{
			List<TextValueOption> tv;
			try
			{
				string selectStr = "EXEC spFLOW_GetFlows";
				tv = CSYSDataContext.ExecuteQuery<TextValueOption>(selectStr).ToList();
			}
			catch (SqlException ex)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw;
			}
			return tv;
		}

		/// <summary>
		/// Updates Flow in the DB.
		/// </summary>
		/// <param name="flowToChange"></param>
		public static void ChangeFlow(Flow flowToChange)
		{
			string updateStr = "EXEC spFLOW_ChangeFlow {0}, {1}, {2}, {3}, {4}";
			CSYSDataContext.ExecuteCommand(updateStr, flowToChange.FlowID, flowToChange.System, flowToChange.Description, flowToChange.ControlDisplayText, flowToChange.UserInterfaceDisplayIndicator);
		}

		#endregion

		#region "Flow Steps"

		/// <summary>
		/// Adds a step to a given flow
		/// </summary>
		/// <param name="newStep"></param>
		public static void AddStepToFlow(FlowStep newStep)
		{
			//get the step count from the flow
			string selectFlowCountStr = "EXEC spFLOW_StepCountForFlow {0}";
			newStep.FlowStepSequenceNumber = (CSYSDataContext.ExecuteQuery<int>(selectFlowCountStr, newStep.FlowID).First() + 1);

			//add step to end on the flow steps
			List<object> param = new List<object>();
			StringBuilder insertFlowStepSB = new StringBuilder();
			int placeHolderCount = 6;
			param.Add(newStep.FlowID);
			param.Add(newStep.FlowStepSequenceNumber);
			param.Add(newStep.AccessAlsoBasedOffBusinessUnit);
			param.Add(newStep.ControlDisplayText);
			param.Add(newStep.Description);
			param.Add(newStep.Status);
			insertFlowStepSB.Append("EXEC spFLOW_AddStepToFlow @FlowID = {0}, @FlowStepSequenceNumber = {1}, @AccessAlsoBasedOffBusinessUnit = {2}, @ControlDisplayText = {3}, @Description = {4}, @Status = {5}");
			if (newStep.AccessKey != DEFAULT_DROP_DOWN_OPTION)
			{
				insertFlowStepSB.AppendFormat(", @AccessKey = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(newStep.AccessKey);
				placeHolderCount++;
			}
			if (newStep.NotificationKey != DEFAULT_DROP_DOWN_OPTION)
			{
				insertFlowStepSB.AppendFormat(", @NotificationKey = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(newStep.NotificationKey);
				placeHolderCount++;
			}
			if (newStep.StaffAssignment != 0)
			{
				insertFlowStepSB.AppendFormat(", @StaffAssignment = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(newStep.StaffAssignment);
				placeHolderCount++;
			}
			if (!string.IsNullOrEmpty(newStep.StaffAssignmentCalculationID))
			{
				insertFlowStepSB.AppendFormat(", @StaffAssignmentCalculationID = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(newStep.StaffAssignmentCalculationID);
				placeHolderCount++;
			}
			if (!string.IsNullOrEmpty(newStep.DataValidationID))
			{
				insertFlowStepSB.AppendFormat(", @DataValidationID = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(newStep.DataValidationID);
				placeHolderCount++;
			}
			CSYSDataContext.ExecuteCommand(insertFlowStepSB.ToString(), param.ToArray());
		}


		/// <summary>
		/// Moves a flow step up or down.
		/// </summary>
		/// <param name="stepToMove"></param>
		/// <param name="direction"></param>
		public static void MoveStep(FlowStep stepToMove, MoveDirection direction)
		{
			int directionTranslation = (direction == MoveDirection.Down ? 1 : -1);
			//check if there is a step directly before/above or after/below the step to move or if the step location is vacant
			List<FlowStep> existingSteps = (from f in GetStepsForSpecifiedFlow(stepToMove.FlowID)
											where f.FlowStepSequenceNumber == (stepToMove.FlowStepSequenceNumber + directionTranslation)
											select f).ToList();
			string updateStr = "EXEC spFLOW_ChangeFlowSequenceNumber {0}, {1}, {2}";
			if (existingSteps.Count == 0)
			{
				//If there isn't a step in the slot then just move it into the vacant slot
				CSYSDataContext.ExecuteCommand(updateStr, stepToMove.FlowID, stepToMove.FlowStepSequenceNumber, (stepToMove.FlowStepSequenceNumber + directionTranslation));
			}
			else
			{
				//move step in slot to -1 
				CSYSDataContext.ExecuteCommand(updateStr, stepToMove.FlowID, (stepToMove.FlowStepSequenceNumber + directionTranslation), -1);
				//move step to move into now vacant slot
				CSYSDataContext.ExecuteCommand(updateStr, stepToMove.FlowID, stepToMove.FlowStepSequenceNumber, (stepToMove.FlowStepSequenceNumber + directionTranslation));
				//move step in temp slot of -1 to now vacant slot
				CSYSDataContext.ExecuteCommand(updateStr, stepToMove.FlowID, -1, stepToMove.FlowStepSequenceNumber);
			}
		}

		/// <summary>
		/// Deletes step and reorders steps that come after it 
		/// </summary>
		/// <param name="stepToDelete"></param>
		public static void DeleteStep(FlowStep stepToDelete)
		{
			//delete step
			string deleteStr = "EXEC spFLOW_DeleteFlowStep {0}, {1}";
			CSYSDataContext.ExecuteCommand(deleteStr, stepToDelete.FlowID, stepToDelete.FlowStepSequenceNumber);
			//move all existing steps up one to fill in the gap that was created by the deletion
			List<FlowStep> existingLaterSteps = (from f in GetStepsForSpecifiedFlow(stepToDelete.FlowID)
												 where f.FlowStepSequenceNumber > stepToDelete.FlowStepSequenceNumber
												 select f).ToList();
			foreach (FlowStep item in existingLaterSteps)
			{
				MoveStep(item, MoveDirection.Up);
			}
		}

		/// <summary>
		/// Updates specified flow
		/// </summary>
		/// <param name="stepToUpdate"></param>
		public static void UpdateStep(FlowStep stepToUpdate)
		{
			//add step to end on the flow steps
			List<object> param = new List<object>();
			StringBuilder updateStr = new StringBuilder();
			int placeHolderCount = 6;
			param.Add(stepToUpdate.FlowID);
			param.Add(stepToUpdate.FlowStepSequenceNumber);
			param.Add(stepToUpdate.AccessAlsoBasedOffBusinessUnit);
			param.Add(stepToUpdate.ControlDisplayText);
			param.Add(stepToUpdate.Description);
			param.Add(stepToUpdate.Status);
			updateStr.Append("EXEC spFLOW_UpdateFlowStep @FlowID = {0}, @FlowStepSequenceNumber = {1}, @AccessAlsoBasedOffBusinessUnit = {2}, @ControlDisplayText = {3}, @Description = {4}, @Status = {5}");
			if (stepToUpdate.AccessKey != DEFAULT_DROP_DOWN_OPTION)
			{
				updateStr.AppendFormat(", @AccessKey = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(stepToUpdate.AccessKey);
				placeHolderCount++;
			}
			if (stepToUpdate.NotificationKey != DEFAULT_DROP_DOWN_OPTION)
			{
				updateStr.AppendFormat(", @NotificationKey = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(stepToUpdate.NotificationKey);
				placeHolderCount++;
			}
			if (stepToUpdate.StaffAssignment != 0)
			{
				updateStr.AppendFormat(", @StaffAssignment = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(stepToUpdate.StaffAssignment);
				placeHolderCount++;
			}
			if (!string.IsNullOrEmpty(stepToUpdate.StaffAssignmentCalculationID))
			{
				updateStr.AppendFormat(", @StaffAssignmentCalculationID = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(stepToUpdate.StaffAssignmentCalculationID);
				placeHolderCount++;
			}
			if (!string.IsNullOrEmpty(stepToUpdate.DataValidationID))
			{
				updateStr.AppendFormat(", @DataValidationID = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(stepToUpdate.DataValidationID);
				placeHolderCount++;
			}
			CSYSDataContext.ExecuteCommand(updateStr.ToString(), param.ToArray());
		}

		public static List<FlowStep> GetAndTweakStepsForSpecifiedFlow(string flowID)
		{
			List<FlowStep> results = GetStepsForSpecifiedFlow(flowID);
			List<FlowStep> newResults = new List<FlowStep>();
			foreach (FlowStep item in results)
			{
				if (item.AccessKey == null)
				{
					item.AccessKey = DEFAULT_DROP_DOWN_OPTION;
				}
				if (item.NotificationKey == null)
				{
					item.NotificationKey = DEFAULT_DROP_DOWN_OPTION;
				}
				if (item.StaffAssignment == 0)
				{
					if (item.StaffAssignmentCalculationID == null)
					{
						item.StaffAssignmentCalculationID = string.Empty;
					}
				}
				else
				{
					item.StaffAssignmentCalculationID = string.Empty;
				}
				newResults.Add(item);
			}
			return newResults;
		}

		#endregion

		#region "System Flow Stuff"

		public static List<Flow> GetFlowsForSystem(string system)
		{
			List<Flow> flow;
			try
			{
				string selectStr = "EXEC spFLOW_GetFlowsForSpecifiedSystem {0}";
				flow = CSYSDataContext.ExecuteQuery<Flow>(selectStr, system).ToList();
			}
			catch (SqlException ex)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw;
			}
			return flow;
		}

		#endregion

		#region "User Flow Stuff"

		public static List<FlowStepInfoForUserSearch> GetUsersFlowInfo(string user)
		{
			List<FlowStepInfoForUserSearch> flowStep;
			try
			{
				string selectStr = "EXEC spFLOW_GetFlowStepsForUser {0}";
				flowStep = CSYSDataContext.ExecuteQuery<FlowStepInfoForUserSearch>(selectStr, user).ToList();
			}
			catch (SqlException ex)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw;
			}
			return flowStep;
		}

		#endregion



	}
}
