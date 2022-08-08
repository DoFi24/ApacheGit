namespace Apache.Models
{
    public class CheckDetails
    {
        public long Id { get; set; }
        public ProductsPrihod ProductsPrihod { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
