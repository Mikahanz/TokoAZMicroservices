using System.Data;
using Dapper;
using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Grpc.Core;
using Mapster;

namespace Discount.gRPC.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDbConnection _dbConnection;
    private ILogger<DiscountService> logger;

    public DiscountService(IDbConnection dbConnection, ILogger<DiscountService> logger)
    {
        _dbConnection = dbConnection;
        this.logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        const string query = "SELECT * FROM \"Coupons\" WHERE \"ProductName\" = @ProductName";
        var coupon = await _dbConnection.QueryFirstOrDefaultAsync<Coupon>(query, new { ProductName = request.ProductName });

        if (coupon is null)
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

        logger.LogInformation("Discount is retrieved for ProductName : {prodName}, Amount : {amount}",
            coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var requestCoupon = request.Coupon.Adapt<Coupon>();
        if(requestCoupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        
        const string query = "INSERT INTO \"Coupons\" (\"ProductName\", \"Description\", \"Amount\") VALUES (@ProductName, @Description, @Amount) RETURNING *";
        var resultCoupon = await _dbConnection.QueryFirstOrDefaultAsync<Coupon>(query, new
        {
            ProductName = requestCoupon.ProductName,
            Description = requestCoupon.Description,
            Amount = requestCoupon.Amount
        });
        
        if (resultCoupon is null)
            throw new RpcException(new Status(StatusCode.Internal, "Failed to create discount"));
        
        logger.LogInformation($"Result : {resultCoupon}");
        logger.LogInformation("Discount is successfully created. ProductName : {prodName}", resultCoupon.ProductName);
        
        var couponModel = resultCoupon.Adapt<CouponModel>();
        return couponModel;
    }
    
    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var requestCoupon = request.Coupon.Adapt<Coupon>();
        if(requestCoupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        
        const string query = "UPDATE \"Coupons\" SET \"ProductName\" = @ProductName, \"Description\" = @Description, \"Amount\" = @Amount WHERE \"Id\" = @Id RETURNING *";
        var resultCoupon = await _dbConnection.QueryFirstOrDefaultAsync<Coupon>(query, new
        {
            Id = requestCoupon.Id,
            ProductName = requestCoupon.ProductName,
            Description = requestCoupon.Description,
            Amount = requestCoupon.Amount
        });
        
        if (resultCoupon is null)
            throw new RpcException(new Status(StatusCode.Internal, "Failed to update discount"));
        
        logger.LogInformation("Discount is successfully updated. ProductName : {prodName}", resultCoupon.ProductName);
        
        var couponModel = resultCoupon.Adapt<CouponModel>();
        return couponModel;
    }
    //
    // public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    // {
    //     var coupon = await discountContext
    //         .Coupons
    //         .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
    //     
    //     if(coupon is null)
    //         throw new RpcException(new Status(StatusCode.NotFound, $"Discount with productName: {request.ProductName} is not found"));
    //     
    //     discountContext.Coupons.Remove(coupon);
    //     await discountContext.SaveChangesAsync();
    //     logger.LogInformation("Discount is successfully deleted. ProductName : {prodName}", coupon.ProductName);
    //     
    //     return new DeleteDiscountResponse { Success = true };
    // }
}