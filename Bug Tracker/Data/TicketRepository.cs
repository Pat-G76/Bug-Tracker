using Bug_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Bug_Tracker.Data
{
    public class TicketRepository : RepositoryBase<Ticket>, ITicketRepository
    {
        public TicketRepository(AppDbContext dbContext) 
                                : base (dbContext)
        {

        }

        public IEnumerable<Ticket> GetTicketsForProject(int projectId)
        {
            return dbContext.Tickets.Where(c => c.ProjectID == projectId);
        }

		public IEnumerable<Ticket> GetTicketsForEmployee(string id)
		{
            return dbContext.Tickets.Include(c => c.EmployeeTickets).ThenInclude(te => te.EmployeeId == id);
		}

	}
}
