
namespace TaskTracker.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}

