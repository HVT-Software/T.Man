#region

using System.Reflection;
using T.Domain.Common;

#endregion

namespace T.Infrastructure.Database;

public class HvtContext : DbContext
{
    public HvtContext() { }

    public HvtContext(string connectionString)
        : base(GetOptions(connectionString)) { }

    public HvtContext(DbContextOptions<HvtContext> options)
        : base(options) { }

    public DbSet<Merchant>   Merchants   { get => Set<Merchant>(); }
    public DbSet<Role>       Roles       { get => Set<Role>(); }
    public DbSet<RoleAction> RoleActions { get => Set<RoleAction>(); }

    public DbSet<Category>    Categories   { get => Set<Category>(); }
    public DbSet<Transaction> Transactions { get => Set<Transaction>(); }

    public DbSet<User> Users { get => Set<User>(); }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("User ID=hota;Password=123456789x@X;Server=db.hvantoan.io.vn;Port=5432;Database=hota;Pooling=true;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DrSchema.Default);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private static DbContextOptions GetOptions(string connectionString)
    {
        return new DbContextOptionsBuilder().UseNpgsql(connectionString).Options;
    }
}
