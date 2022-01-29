using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDatabaseApi_SyncDatabaseToCode
{
    class CompiledScript
    {
        public List<string> Lines { get; internal set; } = new List<string>();
        public void InsertController(ControllerInfo ci)
        {
            Add("INSERT INTO webapi.Controllers (ControllerId, Name)");
            Add($"VALUES ({(int)ci.ControllerId}, '{ci.ControllerName}')");
            Add();
        }

        public void InsertControllerAction(ControllerInfo ci, string action)
        {
            Add($"--add new action '{action}' to controller '{ci.ControllerName}'");
            Add("INSERT INTO webapi.ControllerActions (ControllerId, ActionName)");
            Add($"VALUES ({(int)ci.ControllerId}, '{action}')");
            Add();
        }

        public void RetireController(ControllerInfo ci)
        {
            Add($"--retiring unused controller {ci.ControllerName}");
            Add("UPDATE webapi.Controllers");
            Add("SET RetiredAt = GETDATE()");
            Add($"WHERE ControllerId = {(int)ci.ControllerId}");
            Add();
        }

        public void RetireControllerAction(ControllerInfo ci, string action)
        {
            Add("UPDATE webapi.ControllerActions");
            Add("SET RetiredAt = GETDATE()");
            Add($"WHERE ControllerId = {(int)ci.ControllerId} AND ActionName = '{action}'");
            Add();
        }

        public void RenameController(ControllerInfo ci, string oldName)
        {
            Add($"--renaming from '{oldName}' to '{ci.ControllerName}'");
            Add("UPDATE webapi.Controllers");
            Add($"SET [Name] = '{ci.ControllerName}'");
            Add($"WHERE ControllerId = {(int)ci.ControllerId}");
            Add();
        }

        private void Add(string content = "")
        {
            var split = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in split)
                Lines.Add(line.Trim());
        }

        public string[] GetCompiledScriptLines()

        {
            var lines = Lines.ToArray().ToList(); //local copy
            if (lines.Count == 0)
                lines.Add("--no database changes necessary");
            lines.Insert(0, "GO");
            lines.Insert(0, "USE UheaaWebManagement");

            return lines.ToArray();

        }
    }
}
