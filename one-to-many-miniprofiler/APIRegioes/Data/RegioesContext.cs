using Microsoft.EntityFrameworkCore;

namespace APIRegioes.Data;

public class RegioesContext : DbContext
{
    public DbSet<Regiao>? Regioes { get; set; }
    public DbSet<Estado>? Estados { get; set; }

    public RegioesContext(DbContextOptions<RegioesContext> options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Regiao>().HasMany(r => r.Estados);
        modelBuilder.Entity<Estado>().HasOne(e => e.Regiao);
    }
}
