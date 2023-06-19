using Bug_Tracker.Models;

namespace Bug_Tracker.Data
{
	public class StatusRepository : RepositoryBase<Status>, IStatusRepository
	{

		public StatusRepository(AppDbContext dbContext) 
								: base(dbContext)
		{
		}

	}
}
