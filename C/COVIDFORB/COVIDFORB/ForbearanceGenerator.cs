using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;

namespace COVIDFORB
{
    class ForbearanceGenerator
    {
        public ProcessLogRun logRun { get; set; }
        public DataAccess DA { get; set; }

        public ForbearanceGenerator(ProcessLogRun logRun)
        { 
            this.logRun = logRun;
            DA = new DataAccess(logRun);
        }

        public void AddForbearances()
        {
            var population = DA.GetDelinquencyPopulation();
            var dates = DA.GetEndDate();

            foreach(var person in population)
            {
                bool clearDelq = false;
                var lastDate = person.DelinquencyOccurence;
                if(lastDate < dates.CARESStartDate)
                {
                    lastDate = dates.CARESStartDate;
                    clearDelq = true;
                }

                while (lastDate < dates.EndDate)
                {
                    var forbEnd = lastDate.AddDays(89);
                    if (forbEnd > dates.EndDate)
                    {
                        forbEnd = dates.EndDate;
                    }                  

                    var success = DA.InsertProcessingRecord(person.AccountNumber, lastDate, forbEnd, clearDelq, person.ComakerEligibility);
                    if(!success)
                    {
                        //For now we won't end if it fails
                        logRun.AddNotification($"Failed to add forbearance to ForbearanceProcessing for account: {person.AccountNumber} start date: {lastDate.Date.ToShortDateString()} end date: {forbEnd.Date.ToShortDateString()}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                    //Clear delinquency on the first successful insert
                    else if(!success && clearDelq)
                    {
                        clearDelq = true;
                    }
                    else
                    {
                        clearDelq = false;
                    }

                    lastDate = forbEnd.AddDays(1);
                }
            }
        }
    }
}
