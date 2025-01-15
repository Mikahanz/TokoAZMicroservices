namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotNull().WithMessage("Username is required");
    }
}

internal class DeleteBasketCommandHandler(IDocumentSession session) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        // To Do:  Delete the cart in the database (use Marten)
        // To Do: Update cache (use Redis)
        return new DeleteBasketResult(true);
    }
}