using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.WebApi;
using Xunit;
namespace SCHRPT.Tests
{

    public class BreadcrumbTests
    {
        const string Title = "Test";
        const string Url = "/Test";
        [Fact]
        public void ReturnsItself()
        {
            Breadcrumbs b = new Breadcrumbs();
            Breadcrumbs result = b.Add(Title, Url);
            Assert.Equal(b, result);
        }

        [Fact]
        public void ItemIsAdded()
        {
            Breadcrumbs b = new Breadcrumbs();
            b.Add(Title, Url);
            var item = b.Single();
            Assert.Equal(item.Text, Title);
            Assert.Equal(item.Url, Url);
        }

        [Fact]
        public void AddMethodsIdentical()
        {
            Breadcrumbs b = new Breadcrumbs();
            b.Add(Title, Url).Add(new Breadcrumbs.Breadcrumb() { Text = Title, Url = Url });
            var one = b.First();
            var two = b.Skip(1).Single();
            Assert.Equal(one.Text, two.Text);
            Assert.Equal(one.Url, two.Url);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void SupportsNBreadcrumbs(int n)
        {
            Breadcrumbs b = new Breadcrumbs();
            for (int i = 0; i < n; i++)
            {
                b.Add(Title, Url);
            }
            Assert.Equal(b.Count(), n);
        }

        [Fact]
        public void EnumeratorsIdentical()
        {
            Breadcrumbs b = new Breadcrumbs();
            Assert.Equal(b.GetEnumerator(), (b as IEnumerable).GetEnumerator());
        }
    }
}
