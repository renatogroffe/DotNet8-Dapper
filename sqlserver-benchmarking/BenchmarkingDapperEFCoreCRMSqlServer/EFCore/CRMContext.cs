using BenchmarkingDapperEFCoreCRMSqlServer;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingDapperEFCoreCRMSqlServer.EFCore;

public class CRMContext : DbContext
{
    public DbSet<Empresa>? Empresas { get; set; }
    public DbSet<Contato>? Contatos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configurations.BaseEFCore);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empresa>().HasMany(r => r.Contatos);
        modelBuilder.Entity<Contato>().HasOne(e => e.EmpresaRepresentada);
    }
}