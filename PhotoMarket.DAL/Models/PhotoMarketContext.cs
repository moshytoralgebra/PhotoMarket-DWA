using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PhotoMarket.DAL.Models;

public partial class PhotoMarketContext : DbContext
{
    public PhotoMarketContext()
    {
    }

    public PhotoMarketContext(DbContextOptions<PhotoMarketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<Download> Downloads { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<PhotoTag> PhotoTags { get; set; }

    public virtual DbSet<Photographer> Photographers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("server=.;Database=PhotoMarket;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AppUser__3214EC079DF92A91");

            entity.ToTable("AppUser");

            entity.HasIndex(e => e.Email, "UQ__AppUser__A9D10534AAD970C6").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__AppUser__C9F28456E1100A05").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUser_Role");
        });

        modelBuilder.Entity<Download>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Download__3214EC072A2A16B9");

            entity.ToTable("Download");

            entity.HasIndex(e => e.PhotoId, "IX_Download_PhotoId");

            entity.HasIndex(e => e.UserId, "IX_Download_UserId");

            entity.HasOne(d => d.Photo).WithMany(p => p.Downloads)
                .HasForeignKey(d => d.PhotoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Download_Photo");

            entity.HasOne(d => d.User).WithMany(p => p.Downloads)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Download_AppUser");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3214EC07A678D5D2");

            entity.ToTable("Log");

            entity.HasIndex(e => e.Timestamp, "IX_Log_Timestamp");

            entity.Property(e => e.Level).HasMaxLength(50);
            entity.Property(e => e.Message).HasMaxLength(1000);
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photo__3214EC07DAA54EE5");

            entity.ToTable("Photo");

            entity.HasIndex(e => e.Name, "IX_Photo_Name");

            entity.HasIndex(e => e.PhotographerId, "IX_Photo_PhotographerId");

            entity.HasIndex(e => e.Name, "UQ__Photo__737584F6D5E33B1A").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(150);

            entity.HasOne(d => d.Photographer).WithMany(p => p.Photos)
                .HasForeignKey(d => d.PhotographerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Photo_Photographer");
        });

        modelBuilder.Entity<PhotoTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhotoTag__3214EC07522AF793");

            entity.ToTable("PhotoTag");

            entity.HasIndex(e => new { e.PhotoId, e.TagId }, "UQ_PhotoTag").IsUnique();

            entity.HasOne(d => d.Photo).WithMany(p => p.PhotoTags)
                .HasForeignKey(d => d.PhotoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhotoTag_Photo");

            entity.HasOne(d => d.Tag).WithMany(p => p.PhotoTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhotoTag_Tag");
        });

        modelBuilder.Entity<Photographer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photogra__3214EC07E214BCA1");

            entity.ToTable("Photographer");

            entity.HasIndex(e => e.Name, "UQ__Photogra__737584F6074F77C9").IsUnique();

            entity.Property(e => e.Bio).HasMaxLength(1000);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Website).HasMaxLength(500);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC077F49C74C");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "UQ__Role__737584F6145F710C").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC079639D5F9");

            entity.ToTable("Tag");

            entity.HasIndex(e => e.Name, "IX_Tag_Name");

            entity.HasIndex(e => e.Name, "UQ__Tag__737584F6182942D2").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
