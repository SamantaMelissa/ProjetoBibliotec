using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec.Models;
using Microsoft.EntityFrameworkCore;

namespace Bibliotec.Contexts
{
    public class Context : DbContext
    {
        public Context()
        {
        }
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //string de conexão com o banco
                //Data Source : o nome do servidor do gerenciador do banco
                //initial catalog : nome do banco de dados

                //Autenticação pelo Windows
                //Integrated Security : Autenticação pelo Windows
                //TrustServerCertificate : Autenticação pelo Windows

                //Autenticação pelo SqlServer
                //user Id = "nome do seu usuario de login"
                //pwd = "senha do seu usuario"
                // optionsBuilder.UseSqlServer("Data Source = DESKTOP-LAO5MIJ\\SQLEXPRESS; Initial Catalog = Bibliotec; Integrated Security=true; TrustServerCertificate = true");

                optionsBuilder.UseSqlServer("Data Source=DESKTOP-LAO5MIJ\\SQLEXPRESSTEC; Initial Catalog = Bibliotec; User Id=sa; Password=abc123; Integrated Security=true; TrustServerCertificate = true");
                // optionsBuilder.UseSqlServer("Data Source = localhost; Initial Catalog = gamerManha; User Id=sa; Pwd=123abc; TrustServerCertificate = true");
            }
        }

        //refência de classes e tabelas

        //Preciso saber o pq do plural e pq nao aceita... Acho q é pq tem que estar com o nome do banco de dados...
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Livro> Livro { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<LivroCategoria> LivroCategoria { get; set; }
        public DbSet<LivroReserva> LivroReservas { get; set; }
        public DbSet<LivroFavorito> LivroFavoritos { get; set; }
        public DbSet<UsuarioCurso> UsuarioCursos { get; set; }

    }
}