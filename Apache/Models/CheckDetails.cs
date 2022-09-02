namespace Apache.Models
{
    public class CheckDetails
    {
        public long Id { get; set; }
        public long ChecksId { get; set; }
        public long ProductsPrihodId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
