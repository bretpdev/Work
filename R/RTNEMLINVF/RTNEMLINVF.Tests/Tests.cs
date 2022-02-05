using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTNEMLINVF;

namespace RTNEMLINVF.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestUserByRegionEquality()
        {
            UserByRegion user1 = new UserByRegion("1", true);
            UserByRegion user2 = new UserByRegion("1", true);
            UserByRegion nullUser1 = null;
            UserByRegion nullUser2 = null;
            UserByRegion user3 = new UserByRegion("2", true);
            UserByRegion user4 = new UserByRegion("1", false);

            var comparer = new UserByRegionComparer();

            //Object is equal to itself
            Assert.IsTrue(comparer.Equals(user1, user1));

            //Object is equal to an equivalent object
            Assert.IsTrue(comparer.Equals(user1, user2));

            //Two null objects are equal
            Assert.IsTrue(comparer.Equals(nullUser1, nullUser2));

            //The Objects are not equal if the Ssn is different
            Assert.IsFalse(comparer.Equals(user1, user3));

            //The Objects are not equal if the Onelink flag is different
            Assert.IsFalse(comparer.Equals(user1, user4));

            //Objects are not equal when they differ in Ssn and Onelink flag
            Assert.IsFalse(comparer.Equals(user3, user4));

            //Non-Null Objects are not equal to null objects
            Assert.IsFalse(comparer.Equals(user1, nullUser1));

        }

        [TestMethod]
        public void TestUserByRegionHash()
        {
            UserByRegion user1 = new UserByRegion("1", true);
            UserByRegion user2 = new UserByRegion("1", true);
            UserByRegion user3 = new UserByRegion("2", true);
            UserByRegion user4 = new UserByRegion("1", false);

            var comparer = new UserByRegionComparer();

            //Making sure the hash function gets different results for different users
            //and the same value for the same user
            Assert.IsTrue(comparer.GetHashCode(user1) == comparer.GetHashCode(user1));
            Assert.IsTrue(comparer.GetHashCode(user1) == comparer.GetHashCode(user2));
            Assert.IsTrue(comparer.GetHashCode(user1) != comparer.GetHashCode(user3));
            Assert.IsTrue(comparer.GetHashCode(user1) != comparer.GetHashCode(user4));
        }
    }
}
