using System;
using System.Threading;
using System.Threading.Tasks;

using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Ports;

using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeliveryApp.UnitTests.Application;

public class CreateOrderCommandShould
{
    private readonly IOrderRepository _orderRepositoryMock;

    public CreateOrderCommandShould()
    {
        _orderRepositoryMock = Substitute.For<IOrderRepository>();
    }


    [Fact]
    public async void CanCreateOrder()
    {
        var orderId = Guid.NewGuid();

	   	// setup mock:
		_orderRepositoryMock.UnitOfWork.SaveEntitiesAsync().Returns(Task.FromResult(true));
        _orderRepositoryMock.GetByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult(EmptyOrder()));


        var location_x = 4;
        var location_y = 9;
        var weight = 7;

        var command = new Core.Application.UseCases.Commands.CreateOrder.Command(orderId, location_x, location_y, weight);
        var handler = new Core.Application.UseCases.Commands.CreateOrder.Handler(_orderRepositoryMock);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().BeTrue();
        _orderRepositoryMock.Received(1);
    }

    [Theory]
    [InlineData(true, 1, 1, 1)]
    [InlineData(false, 0, 0, 0)]
    [InlineData(false, 1, 0, 0)]
    [InlineData(false, 0, 1, 0)]
    [InlineData(false, 1, 1, 0)]
    public async void ReturnFalseWhenParamsIsWrong(bool idIsNull, int x, int y, int weight)
    {
    	// setup mock:
		_orderRepositoryMock.UnitOfWork.SaveEntitiesAsync().Returns(Task.FromResult(true));

        var orderId = idIsNull ? Guid.Empty : Guid.NewGuid();

        var command = new Core.Application.UseCases.Commands.CreateOrder.Command(orderId, x, y, weight);
        var handler = new Core.Application.UseCases.Commands.CreateOrder.Handler(_orderRepositoryMock);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async void ReturnFalseWhenOrderAlreadyExists()
    {
        var orderId = Guid.NewGuid();

    	// setup mock:
		_orderRepositoryMock.UnitOfWork.SaveEntitiesAsync().Returns(Task.FromResult(true));
        _orderRepositoryMock.GetByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult(ExistingOrder(orderId)));

        var location_x = 4;
        var location_y = 9;
        var weight = 7;

        var command = new Core.Application.UseCases.Commands.CreateOrder.Command(orderId, location_x, location_y, weight);
        var handler = new Core.Application.UseCases.Commands.CreateOrder.Handler(_orderRepositoryMock);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().BeFalse();
    }

	private Order ExistingOrder(Guid orderId)
	{
		return Order.Create(orderId, Location.MinLocation, Weight.Create(1).Value).Value;
	}
 
	private Order EmptyOrder()
	{
		return null;
	}
}