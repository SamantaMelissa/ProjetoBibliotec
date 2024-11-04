using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bibliotec.Models
{
    public class Curso
    {
        [Key]
        // sse atributo permite inserir o PK para o curso em vez de fazer com que o banco de dados o gere:
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CursoID { get; set; }
        [StringLength(100)]
        public string? Nome { get; set; }
        public char Periodo { get; set; }

        
    }
}