using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apache.Models
{
    public class Checks
    {
        public long Id { get; set; }
        public int IsActive { get; set; } //0 - Активный , 1 - Завершенный , 2 - Удаленный
        public DateTime? PrintDate { get; set; }
        public int? StaffsId { get; set; }
        public int? PlacesId { get; set; }
        public decimal TotalSum { get; set; }
    }
}
