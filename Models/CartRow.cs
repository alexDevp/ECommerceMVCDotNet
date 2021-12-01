using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAW_MP2.Models
{
    public class CartRow
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int Amount { get; set; }
    }
}
