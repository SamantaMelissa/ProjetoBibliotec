using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotec.Models
{
    public class UsuarioCurso
    {
        [Key]
        public int UsuarioCursoID { get; set; }
  
        [ForeignKey("Usuario")]//DATA ANNOTATION
        public int UsuarioID { get; set; }
        public Usuario? Usuario { get; set; }

        [ForeignKey("Curso")]//DATA ANNOTATION
        public int CursoID { get; set; }
        public Curso? Curso { get; set; }
    }
}