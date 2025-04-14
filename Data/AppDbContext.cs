using Microsoft.EntityFrameworkCore;

namespace TgParser.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<RawNewsModel> RawNews { get; set; }


        // В DbContext
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RawNewsModel>()
                .Property(e => e.Text)
                .HasColumnType("text")
                .IsUnicode(true);
        }

    }

}
