using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAW_MP2.Models
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdCart { get; set; }

        public Guid IdItem { get; set; }
    }
}
