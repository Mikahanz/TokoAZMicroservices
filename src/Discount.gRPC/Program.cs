using System.Data;
using Discount.gRPC;
using Discount.gRPC.Data;
using Discount.gRPC.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

// Register IDbConnection for Dapper
builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();

if (app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();