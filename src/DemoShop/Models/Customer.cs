namespace DemoShop.Models;

public class Customer
{
    public int Id;
    public string Name;
    public string Email;
    public string Grade { get; set; } = "Normal";
    public int Points { get; set; }
}
