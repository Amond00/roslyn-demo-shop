using DemoShop.Models;

namespace DemoShop.Services;

public class NotificationService
{
    public async void SendOrderConfirmation(Order order)
    {
        var client = new HttpClient();
        var body = new StringContent($"{{\"orderId\":{order.Id},\"email\":\"{order.Customer.Email}\"}}");
        await client.PostAsync("https://example.com/api/notify", body);
    }

    public HttpClient CreateClient()
    {
        return new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
    }
}
