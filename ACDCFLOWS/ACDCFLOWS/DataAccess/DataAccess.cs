using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Data.SqlClient;
using Q;
using System.Text;

namespace ACDCFlows
{
	public class DataAccess : DataAccessBase
	{
		public ConfigurationMode _mode;
		public DataContext _dc;
		public MoveDirection Direction { get; set; }

		public enum MoveDirection
		{
			Up,
			Down
		}

		public DataAccess(ConfigurationMode mode)
		{
			_mode = mode;
			_dc = CSYSDataContext(mode);
		}

		/// <summary>
		/// Checks for existence of a flow and adds it to the db if doesn't exist else it throws a FlowAlreadyExistsInDatabaseException.
		/// </summary>
		/// <param name="flowToAdd"></param>
		public void AddFlow(Flow flowToAdd)
		{
			string selectStr = "EXEC spFLOW_CheckForExistenceOfFlow {0}";
			if (_dc.ExecuteQuery<int>(selectStr, flowToAdd.FlowID).First() > 0)
			{
				throw new FlowAlreadyExistsInDatabaseException();
			}
			string insertStr = "EXEC spFLOW_AddFlow {0}, {1}, {2}, {3}, {4}";
			_dc.ExecuteCommand(insertStr, flowToAdd.FlowID, flowToAdd.Description, flowToAdd.ControlDisplayText, flowToAdd.UserInterfaceDisplayIndicator, flowToAdd.System);
		}

		/// <summary>
		/// Updates the flow
		/// </summary>
		/// <param name="flow">Flow object</param>
		public void ChangeFlow(Flow flow)
		{
			try
			{
				_dc.ExecuteCommand("EXEC spFLOW_ChangeFlow {0}, {1}, {2}, {3}, {4}", flow.FlowID, flow.System, flow.Description, flow.ControlDisplayText, flow.UserInterfaceDisplayIndicator);
			}
			catch (Exception)
			{
				throw new Exception();
			}
		}

		/// <summary>
		/// Gets all Flows from the system.
		/// </summary>
		/// <returns></returns>
		public List<FlowIDs> GetFlowIDs(string system)
		{
			try
			{
				return _dc.ExecuteQuery<FlowIDs>("EXEC spFLOW_GetFlows {0}", system).ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Unknown exception thrown", ex);
			}
		}

		/// <summary>
		/// Get the flow data for the specified Flow ID
		/// </summary>
		/// <param name="flowID"></param>
		/// <returns></returns>
		public Flow GetFlowData(string flowID)
		{
			return _dc.ExecuteQuery<Flow>("EXEC spFLOW_GetSpecifiedFlow {0}", flowID).SingleOrDefault();
		}

		/// <summary>
		/// Gets all systems listed for portal.
		/// </summary>
		/// <returns></returns>
		public List<string> GetSystems()
		{
			return _dc.ExecuteQuery<string>("EXEC spGENR_GetSystemList").ToList();
		}

		/// <summary>
		/// Returns a list of the available interfaces built into each system
		/// </summary>
		/// <returns></returns>
		public List<SystemInterfaces> GetInterfaces(string system)
		{
			List<SystemInterfaces> interfaces = _dc.ExecuteQuery<SystemInterfaces>("EXEC spFLOW_GetInterfaces {0}", system).ToList();
			interfaces.Insert(0, new SystemInterfaces() { Interface = "", System = "" });
			return interfaces;
		}

		public IEnumerable<FlowStep> GetFlowSteps(string flowID)
		{
			return _dc.ExecuteQuery<FlowStep>("EXEC spFLOW_GetStepsForFlow {0}", flowID).ToList();
		}

		public int GetNextFlowSequenceNumber(string flowID)
		{
			return _dc.ExecuteQuery<int>("EXEC spFLOW_StepCountForFlow {0}", flowID).FirstOrDefault();
		}

		public void AddFlowStep(string insertFlowStep, List<object> param)
		{
			_dc.ExecuteCommand(insertFlowStep, param.ToArray());
		}

		public List<Key> GatherListOfKeys(string system, string type, string keyword, string keywordFieldFilter)
		{
			return _dc.ExecuteQuery<Key>("EXEC spSYSA_GatherKeyDataBasedOffFilterCriteria {0}, {1}, {2}, {3}", system, type, keyword, keywordFieldFilter).ToList();
		}

		//Gets a list of all the active users
		public IEnumerable<SqlUser> GetUsers()
		{
			return _dc.ExecuteQuery<SqlUser>("EXEC spSYSA_GetSqlUsers");
		}

		public IEnumerable<FlowStepInfoForUserSearch> GetUserFlowStep(int id)
		{
			return _dc.ExecuteQuery<FlowStepInfoForUserSearch>("EXEC spFLOW_GetFlowStepsForUser {0}", id);
		}

