using System;
using System.IO;
using Uheaa.Common.DataAccess;

namespace DEMUPDTFED
{
	class CustomRecovery
	{
		public enum Action
		{
			None,
			OpenedTask,
			ReassignedTask,
			UpdatedAddress,
			UpdatedHomePhone,
			UpdatedOtherPhone,
			ProcessedLocate
		}

		private const string ACTION_NONE = "";
		private const string ACTION_OPENED_TASK = "Opened task";
		private const string ACTION_REASSIGNED_TASK = "Reassigned task";
		private const string ACTION_UPDATED_ADDRESS = "Updated address";
		private const string ACTION_UPDATED_HOME_PHONE = "Updated home phone";
		private const string ACTION_UPDATED_OTHER_PHONE = "Updated other phone";
		private const string ACTION_PROCESSED_LOCATE = "Processed locate";

		public bool Exists { get { return !string.IsNullOrEmpty(_queue); } }

		private string _queue;
		public string Queue
		{
			get { return _queue; }
			set
			{
				_queue = value;
				UpdateFile();
			}
		}

		private Action _lastAction;
		public Action LastAction
		{
			get { return _lastAction; }
			set
			{
				_lastAction = value;
				UpdateFile();
			}
		}

		private readonly string _fileName;

		public CustomRecovery(string scriptId, string userId)
		{
			_fileName = string.Format("{0}{1}_{2}.txt", EnterpriseFileSystem.LogsFolder, scriptId, userId);
			if (File.Exists(_fileName))
			{
				string[] recoveryValues = File.ReadAllText(_fileName).Split(',');
				_queue = recoveryValues[0];
				switch (recoveryValues[1])
				{
					case ACTION_NONE:
						_lastAction = Action.None;
						break;
					case ACTION_OPENED_TASK:
						_lastAction = Action.OpenedTask;
						break;
					case ACTION_REASSIGNED_TASK:
						_lastAction = Action.ReassignedTask;
						break;
					case ACTION_UPDATED_ADDRESS:
						_lastAction = Action.UpdatedAddress;
						break;
					case ACTION_UPDATED_HOME_PHONE:
						_lastAction = Action.UpdatedHomePhone;
						break;
					case ACTION_UPDATED_OTHER_PHONE:
						_lastAction = Action.UpdatedOtherPhone;
						break;
					case ACTION_PROCESSED_LOCATE:
						_lastAction = Action.ProcessedLocate;
						break;
					default:
						throw new Exception(string.Format("According to the recovery file, the last action performed was \"{0},\" but I don't know what that means.", recoveryValues[1]));
				}
			}
		}

		public void Clear()
		{
			File.Delete(_fileName);
			_queue = null;
			_lastAction = Action.None;
		}

		private void UpdateFile()
		{
			string actionString;
			switch (_lastAction)
			{
				case Action.OpenedTask:
					actionString = ACTION_OPENED_TASK;
					break;
				case Action.ReassignedTask:
					actionString = ACTION_REASSIGNED_TASK;
					break;
				case Action.UpdatedAddress:
					actionString = ACTION_UPDATED_ADDRESS;
					break;
				case Action.UpdatedHomePhone:
					actionString = ACTION_UPDATED_HOME_PHONE;
					break;
				case Action.UpdatedOtherPhone:
					actionString = ACTION_UPDATED_OTHER_PHONE;
					break;
				case Action.ProcessedLocate:
					actionString = ACTION_PROCESSED_LOCATE;
					break;
				default:
					actionString = ACTION_NONE;
					break;
			}
			File.WriteAllText(_fileName, string.Format("{0},{1}", _queue, actionString));
		}
	}
}