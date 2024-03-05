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
        var id = Guid.NewGuid();
        var location = Location.Create(4, 9);
        var weight = Weight.Create(7);

        var command = new Core.Application.UseCases.Commands.CreateOrder.Command(id, location.Value, weight.Value);
        var handler = new Core.Application.UseCases.Commands.CreateOrder.Handler(_orderRepositoryMock);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().BeTrue();
        _orderRepositoryMock.Received(1);
    }


    /*
        [Fact]
        public async void ReturnFalseWhenGoodNotExists()
        {
            //Arrange
            var buyerId = Guid.NewGuid();
            _goodRepositoryMock.GetAsync(Arg.Any<Guid>())
                .Returns(Task.FromResult(EmptyGood()));

            var command = new Core.Application.UseCases.Commands.ChangeItems.Command(buyerId,Good.Bread.Id,1);
            var handler = new Core.Application.UseCases.Commands.ChangeItems.Handler(_basketRepositoryMock,_goodRepositoryMock);

            //Act
            var result = await handler.Handle(command, new CancellationToken());

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void CanAddNewBasket()
        {
            //Arrange
            var buyerId = Guid.NewGuid();

            _goodRepositoryMock.GetAsync(Arg.Any<Guid>())
                .Returns(Task.FromResult(CorrectGood()));
            _basketRepositoryMock.GetByBuyerIdAsync(Arg.Any<Guid>())
                .Returns(Task.FromResult(EmptyBasket()));
            _basketRepositoryMock.Add(Arg.Any<Basket>())
                .Returns(CorrectBasket(buyerId));
            _basketRepositoryMock.UnitOfWork.SaveEntitiesAsync()
                .Returns(Task.FromResult(true));

            var command = new Core.Application.UseCases.Commands.ChangeItems.Command(buyerId,Good.Bread.Id,1);
            var handler = new Core.Application.UseCases.Commands.ChangeItems.Handler(_basketRepositoryMock,_goodRepositoryMock);

            //Act
            var result = await handler.Handle(command, new CancellationToken());

            //Assert
            result.Should().BeTrue();
            _goodRepositoryMock.Received(1);
            _basketRepositoryMock.Received(1);
        }

        private Good EmptyGood()
        {
            return null;
        }

        private Good CorrectGood()
        {
            return Good.Bread;
        }

        private Basket EmptyBasket()
        {
            return null;
        }

        private Basket CorrectBasket(Guid buyerId)
        {
            return Basket.Create(buyerId).Value;
        }
    */

}