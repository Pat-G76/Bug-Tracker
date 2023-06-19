using Bug_Tracker.Models;

namespace Bug_Tracker.Data
{
	public class IssueTypeRepository : RepositoryBase<IssueType>, IIssueTypeRepository
	{
		public IssueTypeRepository(AppDbContext _dbContext) : base(_dbContext)
		{
		}
	}
}
