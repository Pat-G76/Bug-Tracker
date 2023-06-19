using Bug_Tracker.Models;

namespace Bug_Tracker.Data
{
    public interface IProjectRepository : IRepositoryBase<Project>
    {
		IEnumerable<Project> GetProjectsWithEmployees();

		Project GetProjectForTicket(int ticketID);

	}
}
