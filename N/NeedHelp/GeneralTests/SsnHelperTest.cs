using Xunit;
using NHGeneral;

namespace GeneralTests
{
    public class SsnHelperTest
    {
        //[Theory]
        //[InlineData("Ssn 123-45-6789 123456789 123456789", "Ssn XXX-XX-6789 XXX-XX-6789 XXX-XX-6789")]
        //[InlineData("Ssn 123-45-6789 ", "Ssn XXX-XX-6789 ")]
        //[InlineData("Ssn 123 45 6789", "Ssn XXX-XX-6789")]
        //[InlineData("Ssn 123/45/6789", "Ssn 123/45/6789")]
        //[InlineData("Ssn [123-45-6789", "Ssn [XXX-XX-6789")]
        //[InlineData("Ssn [123456789ATF", "Ssn [123456789ATF")]
        //[InlineData("Ssn ATF123456789", "Ssn ATF123456789")]
        //[InlineData("Ssn 123-45-6789]", "Ssn XXX-XX-6789]")]
        //[InlineData("Ssn 123456789]", "Ssn XXX-XX-6789]")]
        //[InlineData("Ssn (123-45-6789)", "Ssn (XXX-XX-6789)")]
        //[InlineData("Ssn 123-45-6789 123-45-6789 123-45-6789 123-45-6789 123-45-6789 123 45 6789", "Ssn XXX-XX-6789 XXX-XX-6789 XXX-XX-6789 XXX-XX-6789 XXX-XX-6789 XXX-XX-6789")]
        //[InlineData("Ssn 123-45-6789 1234", "Ssn XXX-XX-6789 1234")]
        //[InlineData("Ssn 123-45-6789 done", "Ssn XXX-XX-6789 done")]
        //[InlineData("Ssn 123-45-6789 123-45-6789", "Ssn XXX-XX-6789 XXX-XX-6789")]
        //[InlineData("Ssn 123456789", "Ssn XXX-XX-6789")]
        //[InlineData("Ssn 123456789 done", "Ssn XXX-XX-6789 done")]
        //[InlineData("Ssn 123-45-6789 123456789 123456789 123 45 6789 548 54 8457", "Ssn XXX-XX-6789 XXX-XX-6789 XXX-XX-6789 XXX-XX-6789 XXX-XX-8457")]
        //[InlineData("Ssn 123-45-6789 123456789 1234567890 123 45 6789 548 54 8457", "Ssn XXX-XX-6789 XXX-XX-6789 1234567890 XXX-XX-6789 XXX-XX-8457")]
        //public void MaskSsnIfExists_WhenTextHasSsn_ShouldMask(string textToCheck, string maskedText)
        //{
        //    SsnHelper helper = new SsnHelper(null);

        //    Assert.Equal(maskedText, helper.MaskSsnIfExists(textToCheck));
        //}

        [Theory]
        [InlineData("Not Ssn 012 34 56789")]
        [InlineData("Not Ssn 012-34-56789")]
        [InlineData("Not Ssn 0123456789")]
        [InlineData("Not Ssn 1234567890")]
        [InlineData("Not Ssn 00123456789")]
        [InlineData("Not Ssn 01-234-56789")]
        [InlineData("Not Ssn 01-234-5678")]
        [InlineData("Not Ssn 01 234 5678")]
        [InlineData("Not Ssn 01 234 5678 123-456-789")]
        [InlineData("Not Ssn (01 234 5678)")]
        [InlineData("Not Ssn [01 234 5678")]
        [InlineData("Not Ssn 01 234 5678]")]
        [InlineData("Not Ssn 01234567")]
        [InlineData("Not Ssn 12345678")]
        [InlineData("Not Ssn 123-45-678]")]
        [InlineData("Not Ssn 00600-00799")]
        [InlineData("Not Ssn 00900-00999")]
        public void MaskSsnIfExists_WhenTextIsNotSsn_ShouldNotMask(string textToCheck)
        {
            SsnHelper helper = new SsnHelper(null);

            Assert.True(textToCheck == helper.MaskSsnIfExists(textToCheck));
        }
    }
}