using System;

namespace Uheaa.Common.CommonScreens
{
    public class CsvHeaderNameAttribute : Attribute
    {
        public string HeaderName { get; set; }
        public CsvHeaderNameAttribute(string headerName) { HeaderName = headerName; }
    }
}
