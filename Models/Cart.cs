using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAW_MP2.Models
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }
        
        public Guid IdUser { get; set; }

        public Guid IdStatus { get; set; }

        public Guid IdPaymentMethod { get; set; }

        [MaxLength(100, ErrorMessage = "Max 100 Chars! (Address)")]
        public string Address { get; set; }
    }
}
