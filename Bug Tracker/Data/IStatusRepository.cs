using Bug_Tracker.Models;

namespace Bug_Tracker.Data
{
	public interface IStatusRepository : IRepositoryBase<Status>
	{
		Status GetByTitle(string statusTitle);
	}
}
