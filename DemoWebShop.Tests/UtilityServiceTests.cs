using FluentAssertions;
using OnlineStore.Services;

namespace DemoWebShop.Tests
{
    public class UtilityServiceTests
    {
        [Fact]
        public void UtilityService_ConcatenateString_ReturnString()
        {
            //Arrange
            var utilitySerivce = new UtilityService();
            List<string> strings = new List<string> { "Hello", "How", "Are", "You" };


            //Act

            var string1 = utilitySerivce.ConcatenateStrings(strings, false);
            var string2 = utilitySerivce.ConcatenateStrings(strings, true);

            //Assert

            string1.Should().Be("HelloHowAreYou");
            string2.Should().Be("Hello How Are You ");
        }

        [Theory]
        [InlineData(new[] { "Hello", "How", "Are", "You" }, false, "HelloHowAreYou")]
        [InlineData(new[] { "Hello", "How", "Are", "You" }, true, "Hello How Are You ")]
        [InlineData(new[] { "This", "is", "a", "test" }, false, "Thisisatest")]
        [InlineData(new[] { "This", "is", "a", "test" }, true, "This is a test ")]
        [InlineData(new[] { "SingleWord" }, false, "SingleWord")]
        [InlineData(new[] { "SingleWord" }, true, "SingleWord ")]
        public void UtilityService_ConcatenateStrings_ReturnsCorrectString(string[] strings, bool includeSpaces, string expected)
        {
            // Arrange
            var utilityService = new UtilityService();
            var stringList = new List<string>(strings);

            // Act
            var result = utilityService.ConcatenateStrings(stringList, includeSpaces);

            // Assert
            result.Should().Be(expected);
        }
    }
}