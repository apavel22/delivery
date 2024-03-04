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
        IOrderRepository repository = new OrderRepository(_context);
        repository.Add(order);

        //Assert
        var dataFromDb = await repository.GetByIdAsync(order.Id);
        order.Should().BeEquivalentTo(dataFromDb);

    }

    [Fact]
    public async void CanUpdateOrder()
    {
        //Arrange
    	var id = Guid.NewGuid();
        var location = Location.Create(4,9);
        var weight = Weight.Create(1);

        var order = Order.Create(id, location.Value, weight.Value).Value;

        IOrderRepository repository = new OrderRepository(_context);
        order = repository.Add(order);

        var courier = DeliveryApp.Core.Domain.CourierAggregate.Courier.Create("Name", DeliveryApp.Core.Domain.CourierAggregate.Transport.Car).Value;
        CourierRepository repository2 = new CourierRepository(_context);
        courier = repository2.Add(courier);
        courier.StartWork().IsSuccess.Should().BeTrue();
        repository2.Update(courier);

        //Act
        order.Assigne(courier).IsSuccess.Should().BeTrue();
        // <hack> чтобы приаттачить к курьеру статус и транспорт, но без сохранения в БД
		repository2.Update_WothoutSaveToDb_Hack(courier);
        repository.Update(order);
       
        //Assert
        var dataFromDb = await repository.GetByIdAsync(order.Id);
        order.Should().BeEquivalentTo(dataFromDb);

    }


    [Fact]
    public async void CanGetAllUnassigned()
    {
        //Arrange
    	var id = Guid.NewGuid();
        var location = Location.Create(4,9);
        var weight = Weight.Create(1);

        var order = Order.Create(id, location.Value, weight.Value).Value;

        //Act
        IOrderRepository repository = new OrderRepository(_context);
        order = repository.Add(order);

        //Assert
        var allData = repository.GetAllUnassigned();
        allData.Count().Should().Be(1);


        //Arrange
        var courier = DeliveryApp.Core.Domain.CourierAggregate.Courier.Create("Name", DeliveryApp.Core.Domain.CourierAggregate.Transport.Car).Value;
        CourierRepository repository2 = new CourierRepository(_context);
        courier.StartWork().IsSuccess.Should().BeTrue();
        courier = repository2.Add(courier);

        //Act
        order.Assigne(courier).IsSuccess.Should().BeTrue();
        // <hack> чтобы приаттачить к курьеру статус и транспорт, но без сохранения в БД
		repository2.Update_WothoutSaveToDb_Hack(courier);

        repository.Update(order);

        //Assert
		allData = repository.GetAllUnassigned();
        allData.Count().Should().Be(0);

	}
}