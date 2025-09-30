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
        public DbSet<StatusMoto> Status { get; set; } = null!;
        public DbSet<Patio> Patios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moto>()
            .HasOne(m => m.Tag)
            .WithOne(t => t.Moto)
            .HasForeignKey<Moto>(m => m.TagId);

            modelBuilder.Entity<Tag>()
                .HasOne(t => t.Antena)
                .WithMany(a => a.Tags)
                .HasForeignKey(t => t.AntenaId);

            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Patio)
                .WithMany(p => p.Motos)
                .HasForeignKey(m => m.PatioId);

            modelBuilder.Entity<Antena>()
                .HasOne(a => a.Patio)
                .WithMany(p => p.Antenas)
                .HasForeignKey(a => a.PatioId);

            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Status)
                .WithMany(s => s.Motos)
                .HasForeignKey(m => m.StatusId);

            modelBuilder.Entity<StatusMoto>()
                .Property(s => s.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<StatusMoto>().HasData(
                new StatusMoto { Id = 0, Descricao = "INATIVA" },
                new StatusMoto { Id = 1, Descricao = "ATIVA" },
                new StatusMoto { Id = 2, Descricao = "EM MANUTENÇAO" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
