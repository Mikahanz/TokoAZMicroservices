using System.Data;
using Npgsql;
using Discount.gRPC.Models;

namespace Discount.gRPC.Data;

public class DiscountContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    
    public DiscountContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("Database");
    }
    
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}