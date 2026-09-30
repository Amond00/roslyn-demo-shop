using DemoShop.Models;

namespace DemoShop.Services;

public class NotificationService
{
    private static readonly HttpClient Http = new();

    public async Task SendOrderConfirmationAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        using var body = new StringContent($"{{\"orderId\":{order.Id},\"email\":\"{order.Customer.Email}\"}}");
        using var response = await Http.PostAsync("https://example.com/api/notify", body);
        response.EnsureSuccessStatusCode();
    }

    public HttpClient CreateClient()
    {
        return new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
    }
}
