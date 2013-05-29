using System.Data.Entity;

namespace TasksApp.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}