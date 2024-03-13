using CSharpFunctionalExtensions;
using Primitives;
using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Ports;

public interface IGeoClient 
{
	Task<Result<Location,Error>> GetLocation(string address);
}

