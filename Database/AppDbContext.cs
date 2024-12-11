using Microsoft.EntityFrameworkCore;
using thesis_comicverse_webservice_api.Models;

namespace thesis_comicverse_webservice_api.Database
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public DbSet<Assign>? Assign { get; set; }
        public DbSet<Author>? Author { get; set; }
        //public DbSet<Categories>? Categories { get; set; }
        public DbSet<Comic>? Comic { get; set; }
        //public DbSet<ComicCategories>? ComicCategories { get; set; }
        //public DbSet<License>? License { get; set; }
        public DbSet<Publisher>? Publisher { get; set; }
        public DbSet<Releasing>? Releasing { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Models.Task>? Task { get; set; }



        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
