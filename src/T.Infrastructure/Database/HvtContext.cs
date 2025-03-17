using System.Reflection;
using T.Domain.Common;

namespace T.Infrastructure.Database;

public class HvtContext : DbContext {
    public HvtContext() {
    }

    public HvtContext(string connectionString) : base(GetOptions(connectionString)) {
    }

    public HvtContext(DbContextOptions<HvtContext> options) : base(options) {
    }

    public DbSet<Merchant> Merchants => Set<Merchant>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RoleAction> RoleActions => Set<RoleAction>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseNpgsql(
                "User ID=hota;Password=123456789x@X;Server=db.hvantoan.io.vn;Port=5432;Database=hota;Pooling=true;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.HasDefaultSchema(DrSchema.Default);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private static DbContextOptions GetOptions(string connectionString) {
        return new DbContextOptionsBuilder().UseNpgsql(connectionString).Options;
    }
}
