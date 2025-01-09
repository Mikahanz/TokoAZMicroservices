namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand( string Name, 
                                    string Description, 
                                    List<string> Category, 
                                    decimal Price, 
                                    string ImageFile) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
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