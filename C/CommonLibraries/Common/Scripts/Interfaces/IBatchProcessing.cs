using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.Scripts
{
    public interface IBatchProcessing
    {
        int CreateThreads();
        void ProcessThread(BatchProcessingHelper userIdAndPassword);
    }
}
