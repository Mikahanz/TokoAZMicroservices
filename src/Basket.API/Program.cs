using BuildingBlocks.Exceptions.Handler;
using Discount.gRPC;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container ----------------------

    // Carter is a library that allows you to create HTTP APIs using a minimalistic approach
    builder.Services.AddCarter();
 
    // MediatR is a simple, unambitious mediator implementation in .NET
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });

    // Marten is a .NET transactional document DB and event store on PostgreSQL
    builder.Services.AddMarten(options =>
    {
        options.Connection(builder.Configuration.GetConnectionString("Database")!); 
        options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    }).UseLightweightSessions();

    // Basket Repository
    builder.Services.AddScoped<IBasketRepository, BasketRepository>();
    builder.Services.Decorate<IBasketRepository, CacheBasketRepository>();
    // Add Redis Cache
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis")!;
    });

    // Add Discount gRPC Service
    builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
    }).ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        return handler;
    });

    // Custom Exception Handler
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    // Health Checks
    builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
                                        .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

// End of Services --------------------------------------

var app = builder.Build();

    // Configure the HTTP request pipeline
    app.MapCarter();
    // Exception Handler
    app.UseExceptionHandler(options => { });
    // Health Checks
    app.UseHealthChecks("/health", 
        new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

app.Run();