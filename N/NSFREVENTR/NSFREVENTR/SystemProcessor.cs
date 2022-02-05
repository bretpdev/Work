using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;

namespace NSFREVENTR
{
    public abstract class SystemProcessor : ScriptSessionBase
    {

        protected TestModeResults _testModeResults;
        protected ReversalEntry _userEntry;

        public SystemProcessor(ReflectionInterface ri)
            : base(ri)
        {
        }

        /// <summary>
        /// Main starting point for processing.
        /// </summary>
        public abstract void ProcessEntry();

    }
}
