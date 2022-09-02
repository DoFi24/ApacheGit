using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apache.Models
{
    public class CheckPageModel
    {
        public long Id { get; set; }
        public string IsActive { get; set; } = "White"; //0 - Активный , 1 - Завершенный , 2 - Удаленный
        public DateTime? PrintDate { get; set; }
        public decimal TotalSum { get; set; }
        public IEnumerable<CheckModel>? ChecksList { get; set; }
    }
    public record class CheckModel(string ProductName,int Quantity,decimal Discount,decimal Price);
}
