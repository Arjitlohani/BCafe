

namespace BCafe.Data
{
    public class Coffee
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
