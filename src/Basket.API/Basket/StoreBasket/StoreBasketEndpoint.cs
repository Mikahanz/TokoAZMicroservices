namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);

public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<StoreBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<StoreBasketResponse>();
            return Results.Created($"/basket/{response.UserName}", response);
        })
        .WithName("Store Basket")
        .WithSummary("Store Basket")
        .WithDescription("Store Basket")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .Produces<StoreBasketResponse>(StatusCodes.Status201Created);
    }
}