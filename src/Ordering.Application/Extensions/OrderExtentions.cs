using Ordering.Application.Dtos;
using Ordering.Domain.Models;

namespace Ordering.Application.Extensions;

public static class OrderExtentions
{
    public static List<OrderDto> MapToOrderDtos(this List<Order> orders)
    {
        List<OrderDto> orderDtos = new();
        
        foreach (var order in orders)
        {
            var orderDto = new OrderDto
            (
                order.Id.Value,
                order.CustomerId.Value,
                order.OrderName.Value,
                new AddressDto
                (
                    order.ShippingAddress.FirstName,
                    order.ShippingAddress.LastName,
                    order.ShippingAddress.EmailAddress,
                    order.ShippingAddress.AddressLine,
                    order.ShippingAddress.Country,
                    order.ShippingAddress.State,
                    order.ShippingAddress.ZipCode
                ),
                new AddressDto
                (
                    order.BillingAddress.FirstName,
                    order.BillingAddress.LastName,
                    order.BillingAddress.EmailAddress,
                    order.BillingAddress.AddressLine,
                    order.BillingAddress.Country,
                    order.BillingAddress.State,
                    order.BillingAddress.ZipCode
                ),
                new PaymentDto
                (
                    order.Payment.CardName,
                    order.Payment.CardNumber,
                    order.Payment.Expiration,
                    order.Payment.CVV,
                    order.Payment.PaymentMethod
                ),
                order.Status,
                order.OrderItems.Select(oi => new OrderItemDto
                (
                    oi.OrderId.Value,
                    oi.ProductId.Value,
                    oi.Quantity,
                    oi.Price
                )).ToList()
            );
            
            orderDtos.Add(orderDto);
        }
        
        return orderDtos;
    }
}