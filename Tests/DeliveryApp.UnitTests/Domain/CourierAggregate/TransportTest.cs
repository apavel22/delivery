using System;

using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;

using FluentAssertions;

using Xunit;

namespace DeliveryApp.UnitTests.CourierAggregate;

public class TransportShould
{
    [Fact]
    public void ReturnCorrectIdAndName()
    {
        //Arrange

        //Act
        var pedestrianTransport = Transport.Pedestrian;
        var bicycleTransport = Transport.Bicycle;
        var scooterTransport = Transport.Scooter;
        var carTransport = Transport.Car;

        //Assert
        pedestrianTransport.Id.Should().Be(1);
        pedestrianTransport.Name.Should().Be("pedestrian");

        bicycleTransport.Id.Should().Be(2);
        bicycleTransport.Name.Should().Be("bicycle");

        scooterTransport.Id.Should().Be(3);
        scooterTransport.Name.Should().Be("scooter");

        carTransport.Id.Should().Be(4);
        carTransport.Name.Should().Be("car");

    }

    [Theory]
    [InlineData(1, "pedestrian")]
    [InlineData(2, "bicycle")]
    [InlineData(3, "scooter")]
    [InlineData(4, "car")]
    public void CanBeFoundById(int id, string name)
    {
        //Arrange

        //Act
        var transport = Transport.From(id).Value;

        //Assert
        transport.Id.Should().Be(id);
        transport.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(1, "pedestrian")]
    [InlineData(2, "bicycle")]
    [InlineData(3, "scooter")]
    [InlineData(4, "car")]
    public void CanBeFoundByName(int id, string name)
    {
        //Arrange

        //Act
        var transport = Transport.FromName(name).Value;

        //Assert
        transport.Id.Should().Be(id);
        transport.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(-7, "some")]
    public void CanNotBeFoundByWrongParams(int id, string name)
    {
        //Arrange

        //Act
        var transport = Transport.FromName(name);

        transport.IsSuccess.Should().BeFalse();
        transport.Error.Should().NotBeNull();

        //Act
        transport = Transport.From(id);

        transport.IsSuccess.Should().BeFalse();
        transport.Error.Should().NotBeNull();
    }


    [Fact]
    public void ReturnListOfTransports()
    {
        //Arrange

        //Act
        var allTransports = Transport.List();

        //Assert
        allTransports.Should().NotBeEmpty();
    }


    [Fact]
    public void CanCarry()
    {
        //Arrange
        var transport = Transport.Pedestrian;
        var weight = Weight.Create(1).Value;

        //Act
        var result = transport.CanCarry(weight);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();

        //Act
        Weight weightFrom = null;
        var resultForNull = transport.CanCarry(weightFrom);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();

    }

    [Fact]
    public void CanNotCarry()
    {
        //Arrange
        var transport = Transport.Pedestrian;
        var weight = Weight.Create(transport.Capacity.Value + 1).Value;

        //Act
        var result = transport.CanCarry(weight);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeFalse();
    }
}
