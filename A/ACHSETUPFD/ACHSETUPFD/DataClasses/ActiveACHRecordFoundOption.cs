using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACHSETUPFD
{
    public class ActiveACHRecordFoundOption
    {

        public enum Option
        {
            Add,
            Change,
            Stop
        }

        public Option SelectedOption { get; set; }

    }
}
