using DeliveryApp.Core.Domain.SharedKernel;
using FluentAssertions;

using Xunit;

namespace DeliveryApp.UnitTests.SharedKernel;

public class WeightShould
{
    [Fact]
    public void BeCorrectWhenParamsIsCorrectOnCreated()
    {
        //Arrange
        
        //Act
        var weight = Weight.Create(100);

        //Assert
        weight.IsSuccess.Should().BeTrue();
        weight.Value.Value.Should().Be(100);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReturnErrorWhenParamsIsInCorrectOnCreated(int value)
    {
        //Arrange
        
        //Act
        var weight = Weight.Create(value);

        //Assert
        weight.IsSuccess.Should().BeFalse();
        weight.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void BeEqualWhenAllPropertiesIsEqual()
    {
        //Arrange
        var first = Weight.Create(10).Value;
        var second = Weight.Create(10).Value;
        
        //Act
        var result = first == second;

        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void BeNotEqualWhenAllPropertiesIsEqual()
    {
        //Arrange
        var first = Weight.Create(10).Value;
        var second = Weight.Create(5).Value;
        
        //Act
        var result = first == second;

        //Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void CanCompareMoreThen()
    {
        //Arrange
        var first = Weight.Create(10).Value;
        var second = Weight.Create(5).Value;
        
        //Act
        var result = first > second;

        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void CanCompareLessThen()
    {
        //Arrange
        var first = Weight.Create(5).Value;
        var second = Weight.Create(10).Value;
        
        //Act
        var result = first < second;

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanCompareMoreThenOrEqualMore()
    {
        //Arrange
        var first = Weight.Create(10).Value;
        var second = Weight.Create(5).Value;
        
        //Act
        var result = first >= second;

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanCompareMoreThenOrEqualEqual()
    {
        //Arrange
        var first = Weight.Create(10).Value;
        var second = Weight.Create(10).Value;
        
        //Act
        var result = first >= second;

        //Assert
        result.Should().BeTrue();
    }



    
    [Fact]
    public void CanCompareLessThenOrEqualLess()
    {
        //Arrange
        var first = Weight.Create(5).Value;
        var second = Weight.Create(10).Value;
        
        //Act
        var result = first <= second;

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanCompareLessThenOrEqualEqual()
    {
        //Arrange
        var first = Weight.Create(5).Value;
        var second = Weight.Create(5).Value;
        
        //Act
        var result = first <= second;

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ToIntConversion()
    {
        //Arrange
        
        //Act
        var weight = Weight.Create(10).Value;
        int value = weight;

        //Assert
        value.Should().Be(10);
    }

}
