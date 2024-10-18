using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotec.Models
{
    public class LivroFavorito
    {
        [Key]
        public int LivroFavoritoID { get; set; }
    
        [ForeignKey("Usuario")]//DATA ANNOTATION
        public int UsuarioID { get; set; }
        public Usuario? Usuario {get;set;}    

        [ForeignKey("Livro")]//DATA ANNOTATION
        public int LivroID { get; set; }
        public Livro? Livro {get;set;}    
    }
}