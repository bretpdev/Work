using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Q;
using SCHDEMOUP.Data;

namespace SCHDEMOUP.Data
{
    class DataAccess : DataAccessBase
    {
        private bool _testMode;

        public DataAccess(bool testMode)
        {
            _testMode = testMode;
        }

        internal string[] GetListOfStates()
        {
            string selectStr = @"SELECT Code from GENR_LST_States";
            return BsysDataContext(_testMode).ExecuteQuery<string>(selectStr).ToArray();
        }
    }
}
