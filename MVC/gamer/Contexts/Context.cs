using gamer.Models;
using Microsoft.EntityFrameworkCore;

namespace gamer.Contexts
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
                optionsBuilder.UseSqlServer("Data Source = DESKTOP-LAO5MIJ\\SQLEXPRESSTEC; initial catalog = gamer; User=sa;Password=abc123; Integrated Security = true; TrustServerCertificate=True");
            }
        }

        public DbSet<Jogador> Jogador { get; set; }
        public DbSet<Equipe> Equipe { get; set; }
    }

}