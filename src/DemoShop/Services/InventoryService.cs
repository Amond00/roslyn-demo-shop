using DemoShop.Models;

namespace DemoShop.Services;

public class InventoryService
{
    private readonly Dictionary<int, Product> _products = new();

    public void Register(Product product)
    {
        _products[product.Id] = product;
    }

    public void Decrease(Product product, int quantity)
    {
        product.Stock -= quantity;
    }

    public void UpdatePrice(Product product, decimal newPrice)
    {
        if (product is not { Id: > 0 })
            throw new ArgumentException("유효한 상품이 아닙니다.", nameof(product));
        product.Price = newPrice;
    }

    public void Restock(int productId, int amount)
    {
        try
        {
            _products[productId].Stock += amount;
        }
        catch (KeyNotFoundException)
        {
        }
    }

    public List<Product> GetLowStockElectronics()
    {
        var result = new List<Product>();
        var sorted = _products.Values.OrderBy(p => p.Stock).ToList();
        foreach (var p in sorted.Where(p => p.Category == "Electronics" && p.Stock < 5))
            result.Add(p);
        Console.WriteLine($"조회 완료: {result.Count}건");
        return result;
    }

    public List<Product> GetLowStockFood()
    {
        var result = new List<Product>();
        var sorted = _products.Values.OrderBy(p => p.Stock).ToList();
        foreach (var p in sorted.Where(p => p.Category == "Food" && p.Stock < 5))
            result.Add(p);
        Console.WriteLine($"조회 완료: {result.Count}건");
        return result;
    }
}
