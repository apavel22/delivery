using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;

using DeliveryApp.Core.Ports;

using DeliveryApp.Infrastructure;
using DeliveryApp.Infrastructure.Adapters.Postgres;

using Microsoft.EntityFrameworkCore;

using FluentAssertions;


using Xunit;

namespace DeliveryApp.IntegrationTests;

public class CourierRepositoryTestsShould : BaseRepositoryTestsShould
{

    [Fact]
    public async void CanAddOneCourier()
    {
        //Arrange
        var transport = Transport.Pedestrian;
        var data = Courier.Create("Name 1", transport).Value;

        //Act
        ICourierRepository repository = new CourierRepository(_context);
        repository.Add(data);

        await repository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        var dataFromDb = await repository.GetByIdAsync(data.Id);
        data.Should().BeEquivalentTo(dataFromDb);
    }

    [Fact]
    public async void CanAddFewCouriersAtOnce()
    {
        //Arrange
        var transport = Transport.Pedestrian;
        var data1 = Courier.Create("Name 1", transport).Value;
        var data2 = Courier.Create("Name 2", transport).Value;

        //Act
        ICourierRepository repository = new CourierRepository(_context);
        repository.Add(data1);
        repository.Add(data2);

        await repository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        var dataFromDb1 = await repository.GetByIdAsync(data1.Id);
        data1.Should().BeEquivalentTo(dataFromDb1);

        var dataFromDb2 = await repository.GetByIdAsync(data2.Id);
        data2.Should().BeEquivalentTo(dataFromDb2);
    }


    [Fact]
    public async void CanAddOneChangedCourier()
    {
        //Arrange
        var transport = Transport.Pedestrian;
        var data1 = Courier.Create("Name 1", transport).Value;

        data1.StartWork();

        //Act
        ICourierRepository repository = new CourierRepository(_context);
        repository.Add(data1);

        await repository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        var dataFromDb = await repository.GetByIdAsync(data1.Id);
        data1.Should().BeEquivalentTo(dataFromDb);
    }


    [Fact]
    public async void CanUpdateCourier()
    {
        // Arrange
        var transport = Transport.Car;
        var data = Courier.Create("Name 1", transport).Value;

        ICourierRepository repository = new CourierRepository(_context);
        repository.Add(data);
        await repository.UnitOfWork.SaveEntitiesAsync();

        //Act
        var to = Location.Create(2, 2).Value;

        data.StartWork();
        data.Move(to);

        //Act
        repository.Update(data);
        await repository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        var dataFromDb = await repository.GetByIdAsync(data.Id);
        data.Should().BeEquivalentTo(dataFromDb);


        //Act
        to = Location.Create(3, 3).Value;
        data.Move(to);

        //Act
        repository.Update(data);
        await repository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        dataFromDb = await repository.GetByIdAsync(data.Id);
        data.Should().BeEquivalentTo(dataFromDb);

    }


    [Fact]
    public async void CanGetAllReady()
    {
        // Arrange
        var transport = Transport.Pedestrian;
        var data1 = Courier.Create("Name 1", transport).Value;
        var data2 = Courier.Create("Name 1", transport).Value;

        //Act
        ICourierRepository repository = new CourierRepository(_context);
        repository.Add(data1);
        repository.Add(data2);
        await repository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        var allData = repository.GetAllReady();
        allData.Count().Should().Be(0);

        //Act
        data1.StartWork();
        repository.Update(data1);
        await repository.UnitOfWork.SaveEntitiesAsync();

        //Assert
        allData = repository.GetAllReady();
        allData.Count().Should().Be(1);
        allData.First().Should().BeEquivalentTo(data1);
    }
}

