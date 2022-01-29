using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public enum TaskResult { CompleteTask, CancelTask, SkipTask }
    public static class TaskResultExtensions
    {
        public static TaskResultInfo ToInfo(this TaskResult tr)
        {
            var result = new TaskResultInfo();
            if (tr == TaskResult.CancelTask)
            {
                result.Status = 'X';
                result.ActionResponse = "CANCL";
                result.Verb = "Cancel";
            }
            else if (tr == TaskResult.CompleteTask)
            {
                result.Status = 'C';
                result.ActionResponse = "COMPL";
                result.Verb = "Complete";
            }
            else if (tr == TaskResult.SkipTask)
            {
                result.Verb = "Skip";
            }
            return result;
        }
    }
    public class TaskResultInfo
    {
        public TaskResult Result { get; set; }
        public char Status { get; set; }
        public string ActionResponse { get; set; }
        public string Verb { get; set; }
    }
}
