using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PetsHouseServer.Models
{
    public partial class PetsHouseDatabaseContext : DbContext
    {
        public PetsHouseDatabaseContext()
        {
        }

        public PetsHouseDatabaseContext(DbContextOptions<PetsHouseDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdType> AdTypes { get; set; } = null!;
        public virtual DbSet<Advertisment> Advertisments { get; set; } = null!;
        public virtual DbSet<PetType> PetTypes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=PetsHouseDatabase;Username=admin;Password=a");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdType>(entity =>
            {
                entity.ToTable("AdType");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.AdTypeName).HasColumnType("character varying");
            });

            modelBuilder.Entity<Advertisment>(entity =>
            {
                entity.ToTable("Advertisment");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.AdTypeId).HasDefaultValueSql("3");

                entity.Property(e => e.City).HasColumnType("character varying");

                entity.Property(e => e.Description).HasColumnType("character varying");

                entity.Property(e => e.PetTypeId).HasDefaultValueSql("3");

                entity.Property(e => e.Phone).HasColumnType("character varying");

                entity.Property(e => e.Photo).HasColumnType("character varying");

                entity.HasOne(d => d.AdType)
                    .WithMany(p => p.Advertisments)
                    .HasForeignKey(d => d.AdTypeId)
                    .HasConstraintName("AdType");

                entity.HasOne(d => d.PetType)
                    .WithMany(p => p.Advertisments)
                    .HasForeignKey(d => d.PetTypeId)
                    .HasConstraintName("AdPetType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Advertisments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("AdUser");
            });

            modelBuilder.Entity<PetType>(entity =>
            {
                entity.ToTable("PetType");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.TypeName).HasColumnType("character varying");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasColumnType("character varying");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Email).HasColumnType("character varying");

                entity.Property(e => e.FirstName).HasColumnType("character varying");

                entity.Property(e => e.LastName).HasColumnType("character varying");

                entity.Property(e => e.Password).HasColumnType("character varying");

                entity.Property(e => e.RoleId).HasDefaultValueSql("1");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("UserRole");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
