using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using ACURINTR;

namespace ACURINTR.Tests
{
	[TestFixture]
	public class ExtensionMethodsTests
	{
		[Test]
		public void GetNumericGroups_ApartmentAddressFromSystem_ReturnsTwoGroups()
		{
			string apartmentAddress = "475 BRANT COURT NO 30         ______________________________";
			List<string> numericGroups = apartmentAddress.GetNumericGroups();
			Assert.AreEqual(2, numericGroups.Count);
			Assert.Contains("475", numericGroups);
			Assert.Contains("30", numericGroups);
		}

		[Test]
		public void GetNumericGroups_HouseAddressFromQueueTask_ReturnsOneGroup()
		{
			string houseAddress = "5261 CROCKETT DR";
			List<string> numericGroups = houseAddress.GetNumericGroups();
			Assert.AreEqual(1, numericGroups.Count);
			Assert.Contains("5261", numericGroups);
		}
	}//class
}//namespace
