using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.WinForms;
using Uheaa.Common.DataAccess;

namespace XmlGeneratorECorr
{
    class TagAttributeValueMapping
    {
        [Hidden, PrimaryKey]
        public int TagAttributeValueMappingId { get; set; }
        [Hidden]
        public int TagId { get; set; }
        [Hidden]
        public string Tag { get; set; }
        [Hidden]
        public int TagAttributeValueId { get; set; }
        [Hidden]
        public string Attribute { get; set; }
    }
}
