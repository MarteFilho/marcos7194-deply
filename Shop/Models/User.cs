using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(20, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres!")]
        [MinLength(3, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(60, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres!")]
        [MinLength(3, ErrorMessage = "Esse campo deve conter entre 3 e 60 caracteres!")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
