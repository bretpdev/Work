using Xunit;
using NTISFCFED;


namespace UnitTests
{
    public class TestUnits
    {
        
        [Theory]
        [InlineData(@"R")]
        [InlineData(@"I")]
        [InlineData(@"E")]
        [InlineData(@"F")]
        [InlineData(@"S")]
        [InlineData(@"A")]
        [InlineData(@"X")]
        public void TestParseArc(string value)
        {
            
            DownloadProcessor dp = new DownloadProcessor(1);
            
            string answer = dp.ParseArc(value);

            Assert.NotNull(answer);
            
        }
    }
}
