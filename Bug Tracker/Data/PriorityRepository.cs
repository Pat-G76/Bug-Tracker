using Bug_Tracker.Models;

namespace Bug_Tracker.Data
{
	public class PriorityRepository : RepositoryBase<Priority>, IPriorityRepository
	{

		public PriorityRepository(AppDbContext dbContext)
								  : base(dbContext)
		{

		}

	}
}
