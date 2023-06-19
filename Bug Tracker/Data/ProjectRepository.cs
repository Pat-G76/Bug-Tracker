using Bug_Tracker.Models;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace Bug_Tracker.Data
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {

        public ProjectRepository(AppDbContext dbContext)
                                : base(dbContext)
        {

        } 

        public IEnumerable<Project> GetProjectsWithEmployees()
        {
            return dbContext.Projects.Include(p => p.ProjectEmployees).
                ThenInclude(pe => pe.Employee);
        }

		public Project GetProjectForTicket(int ticketID)
        {
            Ticket ticket = dbContext.Tickets.First(t => t.TicketID == ticketID);

            return dbContext.Projects.First(p => p.ProjectID == ticket.ProjectID);
        }

	}
}
