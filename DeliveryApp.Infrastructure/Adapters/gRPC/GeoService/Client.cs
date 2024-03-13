using CSharpFunctionalExtensions;
using Primitives;
using DeliveryApp.Core.Ports;
using DeliveryApp.Core.Domain.SharedKernel;

using Google.Protobuf.Collections;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;

using GeoApp.Ui;

namespace DeliveryApp.Infrastructure.Adapters.Grpc;


public class Client : IGeoClient
{

    private readonly string _url;
    private readonly SocketsHttpHandler _socketsHttpHandler;
    private readonly MethodConfig _methodConfig;

    public Client(string url)
    {
        _url = url;
            
        _socketsHttpHandler = new SocketsHttpHandler 
        { 
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan, 
            KeepAlivePingDelay = TimeSpan.FromSeconds(60), 
            KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
            EnableMultipleHttp2Connections = true 
        };
        
        _methodConfig = new MethodConfig
        {
            Names = { MethodName.Default },
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = 5,
                InitialBackoff = TimeSpan.FromSeconds(1),
                MaxBackoff = TimeSpan.FromSeconds(5),
                BackoffMultiplier = 1.5,
                RetryableStatusCodes = { StatusCode.Unavailable }
            }
        };
    }


	public async Task<Result<DeliveryApp.Core.Domain.SharedKernel.Location, Error>> GetLocation(string address)
	{
		if(String.IsNullOrWhiteSpace(address)) return GeneralErrors.ValueIsRequired(nameof(address));

        using var channel = GrpcChannel.ForAddress(_url, new GrpcChannelOptions 
        { 
            HttpHandler = _socketsHttpHandler,
            ServiceConfig = new ServiceConfig { MethodConfigs = { _methodConfig } }
        });


        var client = new Geo.GeoClient(channel);
        try
        {
            var reply = await client.GetGeolocationAsync(new GeoApp.Ui.GetGeolocationRequest
            {
                Address = address
            },null, deadline: DateTime.UtcNow.AddSeconds(2));


            var location = DeliveryApp.Core.Domain.SharedKernel.Location.Create(reply.Location.X, reply.Location.Y);

        }
        catch (RpcException)
        {
            //Fallback
            return DeliveryApp.Core.Domain.SharedKernel.Location.MinLocation;
        }


		return DeliveryApp.Core.Domain.SharedKernel.Location.MinLocation;
	}
}