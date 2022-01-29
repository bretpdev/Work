using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace Monitor
{
    class Tsx0pCrawler : CrawlerBase
    {
        public Tsx0pCrawler(ReflectionInterface ri, string ssn)
            : base(ri, null)
        {
            this.Ssn = ssn;
        }
        private string Ssn { get; set; }
        public override string ScreenCode
        {
            get { return "TSX0P"; }
        }

        public override bool LoadItem()
        {
            if (CurrentItem > MaxItemCount)
                return false;
            ri.FastPath("tx3z/ats0n" + Ssn);
            if (ri.ScreenCode == "TSX0O")
                throw new EncounteredErrorCodeException(ri.Message); //borrower not found, normally shouldn't encounter this error
            else if (ri.MessageCode == "03459" || ri.MessageCode == "01020")
                throw new EncounteredCancelCodeException(ri.Message);
            return base.LoadItem();
        }

        protected override bool LoadItemInternal()
        {
            int count = 0;
            int sel = 0;
            PageHelper.Iterate(ri, (row, settings) =>
            {
                var selection = ri.GetText(row, 4, 2).ToIntNullable();
                if (selection.HasValue)
                {
                    count++;
                    if (count == CurrentItem)
                        sel = selection.Value;
                }
                else
                    settings.ContinueIterating = false;
            });
            if (count > 0)
            {
                ri.PutText(21, 12, sel.ToString(), ReflectionInterface.Key.Enter);
                MaxItemCount = count;
                if (ri.ScreenCode == "TSX0O")
                    throw new EncounteredErrorCodeException(ri.Message);
                return true;
            }
            return false;
        }

        protected override CrawlerBase SpawnChildCrawler()
        {
            return new Tsx0qCrawler(ri, this);
        }
    }
}
