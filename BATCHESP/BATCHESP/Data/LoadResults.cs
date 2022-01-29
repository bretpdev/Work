using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BATCHESP
{
    public class LoadResults
    {
        public int NumberOfThreads { get; set; }
        public List<EspEnrollment> TasksToWork { get; set; }
        public bool LoadedSuccessfully { get; set; }

        public LoadResults(int numberOfThreads, List<EspEnrollment> tasksToWork, bool loadedSuccessfully)
        {
            NumberOfThreads = numberOfThreads;
            TasksToWork = tasksToWork;
            LoadedSuccessfully = loadedSuccessfully;
        }
    }
}
