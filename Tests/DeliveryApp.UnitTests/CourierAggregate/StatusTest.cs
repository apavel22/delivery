using DeliveryApp.Core.Domain.CourierAggregate;
using FluentAssertions;

using Xunit;

namespace DeliveryApp.UnitTests.CourierAggregate;

public class StatusShould
{
    [Fact]
    public void ReturnCorrectIdAndName()
    {
        //Arrange
        
        //Act
        var notAvailableStatus = Status.NotAvailable;
        var readyStatus = Status.Ready;
        var busyStatus = Status.Busy;
        

        //Assert
        notAvailableStatus.Id.Should().Be(1);
        notAvailableStatus.Name.Should().Be("notavailable");
        
        readyStatus.Id.Should().Be(2);
        readyStatus.Name.Should().Be("ready");

        busyStatus.Id.Should().Be(3);
        busyStatus.Name.Should().Be("busy");

    }
    
    [Theory]
    [InlineData(1,"notavailable")]
    [InlineData(2,"ready")]
    [InlineData(3,"busy")]
    public void CanBeFoundById(int id, string name)
    {
        //Arrange
        
        //Act
        var status = Status.From(id).Value;
        
        //Assert
        status.Id.Should().Be(id);
        status.Name.Should().Be(name);
    }
    
    [Theory]
    [InlineData(1,"notavailable")]
    [InlineData(2,"ready")]
    [InlineData(3,"busy")]
    public void CanBeFoundByName(int id, string name)
    {
        //Arrange
        
        //Act
        var status = Status.FromName(name).Value;
        
        //Assert
        status.Id.Should().Be(id);
        status.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(7,"some")]
    public void CanNotBeFoundByWrongName(int id, string name)
    {
        //Arrange
        
        //Act
        var status = Status.FromName(name);

        status.IsSuccess.Should().BeFalse();
        status.Error.Should().NotBeNull();
        
    }

    [Theory]
    [InlineData(-7,"some")]
    public void CanNotBeFoundByWrongId(int id, string name)
    {
        //Arrange
        
        //Act
        var status = Status.From(id);

        status.IsSuccess.Should().BeFalse();
        status.Error.Should().NotBeNull();
        
    }
    

    [Fact]
    public void ReturnListOfStatuses()
    {
        //Arrange
        
        //Act
        var allStatuses = Status.List();
        
        //Assert
        allStatuses.Should().NotBeEmpty();
    }

}
