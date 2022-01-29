using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.DocumentProcessing
{
    public abstract class BatchPrintingBase
    {
        public BatchPrintingBase(bool UsesSession = false)
        {
        }

        public virtual void Image()
        {
        }

        public virtual void Print()
        {
        }

        public virtual void AddArc(ArcData arc)
        {

        }
    }
}
