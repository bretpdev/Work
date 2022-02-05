using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Uheaa.Common.WebApi
{
    public class WebApiResult
    {
        public bool Successful
        {
            get
            {
                return !string.IsNullOrEmpty(ExceptionText);
            }
        }
        public DataSet Result { get; set; }
        public string ExceptionText { get; set; }
    }
}
