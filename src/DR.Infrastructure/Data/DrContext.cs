using System.Reflection;
using DR.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DR.Infrastructure.Data {

    public class DrContext : DbContext {
        public DbSet<NotificationToken> NotificationTokens => Set<NotificationToken>();

        public DrContext() {
        }

        public DrContext(string connectionString) : base(GetOptions(connectionString)) {
        }

        public DrContext(DbContextOptions<DrContext> options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseNpgsql("User ID=tuanvudev;Password=tuanvudev;Server=db.tvfersfc.com;Port=21427;Database=tuanvu_dev;Integrated Security=true;Pooling=true;");
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
