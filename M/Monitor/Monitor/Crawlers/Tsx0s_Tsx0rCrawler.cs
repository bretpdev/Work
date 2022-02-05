using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace Monitor
{
    class Tsx0s_Tsx0rCrawler : CrawlerBase
    {
        public Tsx0s_Tsx0rCrawler(ReflectionInterface ri, CrawlerBase parent)
            : base(ri, parent) { }
        public string SchedType { get; private set; }
        protected override bool LoadItemInternal()
        {
            //we need to load these in reverse order because each redisclosed
            //loan moves to the bottom of the list
            if (ri.MessageCode == "03459")
                throw new EncounteredCancelCodeException(ri.Message);
            if (MaxItemCount == null)
                TallyRecords();

            int totalNonPreSels = MaxItemCount.Value - tsx0sPreSelCounts.Count;
            if (CurrentItem <= totalNonPreSels)
            {
                //work a non-pre-sel
                if (ri.ScreenCode == "TSX0S")
                    ri.Hit(ReflectionInterface.Key.Enter);
                if (ri.MessageCode == "02877")
                    throw new EncounteredErrorCodeException(ri.Message);
                int count = 0;
                PageHelper.Iterate(ri, (row, settings) =>
                {
                    if (ri.CheckForText(row, 3, "_"))
                    {
                        count++;
                        if (count == CurrentItem)
                        {
                            SchedType = ri.GetText(row, 49, 2);
                            ri.PutText(row, 3, "X", ReflectionInterface.Key.Enter);
                            settings.ContinueIterating = false;
                        }
                    }
                }, GetSettings());
            }
            else
            {
                int count = 0;
                int tsx0sSelection = CurrentItem - totalNonPreSels;
                //we need to do these in reverse order, so invert the selection now
                tsx0sSelection = tsx0sPreSelCounts.Count - tsx0sSelection + 1;
                PageHelper.Iterate(ri, (row, settings) =>
                {
                    if (ri.CheckForText(row, 3, "_"))
                    {
                        count++;
                        if (count == tsx0sSelection)
                        {
                            ri.PutText(row, 3, "X", ReflectionInterface.Key.Enter);
                            SchedType = ri.GetText(10, 49, 2);
                            ri.Hit(ReflectionInterface.Key.Enter);
                            settings.ContinueIterating = false;
                        }
                    }
                }, GetSettings());
            }
            return true;
        }


        protected List<int> tsx0sPreSelCounts;
        private void TallyRecords()
        {
            int nonPreSelCount = 0;
            tsx0sPreSelCounts = new List<int>();
            if (ri.ScreenCode == "TSX0S")
            {
                int curCount = 0;
                PageHelper.Iterate(ri, (tRow, s) =>
                {
                    if (ri.CheckForText(tRow, 3, "_"))
                        curCount++;
                    if (curCount > tsx0sPreSelCounts.Count)
                    {
                        ri.PutText(tRow, 3, "X", ReflectionInterface.Key.Enter);
                        int count = 0;
                        nonPreSelCount = 0;
                        PageHelper.Iterate(ri, (row, settings) =>
                        {
                            if (ri.CheckForText(row, 3, "X"))
                                count++;
                            else if (ri.CheckForText(row, 3, "_"))
                                nonPreSelCount++;
                        }, GetSettings());
                        tsx0sPreSelCounts.Add(count);
                        ri.Hit(ReflectionInterface.Key.F12);
                        ri.PutText(tRow, 3, "_");
                    }
                }, GetSettings());
            }
            else if (ri.ScreenCode == "TSX0R")
            {
                PageHelper.Iterate(ri, (row, settings) =>
                {
                    if (ri.CheckForText(row, 3, "_"))
                        nonPreSelCount++;
                }, GetSettings());
            }

            //reset to beginning of TSX0S pages
            this.LoadParentRecursive();
            MaxItemCount = tsx0sPreSelCounts.Count + nonPreSelCount;
        }

        protected override CrawlerBase SpawnChildCrawler()
        {
            return null; //no more children
        }
    }
}
