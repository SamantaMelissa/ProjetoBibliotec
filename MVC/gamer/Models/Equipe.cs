using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gamer.Models
{
    public class Equipe
    {

        [Key] //data annotation - chave primária
        public int IdEquipe { get; set; }
        public string? Nome { get; set; }
        public string? Imagem { get; set; }

        //referenciar que a classe equipe vai ter acesso 
		//á collection de Jogadores
        public ICollection<Jogador> ?Jogador { get; set; }        
            
    }
}