using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apache.Models
{
    public class Checks
    {
        public long Id { get; set; }
        public long CheckNum { get; set; }
        public DateTime PrintDate { get; set; }
        public Staffs Staffs { get; set; }
        public Places Places { get; set; }
        public decimal TotalSum { get; set; }
        [Required]
        public IEnumerable<CheckDetails> CheckDetails { get; set; }
    }
}
