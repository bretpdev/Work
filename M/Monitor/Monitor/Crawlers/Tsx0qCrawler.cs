using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace Monitor
{
    class Tsx0qCrawler : CrawlerBase
    {
        public Tsx0qCrawler(ReflectionInterface ri, CrawlerBase parent)
            : base(ri, parent) { }
        public override string ScreenCode
        {
            get { return "TSX0Q"; }
        }
        protected override bool LoadItemInternal()
        {
            if (ri.MessageCode == "03459")
                throw new EncounteredCancelCodeException(ri.Message);
            int count = 0;
            int selection = 0;
            PageHelper.Iterate(ri, (row, settings) =>
            {
                int? sel = ri.GetText(row, 18, 2).ToIntNullable();
                if (sel.HasValue)
                {
                    count++;
                    if (count == CurrentItem)
                        selection = sel.Value;
                }
                else
                    settings.ContinueIterating = false;
            });
            if (count > 0)
            {
                ri.PutText(21, 12, selection.ToString(), ReflectionInterface.Key.Enter);
                MaxItemCount = count;
                return true;
            }
            return false;
        }
        protected override CrawlerBase SpawnChildCrawler()
        {
            return new Tsx0s_Tsx0rCrawler(ri, this);
        }
    }
}
