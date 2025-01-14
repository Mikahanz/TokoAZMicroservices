namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);

internal class GetBasketQueryHandler(IDocumentSession session) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        // var shoppingCard = await session.LoadAsync<ShoppingCart>(query.UserName);
        return new GetBasketResult(new ShoppingCart("test"));
    }
}