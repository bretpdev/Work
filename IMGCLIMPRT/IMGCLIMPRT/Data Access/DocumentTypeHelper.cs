using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace IMGCLIMPRT
{
    static class DocumentTypeHelper
    {
        private static Dictionary<string, string> CachedIds = new Dictionary<string, string>();
        [UsesSproc(DataAccessHelper.Database.DocId, "[imgclimprt].[GetDocIdFromDocumentType]")]
        public static string GetDocId(string documentTypeValue)
        {
            if (!CachedIds.ContainsKey(documentTypeValue))
            {
                var results = DataAccessHelper.ExecuteList<string>("[imgclimprt].[GetDocIdFromDocumentType]", DataAccessHelper.Database.DocId, SqlParams.Single("DocumentTypeValue", documentTypeValue));
                if (!results.Any())
                    return null;
                CachedIds[documentTypeValue] = results.First();
            }
            return CachedIds[documentTypeValue];
        }
    }
}
