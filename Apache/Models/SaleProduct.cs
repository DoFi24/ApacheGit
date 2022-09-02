namespace Apache.Models
{
    public class SaleProduct
    {
        public long Id { get; set; }
        public ProductsPrihod? Products { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Summa { get=> (Quantity * Price); }
        public decimal Itog { get => (Quantity * Price)-((Quantity * Price)/100*Discount); }
    }
}
