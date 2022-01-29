using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpScriptGenerator
{
	class DataAccess
	{
		private const string BSYS_LIVE = "Data Source=NOCHOUSE;Initial Catalog=BSYS;Integrated Security=True";
		private const string BSYS_TEST = "Data Source=OPSDEV;Initial Catalog=BSYS;Integrated Security=True";

		private static DataContext BsysLive { get { return new DataContext(BSYS_LIVE); } }
		private static DataContext BsysTest { get { return new DataContext(BSYS_TEST); } }

		public static string CSharpCodeRoot
		{
			get
			{
				return Properties.Settings.Default.CSharpCodeRoot;
			}
			set
			{
				Properties.Settings.Default.CSharpCodeRoot = value;
				Properties.Settings.Default.Save();
			}
		}//CSharpCodeRoot

		public static List<SackerScript> SackerScripts
		{
			get
			{
				string query = "SELECT ID AS Id, Script AS Name, ScriptType AS Type FROM SCKR_DAT_Scripts WHERE ID IS NOT NULL";
				return BsysLive.ExecuteQuery<SackerScript>(query).ToList();
			}
		}//SackerScripts

		public static void UpdateSackerCodeLocation(string projectName, string scriptName)
		{
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.Append("UPDATE SCKR_DAT_Scripts");
			commandBuilder.Append(" SET Module = 'CSharp',");
			commandBuilder.AppendFormat(" Subroutine = '{0}'", projectName);
			commandBuilder.AppendFormat(" WHERE Script = '{0}'", scriptName);
			BsysLive.ExecuteCommand(commandBuilder.ToString());
		}//UpdateSackerCodeLocation()

		public static void UpdateScriptTable(ScriptAttributes attributes)
		{
			//Insert a new record if the script isn't already in the table.
			string query = string.Format("SELECT COUNT(*) FROM DLLS_DAT_CSharpScript WHERE ScriptId = '{0}'", attributes.ScriptID);
			if (BsysTest.ExecuteQuery<int>(query).Single() == 0)
			{
				string region = (attributes.IsFederalDirectScript ? "CornerStone" : "UHEAA");
				StringBuilder commandBuilder = new StringBuilder();
				commandBuilder.Append("INSERT INTO DLLS_DAT_CSharpScript (ScriptId, ScriptName, AssemblyName, StartingNamespaceAndClass, Region)");
				commandBuilder.AppendFormat(" VALUES ('{0}', '{1}', '{2}', '{2}.{3}', '{4}')", attributes.ScriptID, attributes.ScriptName, attributes.Namespace, attributes.StartingClassName, region);
				BsysTest.ExecuteCommand(commandBuilder.ToString());
			}
		}//UpdateScriptTable()
	}//class

	#region Projection classes
	public class SackerScript
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public override string ToString() { return Id; }
	}//SackerScript
	#endregion Projection classes
}//namespace
