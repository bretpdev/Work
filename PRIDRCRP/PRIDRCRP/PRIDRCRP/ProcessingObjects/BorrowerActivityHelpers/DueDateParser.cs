using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace PRIDRCRP
{
    class DueDateParser
    {
        private string EffectiveDateNotice = "PAYMENTDUEDATECHANGELETTERSENTTOEDABORROWER";
        private string DueDateChangeNotice = "INSTALLMENTDUEDATECHANGEDTO";

        private string TypeHeader = "NOTICE TYPE";
        private string EffectiveDateHeader = "EFFECTIVEDATEWAS";
        private List<string> DueDateChangeNoticeType = new List<string>() { "ACMB" };

        private DataAccess DA;
        public DueDateParser(DataAccess DA)
        {
            this.DA = DA;
        }
        
        //A nullable DueDatePeriod is returned
        //When it is populated the last partial record needs to be added the DueDates and the 
        //returned DueDatePeriod needs to be set up as the new partial record
        public DueDatePeriod GetDueDateChange(BorrowerActivityResult result, DueDatePeriod partialPeriod)
        {
            DueDatePeriod retPeriod = GetDayChange(result, partialPeriod);
            //GetEffectiveDate(result, partialPeriod);
            return retPeriod;
        }

        public DueDatePeriod GetDayChange(BorrowerActivityResult result, DueDatePeriod partialPeriod)
        {
            string flatActivityDescription = StringParsingHelper.GetFlatString(result.ActivityDescription);
            if (flatActivityDescription.Contains(DueDateChangeNotice))
            {
                string effectiveDay = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, DueDateChangeNotice, new char[] { ' ' });
                int? day = effectiveDay.ToIntNullable();
                if (day.HasValue)
                {
                    if(partialPeriod.Day.HasValue)
                    {
                        partialPeriod.BeginDate = partialPeriod.BeginActivityDate;
                        DueDatePeriod newPeriod = new DueDatePeriod() { Day = day, BeginActivityDate = result.ActivityDate };
                        return newPeriod;
                    }
                    partialPeriod.BeginActivityDate = result.ActivityDate;
                    partialPeriod.BeginDate = result.ActivityDate;
                    partialPeriod.Day = day;
                    return null;
                }
            }
            return null;
        }

        public void GetEffectiveDate(BorrowerActivityResult result, DueDatePeriod partialPeriod)
        {
            string flatActivityDescription = StringParsingHelper.GetFlatString(result.ActivityDescription);
            if (flatActivityDescription.Contains(EffectiveDateNotice))
            {
                string noticeType = StringParsingHelper.ReadNextString(result.ActivityDescription, TypeHeader);
                string effectiveDate = StringParsingHelper.ReadNumericFromHeaderToString(flatActivityDescription, EffectiveDateHeader, new char[] { '/', ' ' });
                DateTime? date = effectiveDate.ToDateNullable();
                if (date.HasValue && DueDateChangeNoticeType.Contains(noticeType))
                {
                    partialPeriod.BeginDate = date;
                }
            }
        }

        public void GetPeriod(DueDatePeriod partialPeriod, DueDatePeriod previousPeriod)
        {
            DateTime? newEndDate = partialPeriod.BeginDate;
            if(previousPeriod.BeginDate < newEndDate && !previousPeriod.EndDate.HasValue)
            {
                newEndDate = newEndDate.Value.AddDays(-1);
            }
            previousPeriod.EndDate = newEndDate;
        }
    }
}
