using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SORAPC_API.Models;

public partial class SorapcContext : DbContext
{
    public SorapcContext()
    {
    }

    public SorapcContext(DbContextOptions<SorapcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderPosition> OrderPositions { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductStatus> ProductStatuses { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ZESHALONDRAG\\SQLEXPRESS;Initial Catalog=sorapc;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.IdCart).HasName("PK__Cart__72140ECFE15410D4");

            entity.ToTable("Cart");

            entity.Property(e => e.IdCart).HasColumnName("ID_Cart");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.UsersId).HasColumnName("Users_ID");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__Orders__EC9FA95539954410");

            entity.HasIndex(e => e.OrderNumber, "UQ__Orders__67C7B3CB73F9C2D9").IsUnique();

            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Order_Date");
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Order_Number");
            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatus_ID");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Amount");
            entity.Property(e => e.UsersId).HasColumnName("Users_ID");
        });

        modelBuilder.Entity<OrderPosition>(entity =>
        {
            entity.HasKey(e => e.IdOrderPosition).HasName("PK__OrderPos__FD3AA9D60E8FF866");

            entity.ToTable("OrderPosition");

            entity.Property(e => e.IdOrderPosition).HasColumnName("ID_OrderPosition");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.IdOrderStatus).HasName("PK__OrderSta__36EC88CFD00A69D1");

            entity.ToTable("OrderStatus");

            entity.HasIndex(e => e.Title, "UQ__OrderSta__2CB664DC058289A3").IsUnique();

            entity.Property(e => e.IdOrderStatus).HasColumnName("ID_OrderStatus");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Products__522DE49650C37957");

            entity.HasIndex(e => e.Names, "UQ__Products__44C03486A0F0E0AF").IsUnique();

            entity.Property(e => e.IdProduct).HasColumnName("ID_Product");
            entity.Property(e => e.Descriptions).IsUnicode(false);
            entity.Property(e => e.Img).IsUnicode(false);
            entity.Property(e => e.Names)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategory_ID");
            entity.Property(e => e.ProductStatusId).HasColumnName("ProductStatus_ID");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.IdProductCategory).HasName("PK__ProductC__8FAD631E121D9011");

            entity.ToTable("ProductCategory");

            entity.HasIndex(e => e.Title, "UQ__ProductC__2CB664DC845B774E").IsUnique();

            entity.Property(e => e.IdProductCategory).HasColumnName("ID_ProductCategory");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductStatus>(entity =>
        {
            entity.HasKey(e => e.IdProductStatus).HasName("PK__ProductS__92EB9BF66EBF6C8C");

            entity.ToTable("ProductStatus");

            entity.HasIndex(e => e.Title, "UQ__ProductS__2CB664DC9A323B5C").IsUnique();

            entity.Property(e => e.IdProductStatus).HasColumnName("ID_ProductStatus");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.IdReview).HasName("PK__Reviews__E39E9647A32FE0A9");

            entity.Property(e => e.IdReview).HasColumnName("ID_Review");
            entity.Property(e => e.Comment).IsUnicode(false);
            entity.Property(e => e.Cons).IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.Pros).IsUnicode(false);
            entity.Property(e => e.UsersId).HasColumnName("Users_ID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Roles__43DCD32DA073D94C");

            entity.HasIndex(e => e.Title, "UQ__Roles__2CB664DCC4D7D3EF").IsUnique();

            entity.Property(e => e.IdRole).HasColumnName("ID_Role");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUsers).HasName("PK__Users__B97FFDA1F5F71808");

            entity.HasIndex(e => e.Phone, "UQ__Users__5C7E359E325FEF82").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A5C1EB6B").IsUnique();

            entity.HasIndex(e => e.Logins, "UQ__Users__D00D0632964FBDEB").IsUnique();

            entity.Property(e => e.IdUsers).HasColumnName("ID_Users");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Logins)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Passwords).IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.UserMiddlename)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserSurname)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}