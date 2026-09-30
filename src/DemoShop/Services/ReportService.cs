using DemoShop.Models;

namespace DemoShop.Services;

public class ReportService
{
    public string BuildDailyReport(List<Order> orders)
    {
        string report = "";
        var generatedAt = DateTime.Now;
        foreach (var order in orders)
        {
            report += $"#{order.Id} {order.Customer.Name} {order.Total:N0}원 {order.Status}\n";
        }
        return report;
    }
}
