using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;

namespace Monitor
{
    abstract class CrawlerBase
    {
        public CrawlerBase(ReflectionInterface ri, CrawlerBase parent)
        {
            this.ri = ri;
            this.ParentCrawler = parent;
        }
        public ReflectionInterface ri;
        public int CurrentItem { get; private set; }
        public int? MaxItemCount { get; protected set; }
        public CrawlerBase ChildCrawler { get; private set; }
        public CrawlerBase ParentCrawler { get; private set; }
        public bool HasNextItem()
        {
            bool result = CurrentItem < MaxItemCount || MaxItemCount == null;
            if (result)
                return true;
            else
            {
                if (ChildCrawler != null)
                    result = ChildCrawler.HasNextItem();
                return result;
            }
        }
        public bool NextItem()
        {
            if (ChildCrawler != null)
            {
                if (ChildCrawler.HasNextItem())
                {
                    LoadItem();
                    if (ChildCrawler.NextItem())
                        return true;
                    this.LoadParentRecursive();
                }
            }
            if (CurrentItem < MaxItemCount || MaxItemCount == null)
                CurrentItem++;
            else
                return false;
            var afterLoadResult = LoadItem();
            ChildCrawler = SpawnChildCrawler();
            if (!afterLoadResult)
                return false;
            if (ChildCrawler != null)
                return ChildCrawler.NextItem();
            else
                return true;
        }
        public virtual bool LoadItem()
        {
            if (CurrentItem > MaxItemCount)
                return false;
            if (CurrentItem == 0)
                return false;
            bool result = false;
            if (ri.ScreenCode == this.ScreenCode || this.ScreenCode == null)
                result = LoadItemInternal();
            else //screen was skipped automatically because there is only one selection
            {
                result = CurrentItem == 1;
                MaxItemCount = 1;
            }

            return result;
        }
        protected void LoadParentRecursive(bool recCall = false)
        {
            if (ParentCrawler != null)
                ParentCrawler.LoadParentRecursive(true);
            if (recCall)
                LoadItem();
        }
        protected abstract bool LoadItemInternal();
        public virtual string ScreenCode { get { return null; } }
        protected abstract CrawlerBase SpawnChildCrawler();

        protected PageHelper.IterationSettings GetSettings()
        {
            PageHelper.IterationSettings settings = new PageHelper.IterationSettings()
            {
                ContinueIterating = true,
                MinRow = 10,
                MaxRow = 22,
                RowIncrementValue = 1,
                SkipToNextPage = false
            };
            settings.TerminatingMessageCodes.Add("90007"); //shows up on pages of lists
            settings.TerminatingMessageCodes.Add("46004"); //shows up on pages of data (no selection list)
            return settings;
        }
    }
}
