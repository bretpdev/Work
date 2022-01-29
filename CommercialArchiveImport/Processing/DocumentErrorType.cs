using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommercialArchiveImport
{
    public enum DocumentErrorType
    {
        Unspecified,
        MoreThan11,
        LessThan11,
        UnknownDocType,
        MultipleReference,
        Duplicate,
        BadTif,
        NoImage
    }
}
