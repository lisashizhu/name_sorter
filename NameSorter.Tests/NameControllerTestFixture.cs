using System.Runtime.Serialization;
using Moq;
using NameSorter.Business.Interface;
using NameSorter.Business.Models;
using NameSorter.Common;
using NameSorter.Common.Exceptions;
using NameSorter.WebApi.Controllers;
using NameSorter.WebApi.Models;
using Xunit;

namespace NameSorter.Tests
{
    public class NameControllerTestFixture
    {
        private readonly Mock<ISorterFactory> _mockSortFactory =
            new Mock<ISorterFactory>();

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EnsureThrowExceptionWhenInputIsNullOrEmpty(string input)
        {
            var nameController =
                new NameController(_mockSortFactory.Object);
            var res = Assert.Throws<BusinessException>(() => nameController.SortName(new SortNameRequest
            {
                NameContent = input,
                SortType = SortType.LastNameThenGivenName
            }));

            Assert.Equal("No name to be sorted.", res.Message);
        }

        [Fact]
        public void EnsureThrowExceptionWhenNoSorterCreatedFromFactory()
        {
            var nameController =
                new NameController(_mockSortFactory.Object);
            _mockSortFactory.Setup(obj =>
                obj.Get(It.IsAny<SortType>())).
                Returns((ISort) null);

            var res = Assert.Throws<BusinessException>(() => nameController.SortName(new SortNameRequest
            {
                NameContent = "Janet Parsons"
            }));
            Assert.Equal("No name sorter found that can handle requested sort type.", res.Message);
        }

        [Fact]
        public void EnsureInputCanBeSortedCorrectly()
        {
            var nameController =
                new NameController(_mockSortFactory.Object);

            Mock<ISort> mockSort =  new Mock<ISort>();

            var testData = new []
            {
                new FullName {LastName = "Lewis", GivenNames = new[] {"Vaughn"}},
                new FullName {LastName = "Parsons", GivenNames = new[] {"Janet"}}
            };

            mockSort.Setup(obj => obj.Sort(It.IsAny<FullName[]>())).Returns(testData);

            _mockSortFactory.Setup(obj =>
                    obj.Get(It.IsAny<SortType>())).
                Returns(mockSort.Object);

            var res = nameController.SortName(new SortNameRequest{NameContent = "Janet Parsons"});
            Assert.NotNull(res);
            Assert.NotNull(res.Value);
            Assert.Equal("Vaughn Lewis",res.Value.SortedNames[0]);
            Assert.Equal("Janet Parsons",res.Value.SortedNames[1]);
        }
    }
}
