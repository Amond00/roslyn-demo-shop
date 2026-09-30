using DemoShop.Models;
using static DemoShop.Services.PricingPolicy;

namespace DemoShop.Services;

public class OrderService
{
    private readonly InventoryService _inventory;
    private readonly List<Order> _orders = [];
    private int _nextId = 1;

    public OrderService(InventoryService inventory)
    {
        _inventory = inventory;
    }

    public Order PlaceOrder(Customer customer, List<OrderLine> lines, string couponCode)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(lines);

        var order = new Order { Id = _nextId++, Customer = customer };
        decimal total = 0;

        foreach (var line in lines.Where(l => l.Quantity > 0))
        {
            if (line.Product.Stock < line.Quantity)
            {
                Console.WriteLine($"재고 부족: {line.Product.Name}");
                continue;
            }

            total += LinePrice(line, customer);
            order.Lines.Add(line);
            _inventory.Decrease(line.Product, line.Quantity);
        }

        total = ApplyCoupon(total, couponCode, customer);
        total = ApplyShipping(total);
        AccruePoints(customer, total);

        order.Total = total;
        order.Status = "Placed";
        _orders.Add(order);
        return order;
    }

    private static decimal LinePrice(OrderLine line, Customer customer)
    {
        var subtotal = line.Product.Price * line.Quantity;
        if (line.Product.Category != "Electronics" || line.Quantity < BulkQuantity)
            return subtotal;

        return subtotal * (customer.Grade == "VIP" ? VipBulkRate : NormalBulkRate);
    }

    private static decimal ApplyCoupon(decimal total, string couponCode, Customer customer)
    {
        if (couponCode is null)
            return total;
        if (couponCode.StartsWith("WELCOME"))
            return total - WelcomeCouponAmount;
        if (couponCode.StartsWith("VIP") && customer.Grade == "VIP")
            return total * VipCouponRate;
        return total;
    }

    private static decimal ApplyShipping(decimal total) =>
        total < FreeShippingThreshold ? total + ShippingFee : total;

    private static void AccruePoints(Customer customer, decimal total)
    {
        var rate = customer.Grade == "VIP" || customer.Points > VipPointThreshold
            ? VipPointRate
            : NormalPointRate;
        customer.Points += (int)(total * rate);
    }

    public Order FindOrder(int id) => _orders.FirstOrDefault(o => o.Id == id);

    public List<Order> GetOrdersByCustomer(Customer customer)
    {
        Guard.NotNull(customer, nameof(customer));
        return _orders.Where(o => o.Customer.Id == customer.Id).ToList();
    }

    public decimal ApplyPercentDiscount(decimal price, int percent)
    {
        return price - price * percent / 100.0m;
    }
}
