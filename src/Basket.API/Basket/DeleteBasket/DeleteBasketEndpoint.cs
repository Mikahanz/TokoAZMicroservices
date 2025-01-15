namespace Basket.API.Basket.DeleteBasket;

// public record DeleteBasketRequest(string UserName);
public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(userName));
            var response = result.Adapt<DeleteBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("Delete Basket")
        .WithSummary("Delete Basket")
        .WithDescription("Delete Basket")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .Produces<DeleteBasketResponse>(StatusCodes.Status201Created);;
    }
}