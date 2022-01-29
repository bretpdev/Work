using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    public class BaseTest
    {
        public TransactionScope Scope;

        [SetUp]
        public virtual void Init()
        {
            Scope = new TransactionScope();
            CentralTracking.LoginHelper.Login();
        }

        [TearDown]
        public virtual void CleanUp()
        {
            Scope.Dispose();
        }
    }
}
