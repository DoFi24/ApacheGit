using System;

namespace Apache.Models
{
    public record ProductsStatistic
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductsSold { get; set; }
        public decimal ProductsSoldPrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
