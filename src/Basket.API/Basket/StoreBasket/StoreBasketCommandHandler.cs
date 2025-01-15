namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
    }
}

internal class StoreBasketCommandHandler(IDocumentSession session) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart cart = command.Cart;
        
        // To Do:  Store the cart in the database (use Marten upsert - if cart exists, update it, otherwise insert it)
        // To Do: Update cache (use Redis)
        
        return new StoreBasketResult(command.Cart.UserName);
    }
}