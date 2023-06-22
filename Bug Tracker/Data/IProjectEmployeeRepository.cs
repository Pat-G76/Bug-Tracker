using Bug_Tracker.Models;
using ContosoUniversity.Models;

namespace Bug_Tracker.Data
{
	public interface IProjectEmployeeRepository : IRepositoryBase<ProjectEmployee>
	{
		IEnumerable<Project> ProjectsForEmployee(string id);
		IEnumerable<ProjectEmployee> EmployeesForProject(int projectId);
		ProjectEmployee GetProjectEmployee(int ProjectID, string EmployeeID);
	}
}
