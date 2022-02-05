using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;

namespace REQUETASK
{
    public class RequeueTask : ScriptBase
    {
        public RequeueTask(ReflectionInterface ri) : base(ri, "REQUETASK") { }
        public override void Main()
        {
            new RequeueTaskForm(RI).ShowDialog();
            EndDllScript();
        }
    }
}
