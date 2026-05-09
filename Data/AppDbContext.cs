using Microsoft.EntityFrameworkCore;
using ControleEstoqueApi.Models;

namespace ControleEstoqueApi.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Produto>      Produtos      => Set<Produto>();
    public DbSet<TipoProduto>  TiposProduto  => Set<TipoProduto>();
    public DbSet<Estoquista>   Estoquistas   => Set<Estoquista>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Produto>(entity => {
            entity.Property(p => p.Nome).HasMaxLength(120).IsRequired();
            entity.Property(p => p.Preco).HasPrecision(18, 2);

            // Relacionamento: Produto pertence a um TipoProduto
            entity.HasOne(p => p.TipoProduto)
                  .WithMany(t => t.Produtos)
                  .HasForeignKey(p => p.TipoProdutoId)
                  .OnDelete(DeleteBehavior.Restrict); // Impede exclusão se houver produtos
        });

        modelBuilder.Entity<TipoProduto>(entity => {
            entity.Property(t => t.Nome).HasMaxLength(80).IsRequired();
            entity.Property(t => t.Descricao).HasMaxLength(200);
        });

        modelBuilder.Entity<Estoquista>(entity => {
            entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(200).IsRequired();
        });
    }
}
