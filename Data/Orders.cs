namespace BCafe.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string Coffee { get; set; }
        public string AddIns { get; set; }
        public decimal CoffeePrice { get; set; }
        public decimal AddInsPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CustomerId { get; set; }
        
    }
}
