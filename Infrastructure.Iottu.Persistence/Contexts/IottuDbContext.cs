using Core.Iottu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

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
        public DbSet<User> Users { get; set; } = null!;

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

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Id)
                      .HasColumnType("RAW(16)")
                      .ValueGeneratedNever();

                entity.Property(u => u.Username)
                      .HasColumnType("NVARCHAR2(100)");

                entity.Property(u => u.PasswordHash)
                      .HasColumnType("NVARCHAR2(255)");

                entity.Property(u => u.Role)
                      .HasColumnType("NVARCHAR2(100)");


                entity.Property(u => u.IsActive)
                      .HasConversion<int>()
                      .HasColumnType("NUMBER(1)");

                entity.Property(u => u.CreatedAt)
                      .HasColumnType("TIMESTAMP(7)");
            });

            // Seed admin user
            var adminId = new Guid("4e9c2888-72e4-4659-86fc-494a4c214a27");
            var adminHash = HashPassword("admin123");

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminId,
                Username = "admin",
                PasswordHash = adminHash,
                Role = "admin",
                IsActive = true,
                CreatedAt = new DateTime(2025, 11, 5, 17, 29, 30, 20, DateTimeKind.Utc)
            });

            base.OnModelCreating(modelBuilder);
        }

        private static string HashPassword(string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes("IottuStaticSalt");
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));
            return hash;
        }
    }
}
