using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotec.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }
        public string? Nome { get; set; }
        public DateOnly DtNascimento { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public char Contato { get; set; }
        public bool Admin { get; set; }
        public bool Status { get; set; }
    }
}