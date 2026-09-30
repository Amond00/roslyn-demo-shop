using System.Text;
using DemoShop.Models;

namespace DemoShop.Services;

public class ReportService
{
    public string BuildDailyReport(List<Order> orders)
    {
        ArgumentNullException.ThrowIfNull(orders);

        var report = new StringBuilder();
        foreach (var order in orders)
        {
            report.AppendLine($"#{order.Id} {order.Customer.Name} {order.Total:N0}원 {order.Status}");
        }
        return report.ToString();
    }
}
