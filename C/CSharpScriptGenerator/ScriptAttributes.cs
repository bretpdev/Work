namespace CSharpScriptGenerator
{
	public class ScriptAttributes
	{
		/// <summary>
		/// Indicates whether the script should be able to run from MauiDUDE.
		/// If true, the script generator will create a constructor that MauiDUDE can call.
		/// </summary>
		public bool CanRunFromMauiDude { get; set; }

		/// <summary>
		/// The full path to the C#Scripts directory, which contains Borg, BorgTest, and Scripts.
		/// </summary>
		public string CSharpCodeRoot
		{
			get
			{
				return DataAccess.CSharpCodeRoot;
			}

			set
			{
				DataAccess.CSharpCodeRoot = value;
			}
		}

		/// <summary>
		/// Globally unique identifier.
		/// </summary>
		public string Guid { get; set; }

		/// <summary>
		/// Indicates whether the script is a batch script/user batch script, rather than a user script.
		/// If true, the main class will inherit BatchScriptBase.
		/// If false, the main class will inherit ScriptBase.
		/// </summary>
		public bool IsBatchScript { get; set; }

		/// <summary>
		/// Indicates whether the script is intended for the Federal Direct region.
		/// If true, the solution will be created in the FedScripts directory.
		/// If false, the solution will be created in the FfelScripts directory.
		/// </summary>
		public bool IsFederalDirectScript { get; set; }

		/// <summary>
		/// The namespace to be used for the script, which doubles as the solution name.
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// The script's ID from Sacker.
		/// </summary>
		public string ScriptID { get; set; }

		/// <summary>
		/// The script's name from Sacker.
		/// </summary>
		public string ScriptName { get; set; }

		/// <summary>
		/// The name to use for the script's starting class.
		/// </summary>
		public string StartingClassName { get; set; }

        public bool IsCSharp { get; set; }
	}//clss
}//namespace
