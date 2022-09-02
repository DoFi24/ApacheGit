using System;

namespace Apache.Models
{
    public class ProductsPrihod
    {
        public long Id { get; set; }
        public long ProductsId { get; set; }
        public string ProductsName { get; set; }
        public int Quantity { get; set; }
        public int ComeQuantity { get; set; }
        public decimal Price { get; set; }
        public DateTime? PrihodDate { get; set; }
    }
}
