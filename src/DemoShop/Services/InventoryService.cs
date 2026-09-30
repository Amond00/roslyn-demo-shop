using DemoShop.Models;

namespace DemoShop.Services;

public class InventoryService
{
    private const int LowStockThreshold = 5;

    private readonly Dictionary<int, Product> _products = new();

    public void Register(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        _products[product.Id] = product;
    }

    public void Decrease(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        product.Stock -= quantity;
    }

    public void UpdatePrice(Product product, decimal newPrice)
    {
        if (product is not { Id: > 0 })
            throw new ArgumentException("유효한 상품이 아닙니다.", nameof(product));
        product.Price = newPrice;
    }

    public bool Restock(int productId, int amount)
    {
        if (!_products.TryGetValue(productId, out var product))
            return false;

        product.Stock += amount;
        return true;
    }

    public List<Product> GetLowStockElectronics() => GetLowStock("Electronics");

    public List<Product> GetLowStockFood() => GetLowStock("Food");

    private List<Product> GetLowStock(string category)
    {
        var result = _products.Values
            .Where(p => p.Category == category && p.Stock < LowStockThreshold)
            .OrderBy(p => p.Stock)
            .ToList();
        Console.WriteLine($"조회 완료: {result.Count}건");
        return result;
    }
}
