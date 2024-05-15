using System.Reflection;
using DR.Domain.Entities;
using DR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DR.Infrastructure.Database {

    public partial class DrContext : DbContext {
        public DbSet<Attachment> Attachments => Set<Attachment>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        public DbSet<User> Users => Set<User>();


        public DrContext() {
        }

        public DrContext(string connectionString) : base(GetOptions(connectionString)) {
        }

        public DrContext(DbContextOptions<DrContext> options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseNpgsql("User ID=postgres;Password=12345678x@X;Server=localhost;Port=5432;Database=Doran;Integrated Security=false;Pooling=true;");
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
}
