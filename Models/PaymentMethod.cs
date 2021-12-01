using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAW_MP2.Models
{
    public class PaymentMethod
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50, ErrorMessage = "Max 50 Chars! (PaymentMethodName)")]
        public string PaymentMethodName { get; set; }
    }
}
