using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SerfFIleValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(@"C:\SERF_File\");
            foreach (string file in files)
            {
                CheckForDups(file);
            }
        }

        private static void CheckForOverlappingDefermentForbearance(string file)
        {
            Dictionary<int, DateTime> defermentBegin = new Dictionary<int, DateTime>();
            Dictionary<int, DateTime> defermentEnd = new Dictionary<int, DateTime>();
            Dictionary<int, DateTime> forbBegin = new Dictionary<int, DateTime>();
            Dictionary<int, DateTime> forbEnd = new Dictionary<int, DateTime>();

            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    string currentLine = sr.ReadLine();
                    if (currentLine.Substring(32, 2) != "09") //Deferment
                    {
                        defermentBegin.Add(int.Parse(currentLine.Substring(235, 4)), DateTime.Parse(currentLine.Substring(55, 10)));
                        defermentEnd.Add(int.Parse(currentLine.Substring(235, 4)), DateTime.Parse(currentLine.Substring(65, 10)));
                    }
                    else if (currentLine.Substring(32, 2) != "09") //Forbearance
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Checks the given file for duplicate records excludes Student Records (08)
        /// </summary>
        /// <param name="file"></param>
        private static void CheckForDups(string file)
        {
            List<string> prevLines = new List<string>();
            using (StreamReader sr = new StreamReader(file))
            {
                string previousLine = "";
                string currentLine = "";
                while (!sr.EndOfStream)
                {
                    currentLine = sr.ReadLine();
                    if (previousLine != "" && currentLine.Substring(38) == previousLine.Substring(38) && currentLine.Substring(32, 2) != "08") //Excluding student records
                        throw new Exception("Dup");

                    previousLine = currentLine;
                    if (currentLine.Substring(32, 2) != "08")//Excluding student records.
                        prevLines.Add(currentLine.Substring(23, 9) + " " + currentLine.Substring(32, 2) + " " + currentLine.Substring(38)); //SSN type and then line
                }
            }
            List<string> distinct = prevLines.Distinct().ToList();
            if (prevLines.Count() != distinct.Count())
            {
                List<string> dups = prevLines.GroupBy(s => s).Where(o => o.Count() > 1).Select(p => p.Key).ToList();
                using (StreamWriter sw = new StreamWriter(@"T:\Duplicate_Align_Values.txt"))
                {
                    foreach (string item in dups)
                        sw.WriteLine(item);
                }
                throw new Exception("Dup");
            }
        }
    }
}
