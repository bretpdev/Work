using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDIntermediary
{
    public class MdLettersHelper
    {
        public List<MDLetters.Formats> AvailableFormats { get; private set; }
        public MdLettersHelper(IEnumerable<MDLetters.Formats> availableFormats)
        {
            this.AvailableFormats = availableFormats.ToList();
        }
        //[UsesSproc(DataAccessHelper.Database.ECorrFed, "[GetBorrowersAltFormat]")]
        //public MDLetters.Formats GetFormat(string accountNumber)
        //{
        //    var format = DataAccessHelper.ExecuteSingle<MDLetters.Formats>("[GetBorrowersAltFormat]", DataAccessHelper.Database.ECorrFed, SqlParams.Single("AccountNumber", accountNumber));
        //    return AvailableFormats.Single(o => o.CorrespondenceFormatId == format.CorrespondenceFormatId);
        //}

        //public void SetFormat(string accountNumber, MDLetters.Formats format)
        //{
        //    DataAccessHelper.Execute("[SetBorrowersAltFormat]", DataAccessHelper.Database.ECorrFed, SqlParams.Single("AccountNumber", accountNumber), SqlParams.Single("Format", format.CorrespondenceFormatId));
        //}
    }
}
