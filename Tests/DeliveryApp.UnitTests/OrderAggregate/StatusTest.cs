using DeliveryApp.Core.Domain.OrderAggregate;
using FluentAssertions;

using Xunit;

namespace DeliveryApp.UnitTests.OrderAggregate;

public class StatusShould
{
    [Fact]
    public void ReturnCorrectIdAndName()
    {
        //Arrange
        
        //Act
        var createdStatus = Status.Created;
        var assignedStatus = Status.Assigned;
        var completedStatus = Status.Completed;
        

        //Assert
        createdStatus.Id.Should().Be(1);
        createdStatus.Name.Should().Be("created");
        
        assignedStatus.Id.Should().Be(2);
        assignedStatus.Name.Should().Be("assigned");

        completedStatus.Id.Should().Be(3);
        completedStatus.Name.Should().Be("completed");

    }
    
    [Theory]
    [InlineData(1,"created")]
    [InlineData(2,"assigned")]
    [InlineData(3,"completed")]
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
    [InlineData(1,"created")]
    [InlineData(2,"assigned")]
    [InlineData(3,"completed")]
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
