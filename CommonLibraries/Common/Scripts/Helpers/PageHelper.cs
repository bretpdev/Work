using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.Scripts
{
    public static class PageHelper
    {
        public static void Iterate(ReflectionInterface ri, Action<int> processRow)
        {
            Iterate(ri, (row, settings) => processRow(row));
        }

        public static void IteratePagesOnly(ReflectionInterface ri, Action<IterationSettings> processPage)
        {
            var settings = IterationSettings.Default();
            settings.MaxRow = settings.MinRow; //don't worry about rows

            Iterate(ri, (row, set) => processPage(set), settings);
        }

        public static void Iterate(ReflectionInterface ri, Action<int, IterationSettings> processRow)
        {
            Iterate(ri, processRow, IterationSettings.Default());
        }

        public static void Iterate(ReflectionInterface ri, Action<int> processRow, IterationSettings settings)
        {
            Iterate(ri, (row, s) => processRow(row), settings);
        }

        public static void Iterate(ReflectionInterface ri, Action<int, IterationSettings> processRow, IterationSettings settings)
        {
            while (!settings.TerminatingMessageCodes.Contains(ri.MessageCode) && !settings.TerminatingMessageCodes.Contains(ri.AltMessageCode))
            {
                for (int row = settings.MinRow; row <= settings.MaxRow; row += settings.RowIncrementValue)
                {
                    if (string.IsNullOrEmpty(ri.GetText(row, 1, 80)))
                        continue;
                    processRow(row, settings);
                    if (!settings.ContinueIterating)
                        return;
                    if (settings.SkipToNextPage)
                    {
                        settings.SkipToNextPage = false;
                        break;
                    }
                }
                ri.Hit(ReflectionInterface.Key.F8);
                settings.CurrentPage++;
            }
            ri.Hit(ReflectionInterface.Key.F7);
            ri.Hit(ReflectionInterface.Key.F8);
            if (ri.MessageCode == "01033") //more data on next set of 20 pages
            {
                ri.Hit(ReflectionInterface.Key.Enter);
                Iterate(ri, processRow, settings);
            }
            else
                ri.Hit(ReflectionInterface.Key.F8); //get our 90007 error message back

        }

        public class IterationSettings
        {
            public bool ContinueIterating { get; set; }
            public bool SkipToNextPage { get; set; }
            public int MinRow { get; set; }
            public int MaxRow { get; set; }
            public int RowIncrementValue { get; set; }
            public int CurrentPage { get; set; }
            public HashSet<string> TerminatingMessageCodes { get; set; }
            public IterationSettings()
            {
                TerminatingMessageCodes = new HashSet<string>();
                CurrentPage = 1;
            }


            public static IterationSettings Default()
            {
                IterationSettings settings = new IterationSettings()
                {
                    ContinueIterating = true,
                    MinRow = 8,
                    MaxRow = 20,
                    RowIncrementValue = 1,
                    SkipToNextPage = false
                };
                settings.TerminatingMessageCodes.Add("90007"); //shows up on pages of lists
                settings.TerminatingMessageCodes.Add("46004"); //shows up on pages of data (no selection list)
                return settings;
            }
        }
    }
}
