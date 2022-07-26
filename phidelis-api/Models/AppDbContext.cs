using Microsoft.EntityFrameworkCore;

namespace phidelis_api.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Student>().HasKey(m => m.Registration);
            builder.Entity<Student>().Property(f => f.Registration).ValueGeneratedOnAdd();
            base.OnModelCreating(builder);
        }
    }
}
