namespace Altkom.Shopper.Api.Extensions;

public class ProductEndpoints
{
    public static string GetAll(string color, decimal maxPrice) => $"Product {color} {maxPrice}";
}
