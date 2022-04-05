using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecordBox.Models
{
  public class RecordBoxContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Record> Records { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<RecordGenre> RecordGenres { get; set; }
    public RecordBoxContext(DbContextOptions options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}