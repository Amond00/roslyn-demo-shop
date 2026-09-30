using DemoShop.Models;

namespace DemoShop.Services;

public class CustomerService
{
    private const int VipPointThreshold = 10000;

    private readonly Dictionary<int, Customer> _customers = new();

    public void Add(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        _customers[customer.Id] = customer;
    }

    public Customer Find(int id)
    {
        return _customers.TryGetValue(id, out var customer) ? customer : null;
    }

    public void UpgradeIfEligible(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        if (customer.Points >= VipPointThreshold)
            customer.Grade = "VIP";
    }

    public IReadOnlyList<Customer> GetVipCustomers()
    {
        return _customers.Values
            .Where(c => c.Grade == "VIP")
            .OrderByDescending(c => c.Points)
            .ToList();
    }

    public int CountByGrade(string grade)
    {
        return _customers.Values.Count(c => c.Grade == grade);
    }
}
