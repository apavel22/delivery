using System;
using System.Threading;
using System.Threading.Tasks;

using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Ports;

using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeliveryApp.UnitTests.Application;

public class CreateCourierCommandShould
{
    private readonly ICourierRepository _courierRepositoryMock;

    public CreateCourierCommandShould()
    {
        _courierRepositoryMock = Substitute.For<ICourierRepository>();
    }

    [Fact]
    public async void CantFindCourier()
    {
    }

    [Fact]
    public async void CantStartWork()
    {
    }


    [Fact]
    public async void CanStartWork()
    {
/*
        var CourierId = Guid.NewGuid();



        var location_x = 4;
        var location_y = 9;
        var weight = 7;

        var command = new Core.Application.UseCases.Commands.CreateCourier.Command(CourierId, location_x, location_y, weight);
        var handler = new Core.Application.UseCases.Commands.CreateCourier.Handler(_CourierRepositoryMock);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().BeTrue();
        _CourierRepositoryMock.Received(1);
*/
    }
}