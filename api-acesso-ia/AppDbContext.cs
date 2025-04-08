using api_acesso_ia.Models;
using Microsoft.EntityFrameworkCore;

namespace api_acesso_ia
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> op)
            : base(op) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Acesso> Acessos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder
                                              optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseMySql("Server=localhost;Database=db_api_acesso_ia;User=root;Password=;",
                    new MySqlServerVersion(new Version(5, 7, 0)),
                    op => op.EnableRetryOnFailure()
                    );
            }

        }
    }
}