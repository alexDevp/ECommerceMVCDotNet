using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DAW_MP2.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "Nome do Produto")]
        [MaxLength(50, ErrorMessage = "Max 50 Chars!")]
        public string ProductName { get; set; }

        [Display(Name = "Quantidade em Inventário")]
        public int Stock { get; set; }

        [Display(Name = "Preço")]
        public int Price { get; set; }
        [MaxLength(10000, ErrorMessage = "Max 10000 Chars!")]

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Especificações")]
        [MaxLength(10000, ErrorMessage = "Max 10000 Chars!")]
        public string Specifications { get; set; }

        [MaxLength(10000, ErrorMessage = "Max 10000 Chars!")]
        [Display(Name = "Imagem")]
        public string Image { get; set; }

        public IFormFile ImageFile { get; set; }

        [Display(Name = "Produto Visivel")]
        public bool FlagActive { get; set; }

        [Display(Name = "Categoria")]
        public string Category { get; set; }

    }
}
