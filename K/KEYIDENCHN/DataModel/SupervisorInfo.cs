using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;

namespace KEYIDENCHN
{
    public class SupervisorInfo
    {
        KeyIdentifierChange script;
        public SupervisorInfo(KeyIdentifierChange script)
        {
            this.script = script;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string DOB { get; set; }
        public string Comment { get; set; }
        public void LoadQueueTaskComment(ReflectionInterface ri)
        {
            string lines = ri.GetText(9, 1, 80) + " " + ri.GetText(10, 1, 80);
            string[] pieces = lines.Split(',');
            if (pieces.Length < 6)
                script.NotifyAndEnd("Parse error received retrieving data from S6/01 Supervisor Review queue task.  Data was: " + lines);
            FirstName = pieces[0].Trim();
            MiddleName = pieces[1].Trim();
            LastName = pieces[2].Trim();
            Suffix = pieces[3].Trim();
            DOB = pieces[4].Trim();
            Comment = pieces[5].Trim();
            if (pieces.Length > 6)
                Comment = string.Join(",", pieces.Skip(5).ToArray());
            if (Comment.Contains('{')) //script id is included
                Comment = Comment.Substring(0, Comment.IndexOf('{'));
        }
    }

}
