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

    // FluentValidation is a popular .NET library for building strongly-typed validation rules
    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    // Marten is a .NET transactional document DB and event store on PostgreSQL
    builder.Services.AddMarten(options =>
    {
        options.Connection(builder.Configuration.GetConnectionString("Database")!); 
    }).UseLightweightSessions();

    // Initialize Marten with initial data
    if (builder.Environment.IsDevelopment())
        builder.Services.InitializeMartenWith<CatalogInitialData>();

    // Exception Handler
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    
    // Health Checks
    builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

// End of Services --------------------------------------

var app = builder.Build();

    // Configure the HTTP request pipeline
    app.MapCarter();
    // Exception Handler
    app.UseExceptionHandler(options => { });
    // Health Checks
    app.UseHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();