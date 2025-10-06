using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using API.Domain;

namespace API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<BienChe> BienChes { get; set; }
    public DbSet<Khoi> Khois { get; set; }
    public DbSet<LinhVuc> LinhVucs { get; set; }
    public DbSet<Test> Tests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BienChe>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.TenDonVi).IsRequired().HasMaxLength(200);

    entity.HasOne(e => e.Khoi)
          .WithMany(k => k.BienChes)
          .HasForeignKey(e => e.KhoiId)
          .OnDelete(DeleteBehavior.Restrict);
});

        modelBuilder.Entity<Khoi>(entity =>
{
    entity.HasKey(e => e.KhoiId);

    entity.HasOne(k => k.LinhVuc)
          .WithMany(lv => lv.Khois)  // ❌ sửa thành Khois
          .HasForeignKey(k => k.LinhVucId)
          .OnDelete(DeleteBehavior.Restrict);
});

        modelBuilder.Entity<LinhVuc>(entity =>
 {
     entity.HasKey(e => e.LinhVucId);
     // LinhVuc có nhiều Khoi
     entity.HasMany(lv => lv.Khois)
           .WithOne(k => k.LinhVuc)
           .HasForeignKey(k => k.LinhVucId)
           .OnDelete(DeleteBehavior.Restrict);
 });


        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.StockQuantity)
                .HasDefaultValue(0);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });
        modelBuilder.Entity<Test>(entity =>
          {
              entity.HasKey(e => e.Id);
              entity.Property(e => e.Id).ValueGeneratedOnAdd();

              entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100);

          });
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName)
                .HasMaxLength(50);

            entity.Property(e => e.LastName)
                .HasMaxLength(50);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Code)
                .IsRequired();
            entity.HasIndex(e => e.Code).IsUnique();

            entity.Property(e => e.Name)
                .IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();

            entity.Property(e => e.NameCode)
                .IsRequired();
            entity.HasIndex(e => e.NameCode).IsUnique();

            entity.Property(e => e.TaxCode)
                .IsRequired();
            entity.HasIndex(e => e.TaxCode).IsUnique();
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            // Configure relationship with Company
            entity.HasOne(e => e.Company)
                .WithMany(e => e.Contacts)
                .HasForeignKey("CompanyId")
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}