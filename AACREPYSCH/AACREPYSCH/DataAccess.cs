using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Q;

namespace AACREPYSCH
{
    class DataAccess : DataAccessBase
    {
		private DataContext CLS { get { return ClsDataContext(_testMode); } }
		private readonly bool _testMode;
		
		public DataAccess(bool testMode)
		{
			_testMode = testMode;
		}
    }
}
