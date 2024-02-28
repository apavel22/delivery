using DeliveryApp.Core.Domain.SharedKernel;
using FluentAssertions;

using Xunit;

namespace DeliveryApp.UnitTests.SharedKernel;

public class SpeedShould
{
    [Fact]
    public void BeCorrectWhenParamsIsCorrectOnCreated()
    {
        //Arrange
        
        //Act
        var speed = Speed.Create(10);

        //Assert
        speed.IsSuccess.Should().BeTrue();
        speed.Value.Value.Should().Be(10);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReturnErrorWhenParamsIsInCorrectOnCreated(int value)
    {
        //Arrange
        
        //Act
        var speed = Speed.Create(value);

        //Assert
        speed.IsSuccess.Should().BeFalse();
        speed.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void BeEqualWhenAllPropertiesIsEqual()
    {
        //Arrange
        var first = Speed.Create(10).Value;
        var second = Speed.Create(10).Value;
        
        //Act
        var result = first == second;

        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void BeNotEqualWhenAllPropertiesIsEqual()
    {
        //Arrange
        var first = Speed.Create(10).Value;
        var second = Speed.Create(5).Value;
        
        //Act
        var result = first == second;

        //Assert
        result.Should().BeFalse();
    }
}
