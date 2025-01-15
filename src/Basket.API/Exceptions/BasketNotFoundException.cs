

namespace Basket.API.Exceptions;

public class BasketNotFoundException : NotFoundException
{
    public BasketNotFoundException(string userName) : base($"Basket not found for user: {userName}")
    {
        
    }
}