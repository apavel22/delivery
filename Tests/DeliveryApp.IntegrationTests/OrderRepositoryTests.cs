using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;

using DeliveryApp.Core.Domain.SharedKernel;

using DeliveryApp.Core.Ports;

using DeliveryApp.Infrastructure;
using DeliveryApp.Infrastructure.Adapters.Postgres;

using Microsoft.EntityFrameworkCore;

using FluentAssertions;
using Xunit;

namespace DeliveryApp.IntegrationTests;

public class OrderRepositoryTestsShould: BaseRepositoryTestsShould
{

    [Fact]
    public async void CanAddOrder()
    {
        //Arrange
    	var id = Guid.NewGuid();
        var location = Location.Create(4,9);
        var weight = Weight.Create(7);

        var order = Order.Create(id, location.Value, weight.Value).Value;

        //Act
        OrderRepository repository = new OrderRepository(_context);
        repository.Add(order);

        await repository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        var dataFromDb = await repository.GetByIdAsync(order.Id);
        order.Should().BeEquivalentTo(dataFromDb);
    }


    [Fact]
    public async void CanAddFewOrdersAtOnceWithSameWeightAndLocation()
    {
        //Arrange
    	var id1 = Guid.NewGuid();
    	var id2 = Guid.NewGuid();
        var location = Location.Create(4,9);
        var weight = Weight.Create(7);

        var order1 = Order.Create(id1, location.Value, weight.Value).Value;
        var order2 = Order.Create(id2, location.Value, weight.Value).Value;


        //Act
        OrderRepository repository = new OrderRepository(_context);
        repository.Add(order1);
        repository.Add(order2);

        await repository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        var dataFromDb1 = await repository.GetByIdAsync(order1.Id);
        order1.Should().BeEquivalentTo(dataFromDb1);

        var dataFromDb2 = await repository.GetByIdAsync(order2.Id);
        order2.Should().BeEquivalentTo(dataFromDb2);

        //Act

    }

    [Fact]
    public async void CanUpdateOrder()
    {
        //Arrange
    	var id = Guid.NewGuid();
        var location = Location.Create(4,9);
        var weight = Weight.Create(1);

        var order = Order.Create(id, location.Value, weight.Value).Value;

        OrderRepository orderRepository = new OrderRepository(_context);
        orderRepository.Add(order);
        await orderRepository.UnitOfWork.SaveEntitiesAsync();


        // create courier in database
        var courier = DeliveryApp.Core.Domain.CourierAggregate.Courier.Create("Name", DeliveryApp.Core.Domain.CourierAggregate.Transport.Car).Value;
        CourierRepository courierRepository = new CourierRepository(_context);
        courierRepository.Add(courier);
        await courierRepository.UnitOfWork.SaveEntitiesAsync();

        //Act
        order.AssignToCourier(courier).IsSuccess.Should().BeTrue();
        orderRepository.Update(order);
        await orderRepository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        var dataFromDb = await orderRepository.GetByIdAsync(order.Id);
        order.Should().BeEquivalentTo(dataFromDb);
    }

    [Fact]
    public async void CanGetAllNew()
    {
        //Arrange
        var order1 = Order.Create(Guid.NewGuid(), Location.Create(1,7).Value, Weight.Create(3).Value).Value;
        var order2 = Order.Create(Guid.NewGuid(), Location.Create(1,7).Value, Weight.Create(3).Value).Value;
        var order3 = Order.Create(Guid.NewGuid(), Location.Create(1,7).Value, Weight.Create(3).Value).Value;

        //Act
        OrderRepository orderRepository = new OrderRepository(_context);
        orderRepository.Add(order1);
        orderRepository.Add(order2);
        orderRepository.Add(order3);
        await orderRepository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        var allData = orderRepository.GetAllNew();
        allData.Count().Should().Be(3);
        allData.First().Should().BeEquivalentTo(order1);
        allData.Last().Should().BeEquivalentTo(order3);

        //Arrange
        var courier = DeliveryApp.Core.Domain.CourierAggregate.Courier.Create("Name", DeliveryApp.Core.Domain.CourierAggregate.Transport.Car).Value;
        CourierRepository courierRepository = new CourierRepository(_context);
        courierRepository.Add(courier);
        await courierRepository.UnitOfWork.SaveEntitiesAsync();

        //Act
        order1.AssignToCourier(courier).IsSuccess.Should().BeTrue();
        orderRepository.Update(order1);
        await orderRepository.UnitOfWork.SaveEntitiesAsync();

        //Assert
		allData = orderRepository.GetAllNew();
        allData.Count().Should().Be(2);
        allData.First().Should().BeEquivalentTo(order2);
        allData.Last().Should().BeEquivalentTo(order3);
	}
}