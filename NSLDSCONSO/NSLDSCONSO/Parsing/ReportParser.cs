using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSLDSCONSO
{
    public class ReportParser
    {
        public ReportParser(string fileContents)
        {
            var lines = fileContents.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> currentSection = null;
            foreach (var line in lines)
            {
                if (line.StartsWith("SECTION "))
                {
                    string numeral = line.Substring(8, 1);
                    var matchingProp = typeof(ReportParser).GetProperties().SingleOrDefault(o => o.Name == "Section" + numeral);
                    if (matchingProp != null)
                        currentSection = (List<string>)matchingProp.GetValue(this);
                }
                if (currentSection != null)
                {
                    if (line.StartsWith("SECTION") || line.StartsWith(",,,,,,,,,,,,,") || line.StartsWith("INITIAL BOOKING INFORMATION"))
                        continue;
                    currentSection.Add(line);
                }
            }
        }
        public List<string> Section1 { get; private set; } = new List<string>();
        public List<string> Section2 { get; private set; } = new List<string>();
        public List<string> Section3 { get; private set; } = new List<string>();
        public List<string> Section4 { get; private set; } = new List<string>();
    }
}
