namespace DemoShop.Services;

public static class PricingPolicy
{
    public const int BulkQuantity = 3;
    public const decimal VipBulkRate = 0.85m;
    public const decimal NormalBulkRate = 0.95m;

    public const decimal WelcomeCouponAmount = 5000m;
    public const decimal VipCouponRate = 0.9m;

    public const decimal FreeShippingThreshold = 50000m;
    public const decimal ShippingFee = 3000m;

    public const int VipPointThreshold = 10000;
    public const decimal VipPointRate = 0.05m;
    public const decimal NormalPointRate = 0.01m;
}
