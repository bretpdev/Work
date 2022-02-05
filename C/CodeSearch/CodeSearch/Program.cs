using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace CodeSearch
{
    class Program
    {
        // starting search directory (will traverse subdirectories starting here)
        const string searchStartPath = @"C:\Users\bpehrson\Desktop\Web Apps\paweb\LearningCenter\";
        // location where search results will be saved
        const string resultsSavePath = @"t:\SearchResults.txt";
        
        static void Main(string[] args)
        {
            // array of terms to search for
            #region Search Terms
            // terms to search for within files
            var searchTerms = new[] 
            {
                "procauto", "ProcAuto"
            };
            #endregion  //Search Terms

            // only files with the following extensions will be included in search for terms ("." is required)
            var searchExtensions = new[] {/*".vb", ".cs", ".frm", ".bas", ".sas"*/ ".jsp", ".js", ".java", ".class", ".xml"};

            DirectoryInfo dir = new DirectoryInfo(searchStartPath);

            Console.WriteLine("Started Processing");
            Console.WriteLine("Creating list of files to be searched.");

            var files = dir.GetFiles("*.*", SearchOption.AllDirectories)
                           .Where(file => searchExtensions.Contains(file.Extension));
            Console.WriteLine(String.Format("{0} files will be searched.", files.Count()));

            Console.WriteLine("Searching files for terms.");
            // list of files that contain one or more search terms
            var searchResults = files.Where(file => searchTerms.Any(term => File.ReadAllText(file.FullName).Contains(term)))
                .Select(file => file.FullName);

            Console.WriteLine(String.Format("Search terms found in {0} files.", searchResults.Count()));
            List<SearchResults> sResult = new List<SearchResults>();

            Console.WriteLine("Locating search term locations within files.");
            // cycle through each file that contain one of the search terms and add the results to SearchResults object
            // results include term found, line term was found on, and file name 
            foreach (string file in searchResults)
            {
                Console.WriteLine(String.Format("Searching file:  {0}", file));
                string[] lines = File.ReadAllLines(file);
                foreach (string term in searchTerms)
                {
                    if (lines.Any(line => line.Contains(term)))
                    {
                        SearchResults tmpResults = new SearchResults() { File = file, Term = term, LineNumber = Array.FindIndex(lines, line => line.Contains(term)) + 1 };
                        Console.WriteLine(String.Format("Found Term:  {0};      Line:  {1};     File:  {2}", tmpResults.Term, tmpResults.LineNumber, tmpResults.File));
                        sResult.Add(tmpResults);
                    }
                }
            }

            Console.WriteLine(String.Format("Search resultes are being saved here:  {0}", resultsSavePath));

            // write results to file
            using (StreamWriter sw = new StreamWriter(resultsSavePath))
            {
                 foreach (SearchResults result in sResult)
                {
                    sw.WriteLine(String.Format("Term:  {0};     Line:  {1};     File:  {2}", result.Term, result.LineNumber, result.File));
                }
            }

            Console.WriteLine("Finished Processing\nPress enter to end the applicaiton.");
            Console.ReadLine();
        }
    }
}
