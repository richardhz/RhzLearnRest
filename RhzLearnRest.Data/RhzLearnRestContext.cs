using Microsoft.EntityFrameworkCore;
using RhzLearnRest.Domains.Models;

namespace RhzLearnRest.Data
{
    public class RhzLearnRestContext : DbContext
    {
        public RhzLearnRestContext(DbContextOptions<RhzLearnRestContext> options) :base(options)
        {
            // In this project we are using change tracking.
            // we are using autoMapper to change/mutate structers so when we want to 
            // update we don't have to implement an update method because change tracking spots the 
            // changes and will update on the save.
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
    }
}
