using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace VeterinarskaStanica.Model.Core
{
    public partial class DataBaseConnection : DbContext
    {
        public DataBaseConnection()
        {
        }

        public DataBaseConnection(DbContextOptions<DataBaseConnection> options)
            : base(options)
        {
        }

        public virtual DbSet<PageSetting> PageSettings { get; set; }
        public virtual DbSet<Pet> Pets { get; set; }
        public virtual DbSet<PetType> PetTypes { get; set; }
        public virtual DbSet<RecordStatus> RecordStatuses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VisitRecord> VisitRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PageSetting>(entity =>
            {
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PageSettings)
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_User_PageSettings");
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("Pet");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.PetType)
                    .WithMany(p => p.Pets)
                    .HasForeignKey(x => x.PetTypeId)
                    .HasConstraintName("FK_Pet_PetType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Pets)
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_Pet_User");
            });

            modelBuilder.Entity<PetType>(entity =>
            {
                entity.ToTable("PetType");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<RecordStatus>(entity =>
            {
                entity.ToTable("RecordStatus");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(255);

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(x => x.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<VisitRecord>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.VisitRecords)
                    .HasForeignKey(x => x.EmployeeId)
                    .HasConstraintName("FK_VisitRecords_Employee");

                entity.HasOne(d => d.Pet)
                    .WithMany(p => p.VisitRecords)
                    .HasForeignKey(x => x.PetId)
                    .HasConstraintName("FK_VisitRecords_Pet");

                entity.HasOne(d => d.RecordStatus)
                    .WithMany(p => p.VisitRecords)
                    .HasForeignKey(x => x.RecordStatusId)
                    .HasConstraintName("FK_VisitRecords_RecordStatusId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
