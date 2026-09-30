using DemoShop.Infrastructure;
using DemoShop.Models;
using DemoShop.Services;

var inventory = new InventoryService();
var laptop = new Product { Id = 1, Name = "노트북", Price = 1200000, Stock = 10, Category = "Electronics" };
var snack = new Product { Id = 2, Name = "과자", Price = 1500, Stock = 3, Category = "Food" };
inventory.Register(laptop);
inventory.Register(snack);

var orders = new OrderService(inventory);
var customer = new Customer { Id = 1, Name = "홍길동", Email = "hong@example.com", Grade = "VIP" };
var order = orders.PlaceOrder(customer,
    [new OrderLine { Product = laptop, Quantity = 3 }, new OrderLine { Product = snack, Quantity = 2 }],
    "WELCOME2026");

new FileOrderStore("orders.csv").Append(order);
Console.WriteLine(new ReportService().BuildDailyReport([order]));
Console.WriteLine($"적립 포인트: {customer.Points}");
