using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using keys = Uheaa.Common.DataAccess.EnterpriseFileSystem;

namespace I1I2SCHLTR
{
    class DataFiles
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }

        public List<PrintData> Prints { get; set; } = new List<PrintData>();
        public List<QueueTaskData> QueueTasks { get; set; } = new List<QueueTaskData>();
        public List<BorrowerData> Comments { get; set; } = new List<BorrowerData>();
        public List<SchoolData> Schools { get; set; } = new List<SchoolData>();

        public DataFiles(ProcessLogRun logRun, DataAccess DA)
        {
            this.LogRun = logRun;
            this.DA = DA;

            DA.AddWork();

            InitializePrints();
            InitializeQueueTasks();
            InitializeComments();
        }

        private void InitializePrints()
        {
            Prints = DA.GetUnprocessedPrintData();

            //Initialize the schools to the info from the R2
            foreach(PrintData data in Prints)
            {
                if(!Schools.Select(p => p.School).Contains(data.School) && data.SchoolStatus != "c")
                {
                    Schools.Add(new SchoolData() { School = data.School });
                }
            }
        }

        private void InitializeQueueTasks()
        {
            QueueTasks = DA.GetUnprocessedQueueTaskData();
        }

        private void InitializeComments()
        {
            Comments = DA.GetUnprocessedCommentData();

            //Make sure that all of the schools in the borrower information are in the schools object
            foreach(BorrowerData data in Comments)
            {
                foreach(Schools school in data.Schools)
                {
                    if(!Schools.Select(p => p.School).Contains(school.School) && school.SchoolStatus != "c")
                    {
                        Schools.Add(new SchoolData() { School = school.School });
                    }
                }
            }
        }

    }//Class
}//Namespace
