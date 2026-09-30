using DemoShop.Models;

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
        var order = new Order { Id = _nextId++, Customer = customer };
        decimal total = 0;

        foreach (var line in lines)
        {
            if (line.Quantity > 0)
            {
                if (line.Product.Stock >= line.Quantity)
                {
                    if (line.Product.Category == "Electronics")
                    {
                        if (line.Quantity >= 3)
                        {
                            if (customer.Grade == "VIP")
                            {
                                total += line.Product.Price * line.Quantity * 0.85m;
                            }
                            else
                            {
                                total += line.Product.Price * line.Quantity * 0.95m;
                            }
                        }
                        else
                        {
                            total += line.Product.Price * line.Quantity;
                        }
                    }
                    else if (line.Product.Category == "Food" || line.Product.Category == "Drink")
                    {
                        total += line.Product.Price * line.Quantity;
                    }
                    else
                    {
                        total += line.Product.Price * line.Quantity;
                    }
                    order.Lines.Add(line);
                    _inventory.Decrease(line.Product, line.Quantity);
                }
                else
                {
                    Console.WriteLine($"재고 부족: {line.Product.Name}");
                }
            }
        }

        if (couponCode != null && couponCode.StartsWith("WELCOME"))
        {
            total -= 5000;
        }
        else if (couponCode != null && couponCode.StartsWith("VIP") && customer.Grade == "VIP")
        {
            total *= 0.9m;
        }

        if (total < 50000)
        {
            total += 3000;
        }

        if (customer.Grade == "VIP" || customer.Points > 10000)
        {
            customer.Points += (int)(total * 0.05m);
        }
        else
        {
            customer.Points += (int)(total * 0.01m);
        }

        order.Total = total;
        order.Status = "Placed";
        _orders.Add(order);
        return order;
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
