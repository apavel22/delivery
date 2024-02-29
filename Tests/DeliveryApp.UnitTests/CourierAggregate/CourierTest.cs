using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;

using FluentAssertions;

using Xunit;

namespace DeliveryApp.UnitTests.CourierAggregate;

public class CourierShould
{
    [Fact]
    public void BeCorrectWhenParamsIsCorrectOnCreated()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian);

        //Assert
        courier.IsSuccess.Should().BeTrue();
        courier.Value.Name.Should().Be("Name");
        courier.Value.Status.Should().Be(Status.NotAvailable);
        courier.Value.Transport.Should().Be(Transport.Pedestrian);
        courier.Value.Location.Should().Be(Location.Create(1,1).Value);
    }

    [Fact]
    public void ReturnErrorWhenParamsIsInCorrectOnCreated()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("", Transport.Pedestrian);

        //Assert
        courier.IsSuccess.Should().BeFalse();
        courier.Error.Should().NotBeNull();
    }

    [Fact]
    public void BeCanStart()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;

        var result = courier.StartWork();

        //Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void BeAlreadyStarted()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;

        courier.StartWork();
        var result = courier.StartWork();

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("courier.has.already.started");

//        var error = Courier.Errors.CourierHasAlreadyStarted();
//        result.Error.Should().BeSameAs(error);
    }


    [Fact]
    public void BeCantStart()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;

        courier.StartWork();
        courier.InWork();

        var result = courier.StartWork();

        //Assert
        result.IsSuccess.Should().BeFalse();
    }




    [Fact]
    public void BeCanStop()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;

        courier.StartWork();

        var result = courier.StopWork();

        //Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void BeCantStop()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;

        courier.StartWork();
        courier.InWork();

        var result = courier.StopWork();

        //Assert
        result.IsSuccess.Should().BeFalse();
    }


    [Fact]
    public void BeAlreadyStopped()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;

        courier.StartWork();
        courier.StopWork();

        var result = courier.StopWork();

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("courier.has.already.stopped");
    }



    [Fact]
    public void BeCanBusy()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;

        courier.StartWork();

        var result = courier.InWork();

        //Assert
        result.IsSuccess.Should().BeTrue();
    }


    [Fact]
    public void BeCantBusy()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;

        courier.StartWork();
        courier.StopWork();

        var result = courier.InWork();

        //Assert
        result.IsSuccess.Should().BeFalse();
    }


    [Fact]
    public void BeAlreadyBusy()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;

        courier.StartWork();
        courier.InWork();

        var result = courier.InWork();

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("courier.has.already.busy");
    }



    [Fact]
    public void CalcTimeToPointPedestrian()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;
        var location = Location.Create(5,5).Value;

        var speed = courier.CalculateTimeToPoint(location);

        speed.IsSuccess.Should().BeTrue();
        speed.Value.Should().Be(8);
    }

    [Fact]
    public void CalcTimeToPointCarInWhole()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Car).Value;
        var location = Location.Create(4,5).Value;

        var speed = courier.CalculateTimeToPoint(location);

        speed.IsSuccess.Should().BeTrue();
        speed.Value.Should().Be(2);

        // distance == 4+4 = 8, speed = 4 8/4 = 2 
    }


    [Fact]
    public void CalcTimeToPointCarInFractional()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Car).Value;
        var location = Location.Create(6,5).Value;

        var speed = courier.CalculateTimeToPoint(location);

        speed.IsSuccess.Should().BeTrue();
        speed.Value.Should().Be(3);

        // distance == 4+5 = 9, speed = 4 9/4 = 2 + 1 = 3
    }


    [Fact]
    public void MoveToInSingleStep()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Car).Value;
        var location = Location.Create(1, 3).Value;

        var result = courier.Move(location);

        result.IsSuccess.Should().BeTrue();
        courier.Location.Should().Be(location);

    }

    [Fact]
    public void MoveToCorrectBySteps()
    {
        //Arrange
        
        //Act

        var courier = Courier.Create("Name", Transport.Car).Value;

        var location = Location.Create(3,9).Value;

        // speed = 4
        // first step
        // (1,1) -> (3,3)
        var result = courier.Move(location);

        result.IsSuccess.Should().BeTrue();
        courier.Location.Should().Be(Location.Create(3,3).Value);

        // second step
        // (3,3) - > ( 3, 7)
        result = courier.Move(location);
        result.IsSuccess.Should().BeTrue();
        courier.Location.Should().Be(Location.Create(3,7).Value);

        // third step
        // (3,7) - > ( 3, 9)
        result = courier.Move(location);
        result.IsSuccess.Should().BeTrue();
        courier.Location.Should().Be(Location.Create(3,9).Value);

    }



    [Theory]
    [InlineData(1,1)]
    [InlineData(3,1)]
    [InlineData(1,3)]
    [InlineData(7,9)]
    public void SpeedShouldBeEqualToQtyOfSteps(int x, int y)
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Pedestrian).Value;
        var location = Location.Create(x,y).Value;

        int timeToLocation = courier.CalculateTimeToPoint(location).Value;
        int qtySteps = 0;

        while(courier.Location != location)
        {
        	courier.Move(location);
        	qtySteps++;
        }

        var result = qtySteps == timeToLocation;
        result.Should().BeTrue();
    }



    [Fact]
    public void MoveToInTwoStepsDown()
    {
        //Arrange
        
        //Act
        var courier = Courier.Create("Name", Transport.Car).Value;
        var location = Location.Create(7,7).Value;

        courier.Move(location);
        courier.Move(location);
        courier.Move(location);
        courier.Move(location);
        courier.Move(location);


        // dist = 5+5 = 10, speed = 4, time = 3
        var location2 = Location.Create(2,2).Value;
        courier.Move(location2);
        courier.Move(location2);

        var result = courier.Move(location2);
        
        result.IsSuccess.Should().BeTrue();
        courier.Location.Should().Be(Location.Create(2,2).Value);
    }


}
