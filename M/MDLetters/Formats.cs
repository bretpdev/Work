using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDLetters
{
    class Formats
    {
        public int CorrespondenceFormatId { get; set; }
        public string CorrespondenceFormat { get; set; }

        public static List<Formats> Populate()
        {
            return DataAccessHelper.ExecuteList<Formats>("GetCorrespondenceFormats", DataAccessHelper.Database.ECorrFed);
        }
    }
}
