using Core.Iottu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Iottu.Persistence.Contexts
{
    public class IottuDbContext : DbContext
    {
        public IottuDbContext(DbContextOptions<IottuDbContext> options)
            : base(options) { }

        public DbSet<Moto> Motos { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<Antena> Antenas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moto>(entity =>
            {
                entity.ToTable("Motos");
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Placa)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(m => m.Modelo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.Cor)
                    .HasMaxLength(30);

                entity.HasOne(m => m.Tag)
                    .WithOne(t => t.Moto)
                    .HasForeignKey<Moto>(m => m.TagId);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tags");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Codigo)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Antena>(entity =>
            {
                entity.ToTable("Antenas");
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Localizacao)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(a => a.Identificador)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
