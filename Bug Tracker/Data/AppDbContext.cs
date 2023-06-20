using Bug_Tracker.Models;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bug_Tracker.Data
{
    public class AppDbContext : IdentityDbContext<Employee>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        //public DbSet<EmployeeRepository> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
		public DbSet<Status> Statuses { get; set; }
		public DbSet<Priority> Priorities { get; set; }
		public DbSet<IssueType> IssueTypes { get; set; }
		public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>().ToTable("Comment");
			//modelBuilder.Entity<Employee>().ToTable("Employee");
			modelBuilder.Entity<Status>().ToTable("Status");
			modelBuilder.Entity<IssueType>().ToTable("IssueType");
			modelBuilder.Entity<Priority>().ToTable("Priority");
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<ProjectEmployee>().ToTable("ProjectEmployee");
            modelBuilder.Entity<Ticket>().ToTable("Ticket");

        }

    }
}
