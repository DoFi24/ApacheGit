﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apache.Models
{
    public class Categories
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Products>? Products { get; set; }
    }
}
