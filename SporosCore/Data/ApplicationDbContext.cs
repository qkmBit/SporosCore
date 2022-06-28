using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SporosCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SporosCore.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<GrowthType> GrowthType { get; set; }
        public virtual DbSet<ItemAdvantages> ItemAdvantages { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<MaturationGroup> MaturationGroup { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartItems> CartItems { get; set; }
        public virtual DbSet<AdditionalOption> AdditionalOption { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.CartId).ValueGeneratedOnAdd();

                entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(100);

                entity.HasOne(d => d.Users)
                .WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_AspNetUsers");
            });
            modelBuilder.Entity<AdditionalOption>(entity =>
            {
                entity.Property(e => e.AdditionalOptionId).ValueGeneratedOnAdd();

                entity.HasKey(e => e.AdditionalOptionId);
            });
            modelBuilder.Entity<CartItems>(entity =>
            {
                entity.Property(e => e.CartId).ValueGeneratedOnAdd();

                entity.HasKey(e => new { e.ItemId, e.CartId });

                entity.HasOne(d => d.Cart)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Cart");
                entity.HasOne(d => d.Items)
                .WithMany(p => p.CartItems)
                .HasForeignKey(p => p.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Items");
            });
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.AddressId).ValueGeneratedOnAdd();

                entity.Property(e => e.Address1).HasColumnName("Address");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(100);

                entity.HasOne(d => d.Users)
                .WithMany(p => p.Address)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Address_Users");
            });


            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).ValueGeneratedNever();
            });


            modelBuilder.Entity<GrowthType>(entity =>
            {
                entity.Property(e => e.GrowthTypeId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ItemAdvantages>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.Advantage });

                entity.Property(e => e.Advantage).HasMaxLength(256);

                entity.HasOne(d => d.Items)
                .WithMany(p => p.ItemAdvantages)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemAdvantages_Items");
            });

            modelBuilder.Entity<Items>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.Property(e => e.ItemId).ValueGeneratedNever();

                entity.HasOne(d => d.Category)
                .WithMany(p => p.Items)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Items_Category");

                entity.HasOne(d => d.GrowthType)
                .WithMany(p => p.Items)
                .HasForeignKey(d => d.GrowthTypeId)
                .HasConstraintName("FK_Items_GrowthType");

                entity.HasOne(d => d.MaturationGroup)
                .WithMany(p => p.Items)
                .HasForeignKey(d => d.MaturationGroupId)
                .HasConstraintName("FK_Items_MaturationGroup");
            });

            modelBuilder.Entity<MaturationGroup>(entity =>
            {
                entity.Property(e => e.MaturationGroupId).ValueGeneratedNever();
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.NewsId).ValueGeneratedNever();

                entity.Property(e => e.NewsDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.OrderId });

                entity.HasOne(d => d.Items)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Items");



                entity.HasOne(d => d.AdditionalOption)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.AdditionalOptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Orders");

                entity.HasOne(d => d.Orders)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_AdditionalOption");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId).ValueGeneratedOnAdd();

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(100);

                entity.HasOne(d => d.User)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
            });


            modelBuilder.Entity<Users>(entity =>
            {

                entity.Property(e => e.ConcurrencyStamp).HasMaxLength(256);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.PasswordHash).IsRequired();


                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
