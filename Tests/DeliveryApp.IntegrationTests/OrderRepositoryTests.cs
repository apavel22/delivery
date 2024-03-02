using DeliveryApp.Core.Domain.CourierAggregate;

using DeliveryApp.Infrastructure;

using Microsoft.EntityFrameworkCore;

using FluentAssertions;


using Xunit;

namespace DeliveryApp.IntegrationTests;

public class OrderRepositoryTestsShould: BaseRepositoryTestsShould
{
    [Fact]
    public async void CanInitContainers()
    {
    	var result = true;
        result.Should().BeTrue();
    }
}