using DeliveryApp.Core.Domain.SharedKernel;
using FluentAssertions;

using Xunit;

namespace DeliveryApp.UnitTests.SharedKernel;

public class LocationShould
{
    [Fact]
    public void BeCorrectWhenParamsIsCorrectOnCreated()
    {
        //Arrange

        //Act
        var location = Location.Create(2, 3);

        //Assert
        location.IsSuccess.Should().BeTrue();
        location.Value.X.Should().Be(2);
        location.Value.Y.Should().Be(3);

        //Act by location
        var location2 = Location.Create(location.Value);

        // Assert
        location2.IsSuccess.Should().BeTrue();
        location2.Value.X.Should().Be(location.Value.X);
        location2.Value.Y.Should().Be(location.Value.Y);

    }


    [Theory]
    [InlineData(0, 2)]
    [InlineData(-1, 2)]
    [InlineData(11, 2)]
    [InlineData(2, 0)]
    [InlineData(2, -1)]
    [InlineData(2, 11)]
    public void ReturnErrorWhenParamsIsInCorrectOnCreated(int x, int y)
    {
        //Arrange

        //Act
        var location = Location.Create(x, y);

        //Assert
        location.IsSuccess.Should().BeFalse();
        location.Error.Should().NotBeNull();

        //Act
        Location locationFrom = null;
        var location2 = Location.Create(locationFrom);

        // Assert
        location2.IsSuccess.Should().BeFalse();
        location2.Error.Should().NotBeNull();
    }


    [Fact]
    public void BeEqualWhenAllPropertiesAreEqual()
    {
        //Arrange
        var first = Location.Create(2, 3).Value;
        var second = Location.Create(2, 3).Value;

        //Act
        var result = first == second;

        //Assert
        result.Should().BeTrue();
    }


    [Theory]
    [InlineData(2, 3, 3, 2)]
    [InlineData(2, 3, 2, 2)]
    public void BeNotEqualWhenAllPropertiesAreNotEqual(int _first_x, int _first_y, int _second_x, int _second_y)
    {
        //Arrange
        var first = Location.Create(_first_x, _first_y).Value;
        var second = Location.Create(_second_x, _second_y).Value;

        //Act
        var result = first == second;

        //Assert
        result.Should().BeFalse();
    }


    [Theory]
    [InlineData(1, 1, 5, 5, 8)]
    [InlineData(2, 2, 2, 2, 0)]
    public void CalcCorrectDistance(int _first_x, int _first_y, int _second_x, int _second_y, int resultDistance)
    {
        //Arrange
        var first = Location.Create(_first_x, _first_y).Value;
        var second = Location.Create(_second_x, _second_y).Value;

        //Act
        var result = first.Distance(second).Value == resultDistance;
        result.Should().BeTrue();

        result = (first - second).Value == resultDistance;
        result.Should().BeTrue();

        result = first.Distance(second).Value == second.Distance(first).Value;
        result.Should().BeTrue();

        result = (first - second).Value == (second - first).Value;
        result.Should().BeTrue();
    }

}