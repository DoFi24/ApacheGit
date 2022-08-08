using System;

namespace Apache.Models
{
    public class ProductsPrihod
    {
        public long Id { get; set; }
        public Products Products { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime PrihodDate { get; set; }
    }
}
