using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Q;

namespace Q.Tests
{
	[TestFixture]
	public class ExtensionMethodsTests
	{
		#region MaskSSNs
		[Test]
		public void MaskSSNs_Empty_DoNothing()
		{
			string masked = "".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("", masked);
		}

		[Test]
		public void MaskSSNs_Null_DoNothing()
		{
			string masked = null;
			masked = masked.MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual(null, masked);
		}

		[Test]
		public void MaskSSNs_EightDigits_DoNothing()
		{
			string masked = "12345678".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("12345678", masked);
		}

		#region Lone SSN
		//The next four tests check that the right number of characters get masked when there is no separator.
		[Test]
		public void MaskSSNs_LoneSsnNoSeparator_MaskZeroCharacters()
		{
			string maskedSsn = "123456789".MaskSSNs(0, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("123456789", maskedSsn);
		}

		[Test]
		public void MaskSSNs_LoneSsnNoSeparator_MaskOneCharacter()
		{
			string maskedSsn = "123456789".MaskSSNs(1, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("X23456789", maskedSsn);
		}

		[Test]
		public void MaskSSNs_LoneSsnNoSeparator_MaskEightCharacters()
		{
			string maskedSsn = "123456789".MaskSSNs(8, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXXXXX9", maskedSsn);
		}

		[Test]
		public void MaskSSNs_LoneSsnNoSeparator_MaskNineCharacters()
		{
			string maskedSsn = "123456789".MaskSSNs(9, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXXXXXX", maskedSsn);
		}

		//The next four tests check that the right number of characters get masked when there is a separator (dash is arbitrarily chosen).
		[Test]
		public void MaskSSNs_LoneSsnDashSeparator_MaskZeroCharacters()
		{
			string maskedSsn = "123-45-6789".MaskSSNs(0, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("123456789", maskedSsn);
		}

		[Test]
		public void MaskSSNs_LoneSsnDashSeparator_MaskOneCharacter()
		{
			string maskedSsn = "123-45-6789".MaskSSNs(1, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("X23456789", maskedSsn);
		}

		[Test]
		public void MaskSSNs_LoneSsnDashSeparator_MaskEightCharacters()
		{
			string maskedSsn = "123-45-6789".MaskSSNs(8, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXXXXX9", maskedSsn);
		}

		[Test]
		public void MaskSSNs_LoneSsnDashSeparator_MaskNineCharacters()
		{
			string maskedSsn = "123-45-6789".MaskSSNs(9, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXXXXXX", maskedSsn);
		}

		//The next two tests check that the remaining separators are detected.
		[Test]
		public void MaskSSNs_LoneSsnDotSeparator_MaskAnyCharacters()
		{
			string maskedSsn = "123.45.6789".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXX6789", maskedSsn);
		}

		[Test]
		public void MaskSSNs_LoneSsnSpaceSeparator_MaskAnyCharacters()
		{
			string maskedSsn = "123 45 6789".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXX6789", maskedSsn);
		}

		//The next eight tests check that the correct separator is used in the mask.
		[Test]
		public void MaskSSNs_LoneSsnNoSeparator_OutputNoSeparator()
		{
			string mask = "123456789".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXX6789", mask);
		}

		[Test]
		public void MaskSSNs_LoneSsnNoSeparator_OutputDashSeparator()
		{
			string mask = "123456789".MaskSSNs(5, 'X', ExtensionMethods.Separator.Dash);
			Assert.AreEqual("XXX-XX-6789", mask);
		}

		[Test]
		public void MaskSSNs_LoneSsnNoSeparator_OutputDotSeparator()
		{
			string mask = "123456789".MaskSSNs(5, 'X', ExtensionMethods.Separator.Dot);
			Assert.AreEqual("XXX.XX.6789", mask);
		}

		[Test]
		public void MaskSSNs_LoneSsnNoSeparator_OutputSpaceSeparator()
		{
			string mask = "123456789".MaskSSNs(5, 'X', ExtensionMethods.Separator.Space);
			Assert.AreEqual("XXX XX 6789", mask);
		}

		[Test]
		public void MaskSSNs_LoneSsnNoSeparator_OutputSeparatorSameAsOriginal()
		{
			string mask = "123456789".MaskSSNs(5, 'X', ExtensionMethods.Separator.SameAsOriginal);
			Assert.AreEqual("XXXXX6789", mask);
		}

		[Test]
		public void MaskSSNs_LoneSsnDashSeparator_OutputSeparatorSameAsOriginal()
		{
			string mask = "123-45-6789".MaskSSNs(5, 'X', ExtensionMethods.Separator.SameAsOriginal);
			Assert.AreEqual("XXX-XX-6789", mask);
		}

		[Test]
		public void MaskSSNs_LoneSsnDotSeparator_OutputSeparatorSameAsOriginal()
		{
			string mask = "123.45.6789".MaskSSNs(5, 'X', ExtensionMethods.Separator.SameAsOriginal);
			Assert.AreEqual("XXX.XX.6789", mask);
		}

		[Test]
		public void MaskSSNs_LoneSsnSpaceSeparator_OutputSeparatorSameAsOriginal()
		{
			string mask = "123 45 6789".MaskSSNs(5, 'X', ExtensionMethods.Separator.SameAsOriginal);
			Assert.AreEqual("XXX XX 6789", mask);
		}
		#endregion Lone SSN

		#region SSN at start
		//The next four tests check that the right number of characters get masked when there is no separator.
		[Test]
		public void MaskSSNs_SsnAtStartNoSeparator_MaskZeroCharacters()
		{
			string maskedSsn = "123456789abc".MaskSSNs(0, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("123456789abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtStartNoSeparator_MaskOneCharacter()
		{
			string maskedSsn = "123456789abc".MaskSSNs(1, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("X23456789abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtStartNoSeparator_MaskEightCharacters()
		{
			string maskedSsn = "123456789abc".MaskSSNs(8, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXXXXX9abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtStartNoSeparator_MaskNineCharacters()
		{
			string maskedSsn = "123456789abc".MaskSSNs(9, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXXXXXXabc", maskedSsn);
		}

		//The next four tests check that the right number of characters get masked when there is a separator (dash is arbitrarily chosen).
		[Test]
		public void MaskSSNs_SsnAtStartDashSeparator_MaskZeroCharacters()
		{
			string maskedSsn = "123-45-6789abc".MaskSSNs(0, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("123456789abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtStartDashSeparator_MaskOneCharacter()
		{
			string maskedSsn = "123-45-6789abc".MaskSSNs(1, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("X23456789abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtStartDashSeparator_MaskEightCharacters()
		{
			string maskedSsn = "123-45-6789abc".MaskSSNs(8, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXXXXX9abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtStartDashSeparator_MaskNineCharacters()
		{
			string maskedSsn = "123-45-6789abc".MaskSSNs(9, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXXXXXXabc", maskedSsn);
		}

		//The next two tests check that the remaining separators are detected.
		[Test]
		public void MaskSSNs_SsnAtStartDotSeparator_MaskAnyCharacters()
		{
			string maskedSsn = "123.45.6789abc".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXX6789abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtStartSpaceSeparator_MaskAnyCharacters()
		{
			string maskedSsn = "123 45 6789abc".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("XXXXX6789abc", maskedSsn);
		}
		#endregion SSN at start

		#region SSN at end
		//The next four tests check that the right number of characters get masked when there is no separator.
		[Test]
		public void MaskSSNs_SsnAtEndNoSeparator_MaskZeroCharacters()
		{
			string maskedSsn = "abc123456789".MaskSSNs(0, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abc123456789", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtEndNoSeparator_MaskOneCharacter()
		{
			string maskedSsn = "abc123456789".MaskSSNs(1, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcX23456789", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtEndNoSeparator_MaskEightCharacters()
		{
			string maskedSsn = "abc123456789".MaskSSNs(8, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXXXXX9", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtEndNoSeparator_MaskNineCharacters()
		{
			string maskedSsn = "abc123456789".MaskSSNs(9, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXXXXXX", maskedSsn);
		}

		//The next four tests check that the right number of characters get masked when there is a separator (dash is arbitrarily chosen).
		[Test]
		public void MaskSSNs_SsnAtEndDashSeparator_MaskZeroCharacters()
		{
			string maskedSsn = "abc123-45-6789".MaskSSNs(0, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abc123456789", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtEndDashSeparator_MaskOneCharacter()
		{
			string maskedSsn = "abc123-45-6789".MaskSSNs(1, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcX23456789", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtEndDashSeparator_MaskEightCharacters()
		{
			string maskedSsn = "abc123-45-6789".MaskSSNs(8, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXXXXX9", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtEndDashSeparator_MaskNineCharacters()
		{
			string maskedSsn = "abc123-45-6789".MaskSSNs(9, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXXXXXX", maskedSsn);
		}

		//The next two tests check that the remaining separators are detected.
		[Test]
		public void MaskSSNs_SsnAtEndDotSeparator_MaskAnyCharacters()
		{
			string maskedSsn = "abc123.45.6789".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXX6789", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnAtEndSpaceSeparator_MaskAnyCharacters()
		{
			string maskedSsn = "abc123 45 6789".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXX6789", maskedSsn);
		}
		#endregion SSN at end

		#region SSN in middle
		//The next four tests check that the right number of characters get masked when there is no separator.
		[Test]
		public void MaskSSNs_SsnInMiddleNoSeparator_MaskZeroCharacters()
		{
			string maskedSsn = "abc123456789abc".MaskSSNs(0, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abc123456789abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnInMiddleNoSeparator_MaskOneCharacter()
		{
			string maskedSsn = "abc123456789abc".MaskSSNs(1, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcX23456789abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnInMiddleNoSeparator_MaskEightCharacters()
		{
			string maskedSsn = "abc123456789abc".MaskSSNs(8, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXXXXX9abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnInMiddleNoSeparator_MaskNineCharacters()
		{
			string maskedSsn = "abc123456789abc".MaskSSNs(9, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXXXXXXabc", maskedSsn);
		}

		//The next four tests check that the right number of characters get masked when there is a separator (dash is arbitrarily chosen).
		[Test]
		public void MaskSSNs_SsnInMiddleDashSeparator_MaskZeroCharacters()
		{
			string maskedSsn = "abc123-45-6789abc".MaskSSNs(0, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abc123456789abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnInMiddleDashSeparator_MaskOneCharacter()
		{
			string maskedSsn = "abc123-45-6789abc".MaskSSNs(1, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcX23456789abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnInMiddleDashSeparator_MaskEightCharacters()
		{
			string maskedSsn = "abc123-45-6789abc".MaskSSNs(8, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXXXXX9abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnInMiddleDashSeparator_MaskNineCharacters()
		{
			string maskedSsn = "abc123-45-6789abc".MaskSSNs(9, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXXXXXXabc", maskedSsn);
		}

		//The next two tests check that the remaining separators are detected.
		[Test]
		public void MaskSSNs_SsnInMiddleDotSeparator_MaskAnyCharacters()
		{
			string maskedSsn = "abc123.45.6789abc".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXX6789abc", maskedSsn);
		}

		[Test]
		public void MaskSSNs_SsnInMiddleSpaceSeparator_MaskAnyCharacters()
		{
			string maskedSsn = "abc123 45 6789abc".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abcXXXXX6789abc", maskedSsn);
		}
		#endregion SSN in middle

		#region Two SSNs
		[Test]
		public void MaskSSNs_TwoSsnsNoPadding_MaskBoth()
		{
			string masked = "123-45-6789 123-45-6789".MaskSSNs(5, 'X', ExtensionMethods.Separator.Dash);
			Assert.AreEqual("XXX-XX-6789 XXX-XX-6789", masked);
		}

		[Test]
		public void MaskSSNs_TwoSsnsLeftPadded_MaskBoth()
		{
			string masked = "abc123-45-6789 123-45-6789".MaskSSNs(5, 'X', ExtensionMethods.Separator.Dash);
			Assert.AreEqual("abcXXX-XX-6789 XXX-XX-6789", masked);
		}

		[Test]
		public void MaskSSNs_TwoSsnsRightPadded_MaskBoth()
		{
			string masked = "123-45-6789 123-45-6789abc".MaskSSNs(5, 'X', ExtensionMethods.Separator.Dash);
			Assert.AreEqual("XXX-XX-6789 XXX-XX-6789abc", masked);
		}

		[Test]
		public void MaskSSNs_TwoSsnsBothPadded_MaskBoth()
		{
			string masked = "abc123-45-6789 123-45-6789abc".MaskSSNs(5, 'X', ExtensionMethods.Separator.Dash);
			Assert.AreEqual("abcXXX-XX-6789 XXX-XX-6789abc", masked);
		}
		#endregion

		#region Account number
		[Test]
		public void MaskSSNs_LoneAccountNumber_LeaveAsIs()
		{
			string maskedAccountNumber = "1234567890".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("1234567890", maskedAccountNumber);
		}

		[Test]
		public void MaskSSNs_AccountNumberAtStart_LeaveAsIs()
		{
			string maskedAccountNumber = "1234567890abc".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("1234567890abc", maskedAccountNumber);
		}

		[Test]
		public void MaskSSNs_AccountNumberAtEnd_LeaveAsIs()
		{
			string maskedAccountNumber = "abc1234567890".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abc1234567890", maskedAccountNumber);
		}

		[Test]
		public void MaskSSNs_AccountNumberInMiddle_LeaveAsIs()
		{
			string maskedAccountNumber = "abc1234567890abc".MaskSSNs(5, 'X', ExtensionMethods.Separator.None);
			Assert.AreEqual("abc1234567890abc", maskedAccountNumber);
		}
		#endregion Account number
		#endregion MaskSSNs
	}//class
}//namespace
