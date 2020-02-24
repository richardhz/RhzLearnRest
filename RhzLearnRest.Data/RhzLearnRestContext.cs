using Microsoft.EntityFrameworkCore;
using RhzLearnRest.Domains.Models;

namespace RhzLearnRest.Data
{
    public class RhzLearnRestContext : DbContext
    {
        public RhzLearnRestContext(DbContextOptions<RhzLearnRestContext> options) :base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
    }
}
