using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;

using DeliveryApp.Core.Ports;

using DeliveryApp.Infrastructure;
using DeliveryApp.Infrastructure.Adapters.Postgres;

using Microsoft.EntityFrameworkCore;

using FluentAssertions;


using Xunit;

namespace DeliveryApp.IntegrationTests;

public class CourierRepositoryTestsShould: BaseRepositoryTestsShould
{
    [Fact]
    public async void CanInitContainers()
    {
    	var result = true;
        result.Should().BeTrue();
    }

    [Fact]
    public async void CanAddCourier()
    {
        //Arrange
        var transport = Transport.Pedestrian;
        var data1 = Courier.Create("Name 1", transport).Value;
        var data2 = Courier.Create("Name 2", transport).Value;

        //Act
        ICourierRepository repository = new CourierRepository(_context);
        repository.Add(data1);
        repository.Add(data2);

        //Assert
        var dataFromDb = await repository.GetByIdAsync(data1.Id);
        data1.Should().BeEquivalentTo(dataFromDb);

		dataFromDb = await repository.GetByIdAsync(data2.Id);
        data2.Should().BeEquivalentTo(dataFromDb);
    }

    [Fact]
    public async void CanUpdateCourier()
    {
    	// Arrange
        var transport = Transport.Pedestrian;
        var data = Courier.Create("Name 1", transport).Value;

        ICourierRepository repository = new CourierRepository(_context);
        data = repository.Add(data);

        //Act
        var to = Location.Create(2,2).Value;

        data.StartWork();
        data.Move(to);
        
        //Act
        repository.Update(data);
        
        //Assert
        var dataFromDb = await repository.GetByIdAsync(data.Id);
        data.Should().BeEquivalentTo(dataFromDb);
    }

    [Fact]
    public async void CanGetallReady()
    {
    	// Arrange
        var transport = Transport.Pedestrian;
        var data = Courier.Create("Name 1", transport).Value;

        //Act
        ICourierRepository repository = new CourierRepository(_context);
        data = repository.Add(data);

        //Assert
        var allData = repository.GetAllReady();
        allData.Count().Should().Be(0);

        //Act
        data.StartWork();
        repository.Update(data);

        //Assert
		allData = repository.GetAllReady();
        allData.Count().Should().Be(1);
	}
}
