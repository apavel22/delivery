using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using System;

using FluentAssertions;

using Xunit;

namespace DeliveryApp.UnitTests.OrderAggregate;

public class OrderShould
{
    [Fact]
    public void BeCorrectWhenParamsIsCorrectOnCreated()
    {
        //Arrange

        //Act
    	var id = Guid.NewGuid();
        var location = Location.Create(4,9);
        var weight = Weight.Create(7);


        var order = Order.Create(id, location.Value, weight.Value);

        //Assert
        order.IsSuccess.Should().BeTrue();
        order.Value.CourierId.Should().Be(Guid.Empty);
        order.Value.Status.Should().Be(Status.Created);
        order.Value.Location.Should().Be(location.Value);
        order.Value.Weight.Should().Be(weight.Value);
    }

    [Fact]
    public void ReturnErrorWhenParamsIsInCorrectOnCreated()
    {
        //Arrange
        
        //Act
        var id = Guid.Empty;
        var location = Location.Create(4,9);
        var weight = Weight.Create(7);


        var order = Order.Create(id, location.Value, weight.Value);

        //Assert
        order.IsSuccess.Should().BeFalse();
        order.Error.Should().NotBeNull();
    }


    [Fact]
    public void CanAssignFreeCourier()
    {
        //Arrange

        //Act
    	var id = Guid.NewGuid();
        var location = Location.Create(4,9);
        var weight = Weight.Create(7);

        var order = Order.Create(id, location.Value, weight.Value).Value;

        var courier = DeliveryApp.Core.Domain.CourierAggregate.Courier.Create("Name", DeliveryApp.Core.Domain.CourierAggregate.Transport.Car).Value;
        courier.StartWork();

        var result = order.Assigne(courier);


        //Assert
        result.IsSuccess.Should().BeTrue();
        order.CourierId.Should().Be(courier.Id);
        order.Status.Should().Be(Status.Assigned);

    }

    [Fact]
    public void CantAssignBusyCourier()
    {
        //Arrange

        //Act
    	var id = Guid.NewGuid();
        var location = Location.Create(4,9);
        var weight = Weight.Create(7);

        var order = Order.Create(id, location.Value, weight.Value).Value;

        var courier = DeliveryApp.Core.Domain.CourierAggregate.Courier.Create("Name", DeliveryApp.Core.Domain.CourierAggregate.Transport.Car).Value;

        var result = order.Assigne(courier);


        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("order.cant.be.assigned.to.non.ready.courier");

        order.CourierId.Should().Be(Guid.Empty);
        order.Status.Should().Be(Status.Created);
    }


    [Fact]
    public void CantAssignCourierByCapacity()
    {
        //Arrange

        //Act
    	var id = Guid.NewGuid();
        var location = Location.Create(4,9);
        var weight = Weight.Create(7);

        var order = Order.Create(id, location.Value, weight.Value).Value;

        var courier = DeliveryApp.Core.Domain.CourierAggregate.Courier.Create("Name", DeliveryApp.Core.Domain.CourierAggregate.Transport.Pedestrian).Value;
        courier.StartWork();

        // couier.pedestrian.transport.capacity = 1
        // order.weight = 7

        var result = order.Assigne(courier);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("order.cant.be.assigned.to.courier.by.capacity");

    }


    [Fact]
    public void CantAssignForAlreadyAssigned()
    {
        //Arrange

        //Act
    	var id = Guid.NewGuid();
        var location = Location.Create(4,9);
        var weight = Weight.Create(7);
        var order = Order.Create(id, location.Value, weight.Value).Value;

        // first courier
        var courier = DeliveryApp.Core.Domain.CourierAggregate.Courier.Create("Name", DeliveryApp.Core.Domain.CourierAggregate.Transport.Car).Value;
        courier.StartWork();

        order.Assigne(courier);

        // second courier
        var courier2 = DeliveryApp.Core.Domain.CourierAggregate.Courier.Create("Other Name", DeliveryApp.Core.Domain.CourierAggregate.Transport.Car).Value;
        courier2.StartWork();

        var result = order.Assigne(courier2);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("order.has.already.assigned.to.courier");

        order.CourierId.Should().Be(courier.Id);
        order.Status.Should().Be(Status.Assigned);
    }

}
