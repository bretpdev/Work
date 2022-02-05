using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NSLDSCONSO.Tests
{
    public class NameParserTests
    {
        [Fact]
        public void CommaNameParsedCorrectly()
        {
            var result = new NameParser("DOE, JANE M");
            Assert.Equal("DOE", result.LastName);
            Assert.Equal("JANE", result.FirstName);
            Assert.Equal("M", result.MiddleInitial);
        }

        [Fact]
        public void BadCommaNameParsedCorrectly()
        {
            var result = new NameParser("DEVLIN, ,ARCIE L");
            Assert.Equal("DEVLIN", result.LastName);
            Assert.Equal("ARCIE", result.FirstName);
            Assert.Equal("L", result.MiddleInitial);
        }

        [Fact]
        public void SpacedNameParsedCorrectly()
        {
            var result = new NameParser("JANE M DOE");
            Assert.Equal("DOE", result.LastName);
            Assert.Equal("JANE", result.FirstName);
            Assert.Equal("M", result.MiddleInitial);
        }

        [Fact]
        public void NotNameParsedCorrectly()
        {
            var result = new NameParser("My Company Has A Long Name");
            Assert.Equal("My Company Has A Long Name", result.FirstName);
        }

        [Fact]
        public void NoMiddleInitialParsedCorrectly()
        {
            var result = new NameParser("DOE, JANE");
            Assert.Equal("DOE", result.LastName);
            Assert.Equal("JANE", result.FirstName);
            Assert.Null(result.MiddleInitial);

        }
    }
}
