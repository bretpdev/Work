using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Uheaa.Common;

namespace MyCornerstoneLoan
{
    public class BannerHelper
    {
        public BannerHelper(HttpServerUtilityBase server)
        {
            this.BannerFileLocation = server.MapPath("~/_MCLBanner.csv");
        }
        public string GetCurrentBannerText()
        {
            try
            {
                var items = CsvHelper.ParseTo<BannerItem>(File.ReadAllLines(BannerFileLocation));
                var matching = items.ValidLines.Where(o => o.ParsedEntity.StartDate <= DateTime.Now && o.ParsedEntity.EndDate >= DateTime.Now);
                string message = string.Join("<br />", matching.Select(o => o.ParsedEntity.BannerText));
                if (string.IsNullOrEmpty(message))
                    message = null;
                return message;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string BannerFileLocation { get; private set; }
    }

    public class BannerItem
    {
        public string BannerText { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}