		public List<string> GetStaffCalculationID(string system)
		{
			return _dc.ExecuteQuery<string>("EXEC spFLOW_GetCalculationID {0}", system).ToList();
		}

		public List<string> GetDataValidation(string system)
		{
			return _dc.ExecuteQuery<string>("EXEC spFLOW_GetDataValidation {0}", system).ToList();
		}

		public List<string> GetAccessKeys()
		{
			return _dc.ExecuteQuery<string>("EXEC spFLOW_GetAccessKeys").ToList();
		}

		public List<string> GetStatus()
		{
			return _dc.ExecuteQuery<string>("EXEC spFLOW_GetStatus").ToList();
		}

		private List<FlowStep> GetStepsForSpecifiedFlow(string flowID)
		{
			string selectStr = "EXEC spFLOW_GetStepsForFlow {0}";
			return _dc.ExecuteQuery<FlowStep>(selectStr, flowID).ToList();
		}

		public void MoveStep(FlowStep stepToMove, MoveDirection direction)
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
				_dc.ExecuteCommand(updateStr, stepToMove.FlowID, stepToMove.FlowStepSequenceNumber, (stepToMove.FlowStepSequenceNumber + directionTranslation));
			}
			else
			{
				//move step in slot to -1 
				_dc.ExecuteCommand(updateStr, stepToMove.FlowID, (stepToMove.FlowStepSequenceNumber + directionTranslation), -1);
				//move step to move into now vacant slot
				_dc.ExecuteCommand(updateStr, stepToMove.FlowID, stepToMove.FlowStepSequenceNumber, (stepToMove.FlowStepSequenceNumber + directionTranslation));
				//move step in temp slot of -1 to now vacant slot
				_dc.ExecuteCommand(updateStr, stepToMove.FlowID, -1, stepToMove.FlowStepSequenceNumber);
			}
		}

		/// <summary>
		/// Deletes step and reorders steps that come after it 
		/// </summary>
		/// <param name="stepToDelete"></param>
		public void DeleteStep(FlowStep stepToDelete)
		{
			//delete step
			string deleteStr = "EXEC spFLOW_DeleteFlowStep {0}, {1}";
			_dc.ExecuteCommand(deleteStr, stepToDelete.FlowID, stepToDelete.FlowStepSequenceNumber);
			//move all existing steps up one to fill in the gap that was created by the deletion
			List<FlowStep> existingLaterSteps = (from f in GetStepsForSpecifiedFlow(stepToDelete.FlowID)
												 where f.FlowStepSequenceNumber > stepToDelete.FlowStepSequenceNumber
												 select f).ToList();
			foreach (FlowStep step in existingLaterSteps)
			{
				MoveStep(step, MoveDirection.Up);
			}
		}

		/// <summary>
		/// Updates specified flow
		/// </summary>
		/// <param name="stepToUpdate"></param>
		public void UpdateFlowStep(FlowStep step)
		{
			//add step to end on the flow steps
			List<object> param = new List<object>();
			StringBuilder updateStr = new StringBuilder();
			int placeHolderCount = 6;
			param.Add(step.FlowID);
			param.Add(step.FlowStepSequenceNumber);
			param.Add(step.AccessAlsoBasedOffBusinessUnit);
			param.Add(step.ControlDisplayText);
			param.Add(step.Description);
			param.Add(step.Status);
			updateStr.Append("EXEC spFLOW_UpdateFlowStep @FlowID = {0}, @FlowStepSequenceNumber = {1}, @AccessAlsoBasedOffBusinessUnit = {2}, @ControlDisplayText = {3}, @Description = {4}, @Status = {5}");
			if (!string.IsNullOrEmpty(step.AccessKey))
			{
				updateStr.AppendFormat(", @AccessKey = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(step.AccessKey);
				placeHolderCount++;
			}
			if (!string.IsNullOrEmpty(step.NotificationType))
			{
				updateStr.AppendFormat(", @NotificationType = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(step.NotificationType);
				placeHolderCount++;
			}
			if (step.StaffAssignment != 0)
			{
				updateStr.AppendFormat(", @StaffAssignment = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(step.StaffAssignment);
				placeHolderCount++;
			}
			if (!string.IsNullOrEmpty(step.StaffAssignmentCalculationID))
			{
				updateStr.AppendFormat(", @StaffAssignmentCalculationID = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(step.StaffAssignmentCalculationID);
				placeHolderCount++;
			}
			if (!string.IsNullOrEmpty(step.DataValidationID))
			{
				updateStr.AppendFormat(", @DataValidationID = {0}{1}{2}", "{", placeHolderCount, "}");
				param.Add(step.DataValidationID);
				placeHolderCount++;
			}
			_dc.ExecuteCommand(updateStr.ToString(), param.ToArray());
		}

	}
}
