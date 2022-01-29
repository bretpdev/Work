using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
    public class DefaultValue : Attribute
    {
        public string Key { get; set; }
        public string Value
        {
            get
            {
                return DefaultValues[Key].ToString();
            }
        }

        private Dictionary<string, object> DefaultValues = new Dictionary<string, object>()
        {
            {"A", 0},
            {"C", " "},
            {"D", DateTime.Now.ToString("MM/dd/yyyy")},
            {"F", " "},
            {"G", null},//TODO look at this code.
            {"I", " "},
            {"M", " "},
            {"N", 0},
            {"R", 0},
            {"T", DateTime.Now.ToString("hh:mm:ss")},
            {"X", " "},
            {"S9", 0},//TODO This are not in the definition but in the document as values
            {"S9V9", 00}
        };
        public DefaultValue(string value)
        {
            Key = value;
        }
    }
}
