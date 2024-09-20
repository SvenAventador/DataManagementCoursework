using Microsoft.EntityFrameworkCore;

namespace ShopsGoodsLibrary.Models;

public partial class ShopsGoodsContext : DbContext
{
    private static ShopsGoodsContext _context = new();
    public ShopsGoodsContext()
    {
    }

    public ShopsGoodsContext(DbContextOptions<ShopsGoodsContext> options)
        : base(options)
    {
    }

    public static ShopsGoodsContext GetContext() => _context ??= new ShopsGoodsContext();

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartGood> CartGoods { get; set; }

    public virtual DbSet<Good> Goods { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderGood> OrderGoods { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<ShopGood> ShopGoods { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-SIMH2QH;Database=ShopsGoods;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brand__3214EC07B1110EB9");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandName).HasMaxLength(255);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC070151C5B5");

            entity.ToTable("Cart");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_CART_USER");
        });

        modelBuilder.Entity<CartGood>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartGood__3214EC07FDB818F3");

            entity.ToTable("CartGood");

            entity.Property(e => e.GoodAmount).HasDefaultValue(1);

            entity.HasOne(d => d.Cart).WithMany(p => p.CartGoods)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_CART_GOOD_CART");

            entity.HasOne(d => d.Good).WithMany(p => p.CartGoods)
                .HasForeignKey(d => d.GoodId)
                .HasConstraintName("FK_CART_GOOD_GOOD");
        });

        modelBuilder.Entity<Good>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Good__3214EC07076D3138");

            entity.ToTable("Good");

            entity.Property(e => e.DescriptionGood).HasColumnType("text");
            entity.Property(e => e.GoodImage).HasColumnType("text");
            entity.Property(e => e.NameGood).HasMaxLength(255);

            entity.HasOne(d => d.Brand).WithMany(p => p.Goods)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_GOOD_BRAND");

            entity.HasOne(d => d.Type).WithMany(p => p.Goods)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_GOOD_TYPE");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC075D56D959");

            entity.ToTable("Order");

            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .HasConstraintName("FK_ORDER_ORDER_STATUS");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ORDER_USER");
        });

        modelBuilder.Entity<OrderGood>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderGoo__3214EC07B6F64091");

            entity.ToTable("OrderGood");

            entity.HasOne(d => d.Good).WithMany(p => p.OrderGoods)
                .HasForeignKey(d => d.GoodId)
                .HasConstraintName("FK_ORDER_GOOD_GOOD");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderGoods)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_ORDER_GOOD_ORDER");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderSta__3214EC0761A9C89F");

            entity.ToTable("OrderStatus");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shop__3214EC07F991EF47");

            entity.ToTable("Shop");

            entity.Property(e => e.ShopAddress).HasMaxLength(255);
            entity.Property(e => e.ShopName).HasMaxLength(255);
            entity.Property(e => e.ShopPhone).HasMaxLength(30);
        });

        modelBuilder.Entity<ShopGood>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ShopGood__3214EC07A749663E");

            entity.ToTable("ShopGood");

            entity.HasOne(d => d.Good).WithMany(p => p.ShopGoods)
                .HasForeignKey(d => d.GoodId)
                .HasConstraintName("FK_SHOP_GOOD_GOOD");

            entity.HasOne(d => d.Shop).WithMany(p => p.ShopGoods)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK_SHOP_GOOD_SHOP");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Type__3214EC07239E248A");

            entity.ToTable("Type");

            entity.Property(e => e.TypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0769296E86");

            entity.ToTable("User");

            entity.Property(e => e.UserEmail).HasMaxLength(255);
            entity.Property(e => e.UserFio).HasMaxLength(255);
            entity.Property(e => e.UserPhone).HasMaxLength(30);
            entity.Property(e => e.UserRole)
                .HasMaxLength(20)
                .HasDefaultValue("Пользователь");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
