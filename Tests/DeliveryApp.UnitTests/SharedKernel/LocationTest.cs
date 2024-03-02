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
        var location = Location.Create(2,3);

        //Assert
        location.IsSuccess.Should().BeTrue();
        location.Value.X.Should().Be(2);
        location.Value.Y.Should().Be(3);
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
        var location = Location.Create(x,y);

        //Assert
        location.IsSuccess.Should().BeFalse();
        location.Error.Should().NotBeNull();
    }


    [Fact]
    public void BeEqualWhenAllPropertiesIsEqual()
    {
        //Arrange
        var first = Location.Create(2, 3).Value;
        var second = Location.Create(2, 3).Value;
        
        //Act
        var result = first == second;

        //Assert
        result.Should().BeTrue();
    }


    [Fact]
    public void BeNotEqualWhenAllPropertiesIsEqual()
    {
        //Arrange
        var first = Location.Create(2,3).Value;
        var second = Location.Create(3,2).Value;
        
        //Act
        var result = first == second;

        //Assert
        result.Should().BeFalse();
    }


    [Fact]
    public void CalcCorrectDistance()
    {
        //Arrange
        var first = Location.Create(1,1).Value;
        var second = Location.Create(5,5).Value;
        var resultDistance = 8;
        
        //Act
        var result = first.Distance(second).Value == resultDistance;
        result.Should().BeTrue();

        result = (first - second).Value == resultDistance;
        result.Should().BeTrue();

        result = first.Distance(second).Value == second.Distance(first).Value;
        result.Should().BeTrue();

        result = (first-second).Value == (second-first).Value;
        result.Should().BeTrue();
    }


    [Fact]
    public void BeZeroDisplacementWhenLocationsAreEqual()
    {
        //Arrange
        var first = Location.Create(3,4).Value;
        var second = Location.Create(3,4).Value;
 
        //Act
        var result = first.Distance(second).Value == 0;

        //Assert
        result.Should().BeTrue();
    }
}