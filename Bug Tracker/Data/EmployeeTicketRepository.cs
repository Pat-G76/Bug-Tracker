using Bug_Tracker.Models;

namespace Bug_Tracker.Data
{
	public class EmployeeTicketRepository : RepositoryBase<EmployeeTicket>, IEmployeeTicketRepository
	{

		public EmployeeTicketRepository(AppDbContext dbContext)
										: base (dbContext)
		{

		}

	}
}
