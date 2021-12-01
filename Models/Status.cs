using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAW_MP2.Models
{
    public class Status
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50, ErrorMessage = "Max 50 Chars! (StatusName)")]
        public string StatusName { get; set; }
    }
}
