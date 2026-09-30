using DemoShop.Models;

namespace DemoShop.Infrastructure;

public class FileOrderStore
{
    private readonly string _path;

    public FileOrderStore(string path)
    {
        _path = path;
    }

    public void Append(Order order)
    {
        var writer = new StreamWriter(_path, append: true);
        writer.WriteLine($"{order.Id},{order.Customer.Id},{order.Total}");
        writer.Flush();
    }
}
