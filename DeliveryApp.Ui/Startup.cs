using Microsoft.EntityFrameworkCore;

using DeliveryApp.Infrastructure;

using DeliveryApp.Core.Ports;
using DeliveryApp.Infrastructure.Adapters.Postgres;



namespace DeliveryApp.Ui;

public class Startup
{
    public Startup()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables();
        var configuration = builder.Build();
        Configuration = configuration;
    }

    /// <summary>
    /// Конфигурация
    /// </summary>
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configuration
        services.Configure<Settings>(options => Configuration.Bind(options));
        var connectionString = Configuration["CONNECTION_STRING"];
        var rabbitMqHost = Configuration["RABBIT_MQ_HOST"];

        // БД 
        services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("DeliveryApp.Infrastructure");
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                    });
                options.EnableSensitiveDataLogging();                
            }
        );

        //Postgres
        services.AddTransient<ICourierRepository, CourierRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();


        //MediatR Commands
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
        // services.AddTransient<IRequestHandler<Core.Application.UseCases.Commands.AddOrder.Command,bool>,
        //     Core.Application.UseCases.Commands.AddOrder.Handler>();
        // services.AddTransient<IRequestHandler<Core.Application.UseCases.Commands.CompleteOrder.Command,bool>,
        //     Core.Application.UseCases.Commands.CompleteOrder.Handler>();

        //MediatR Queries
        // services.AddTransient<IRequestHandler<Core.Application.UseCases.Queries.GetOrder.Query,
        //     Core.Application.UseCases.Queries.GetOrder.Response>>(x 
        //     => new Core.Application.UseCases.Queries.GetOrder.Handler(connectionString));


        //MediatR Domain Event Handlers
        //services.AddTransient<INotificationHandler<OrderCompletedDomainEvent>,DeliveryConfirmedDomainEventHandler>();

        //HTTP Handlers
        // services.AddControllers(options =>
        //     {
        //         options.InputFormatters.Insert(0, new InputFormatterStream());
        //     })
        //     .AddNewtonsoftJson(opts =>
        //     {
        //         opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //         opts.SerializerSettings.Converters.Add(new StringEnumConverter
        //         {
        //             NamingStrategy = new CamelCaseNamingStrategy()
        //         });
        //     });

        //Swagger
        // services.AddSwaggerGen(c =>
        // {
        //     c.SwaggerDoc("1.0.0", new OpenApiInfo
        //     {
        //         Title = "Swagger Petstore",
        //         Description = "Swagger Petstore (ASP.NET Core 3.1)",
        //         TermsOfService = new Uri("http://swagger.io/terms/"),
        //         Contact = new OpenApiContact
        //         {
        //             Name = "Swagger API Team",
        //             Url = new Uri("http://swagger.io"),
        //             Email = "apiteam@swagger.io"
        //         },
        //         License = new OpenApiLicense
        //         {
        //             Name = "NoLicense",
        //             Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
        //         },
        //         Version = "1.0.0",
        //     });
        //     c.CustomSchemaIds(type => type.FriendlyId(true));
        //     c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly().GetName().Name}.xml");
        //     // Sets the basePath property in the OpenAPI document generated
        //     c.DocumentFilter<BasePathFilter>("");
        //
        //     // Include DataAnnotation attributes on Controller Action parameters as OpenAPI validation rules (e.g required, pattern, ..)
        //     // Use [ValidateModelState] on Actions to actually validate it in C# as well!
        //     c.OperationFilter<GeneratePathParamsValidationFilter>();
        // });
        // services.AddSwaggerGenNewtonsoftSupport();
        services.AddHealthChecks();

        //Message Broker
        // services.AddMassTransit(x =>
        // {
        //     //Consumers
        //     x.AddConsumer<BasketConfirmedConsumer>();
        //     x.UsingRabbitMq((context,cfg) =>
        //     {
        //         cfg.Host(rabbitMqHost, "/", h => {
        //             h.Username("guest");
        //             h.Password("guest");
        //         });
        //         cfg.ConfigureEndpoints(context);
        //     });
        // });

        Console.WriteLine("--------");
        foreach (var service in services)
        {
            Console.WriteLine($"{service.ServiceType.FullName},{service.ImplementationType?.FullName}");
        }
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        // app.UseDefaultFiles();
        // app.UseStaticFiles();
        // app.UseSwagger(c =>
        //     {
        //         c.RouteTemplate = "openapi/{documentName}/openapi.json";
        //     })
        //     .UseSwaggerUI(c =>
        //     {
        //         // set route prefix to openapi, e.g. http://localhost:80/openapi/index.html
        //         c.RoutePrefix = "openapi";
        //         //TODO: Either use the SwaggerGen generated OpenAPI contract (generated from C# classes)
        //         c.SwaggerEndpoint("/openapi/1.0.0/openapi.json", "Swagger Petstore");
        //         c.RoutePrefix = string.Empty;
        //         //TODO: Or alternatively use the original OpenAPI contract that's included in the static files
        //         c.SwaggerEndpoint("/openapi-original.json", "Swagger Petstore Original");
        //     });

        app.UseHealthChecks("/health");
        app.UseRouting();
        //app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
