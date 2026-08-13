// Criando modelo de dados para representar a tabela de produtos no banco de dados. Cada instância da classe Produto corresponde a uma linha na tabela "produtos".
using EstoqueApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueApi.Data;


public class EstoqueDbContext : DbContext
{
    public EstoqueDbContext(DbContextOptions<EstoqueDbContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos => Set<Produto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("produtos");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Codigo).IsRequired().HasMaxLength(50);
            entity.HasIndex(p => p.Codigo).IsUnique();
            entity.Property(p => p.Descricao).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Saldo).IsRequired();
        });
    }
}
