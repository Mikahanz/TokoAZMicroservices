using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand( string Name, 
                                    string Description, 
                                    List<string> Category, 
                                    decimal Price, 
                                    string ImageFile) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
    }
}

internal class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger) 
                                                        : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateProductCommandHandler.Handle called with command: {@command}", command);
        
        // create product object from command object
        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Category = command.Category,
            Price = command.Price
        };
        
        // Save to Database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        // Return CreateProductResult result
        return new CreateProductResult(product.Id);
    }
}