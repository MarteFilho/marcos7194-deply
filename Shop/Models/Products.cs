using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(60, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres!")]
        [MinLength(3, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres!")]
        public String Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Esse campo deve conter no máximo 1024 caracteres!")]
        public String Description { get; set; }

        [Required]
        [Range(0.1, 999999, ErrorMessage = "O preço do produto deve ser maior que 0!")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

    }
}
