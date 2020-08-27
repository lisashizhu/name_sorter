using System;
using NameSorter.Business;
using NameSorter.Business.Models;
using Xunit;

namespace NameSorter.Tests
{
    public class FromLastNameThenGivenNameSorterTestFixture
    {
        [Fact]
        public void EnsureNameCanBeSortedFromLastNameThenGivenName()
        {
            var sorter = new FromLastNameThenGivenNameSorter();
            FullName[] testData =
            {
                new FullName{LastName = "Parsons",GivenNames = new []{"Janet"}},
                new FullName{LastName = "Lewis",GivenNames = new []{"Vaughn"}},
                new FullName{LastName = "Archer",GivenNames = new []{"Adonis","Julius"}}
            };
            var sortedData = sorter.Sort(testData);
            Assert.Equal("Archer",sortedData[0].LastName);
            Assert.Equal("Lewis",sortedData[1].LastName);
            Assert.Equal("Parsons",sortedData[2].LastName);

        }
    }
}
