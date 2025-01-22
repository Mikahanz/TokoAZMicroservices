using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Services;

public class DiscountService(DiscountContext discountContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await discountContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon is null)
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

        logger.LogInformation("Discount is retrieved for ProductName : {prodName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if(coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        
        discountContext.Coupons.Add(coupon);
        await discountContext.SaveChangesAsync();
        logger.LogInformation("Discount is successfully created. ProductName : {prodName}", coupon.ProductName);
        
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    
    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if(coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        
        discountContext.Coupons.Update(coupon);
        await discountContext.SaveChangesAsync();
        logger.LogInformation("Discount is successfully updated. ProductName : {prodName}", coupon.ProductName);
        
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await discountContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        
        if(coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with productName: {request.ProductName} is not found"));
        
        discountContext.Coupons.Remove(coupon);
        await discountContext.SaveChangesAsync();
        logger.LogInformation("Discount is successfully deleted. ProductName : {prodName}", coupon.ProductName);
        
        return new DeleteDiscountResponse { Success = true };
    }
}