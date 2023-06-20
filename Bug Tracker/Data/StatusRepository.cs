using Bug_Tracker.Models;

namespace Bug_Tracker.Data
{
	public class StatusRepository : RepositoryBase<Status>, IStatusRepository
	{

		public StatusRepository(AppDbContext dbContext) 
								: base(dbContext)
		{

		}

		public Status GetByTitle(string statusTitle)
		{
			return dbContext.Statuses.FirstOrDefault(s => s.StatusTitle.ToLower() == statusTitle.ToLower());
		}

	}
}
