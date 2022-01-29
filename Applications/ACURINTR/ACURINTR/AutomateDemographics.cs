using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Windows.Forms;
using ACURINTR.DemographicsProcessors;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACURINTR
{
    public class AutomateDemographics
    {
        public AutomateDemographics(string scriptId, ProcessLogData data)
        {
            this.ScriptId = scriptId;
            this.LogData = data;
        }
        private ReflectionInterface RI { get; set; }
        private string ScriptId { get; set; }
        private string UserId { get; set; }
        private ProcessLogData LogData { get; set; }

        /// <summary>
        /// Returns 

        /// </summary>
        /// <returns></returns>
        public bool Process(QueueData data)
        {
            bool pauseOnQueueClosingError = false;

            //Grab some variables that we only need to set once.
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            ProcessQueue(data, UserId, assemblyName, pauseOnQueueClosingError);
            return true;
        }

        /// <summary>
        /// Start processorBase for the queue that we pass in.
        /// </summary>
        /// <param name="queueData"></param>
        /// <param name="userId"></param>
        /// <param name="assemblyName"></param>
        /// <param name="pauseOnQueueClosingError"></param>
        private void ProcessQueue(QueueData queueData, string userId, string assemblyName, bool pauseOnQueueClosingError)
        {
            //Instantiate the class specified by the queue data and process the queue.
            string fullyQualifiedProcessor = string.Format("{0}.DemographicsProcessors.{1}", assemblyName, queueData.Processor);
            ProcessorBase queueProcessor = (ProcessorBase)Activator.CreateInstance(assemblyName, fullyQualifiedProcessor, false, BindingFlags.Default, null, new Object[] { RI, userId, ScriptId, LogData }, null, new Object[] { }).Unwrap();
            queueProcessor.Process(queueData, assemblyName, pauseOnQueueClosingError);
        }

        /// <summary>
        /// Load a Reflection Interface.
        /// </summary>
        /// <param name="batch">BatchProcessingHelper</param>
        /// <returns>true if successful</returns>
        public bool Login(BatchProcessingHelper batch, DataAccessHelper.Region region)
        {
            DataAccessHelper.CurrentRegion = region;
            RI = new ReflectionInterface();
            return RI.Login(batch.UserName, batch.Password);
        }

        internal void Close()
        {
            RI.CloseSession();
        }
    }
}
