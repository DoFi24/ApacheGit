using System;
using System.Collections.Generic;

namespace Apache.Models
{
    public class CheckPageModel
    {
        public long Id { get; set; }
        public string IsActive { get; set; } = "White";
        public DateTime? PrintDate { get; set; }
        public decimal TotalSum { get; set; }
        public IEnumerable<CheckModel>? ChecksList { get; set; }
    }
    public record class CheckModel(string ProductName,int Quantity,decimal Discount,decimal Price);
}
