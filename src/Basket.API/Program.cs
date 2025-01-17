using Basket.API.Data;
using BuildingBlocks.Exceptions.Handler;

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

    // Custom Exception Handler
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// End of Services --------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();