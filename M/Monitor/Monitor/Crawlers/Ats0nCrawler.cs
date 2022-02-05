using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace Monitor
{
    class Ats0nCrawler
    {
        ReflectionInterface ri;
        R0Task task;
        public Ats0nCrawler(ReflectionInterface ri, R0Task task)
        {
            this.ri = ri;
            this.task = task;
            tsx0p = new Tsx0pCrawler(ri, task.Ssn);
        }

        Tsx0pCrawler tsx0p;
        /// <summary>
        /// Moves to the next Tsx0t entry.  Returns the captured SCHED TYPE if applicable, or NULL if done crawling.
        /// </summary>
        public Tsx0tResult MoveToNextTsx0t()
        {
            var result = new Tsx0tResult();
            try
            {
                if (tsx0p.NextItem())
                {
                    string schedType = (tsx0p.ChildCrawler.ChildCrawler as Tsx0s_Tsx0rCrawler).SchedType;
                    schedType = string.IsNullOrWhiteSpace(schedType) ? "L" : schedType;
                    result.SchedType = schedType;
                }
            }
            catch (EncounteredErrorCodeException ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ri.MessageCode;
            }
            return result;
        }

        public class Tsx0tResult
        {
            public string SchedType { get; set; }
            public string ErrorMessage { get; set; }
            public bool HasError { get { return !string.IsNullOrWhiteSpace(ErrorMessage); } }
            public string ErrorCode { get; set; }
        }
    }
}
