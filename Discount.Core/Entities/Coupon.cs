namespace Discount.Core.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public int Amount { get; set; }

    }
}
