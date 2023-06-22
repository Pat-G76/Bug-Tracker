using Bug_Tracker.Models;
using ContosoUniversity.Models;

namespace Bug_Tracker.Data
{
	public class ProjectEmployeeRepository : RepositoryBase<ProjectEmployee>, IProjectEmployeeRepository
	{

		public ProjectEmployeeRepository(AppDbContext dbContext)
										: base (dbContext)
		{

		}

		public IEnumerable<Project> ProjectsForEmployee(string id)
		{
			return dbContext.ProjectEmployees.Where(pe => pe.EmployeeId == id).Select(pe => pe.Project);
		}

		public IEnumerable<ProjectEmployee> EmployeesForProject(int projectId)
		{


			return dbContext.ProjectEmployees.Where(pe => pe.ProjectID == projectId);

		}

		public ProjectEmployee GetProjectEmployee(int ProjectID, string EmployeeID)
		{

			return dbContext.ProjectEmployees.FirstOrDefault(pe => pe.ProjectID == ProjectID && pe.EmployeeId == EmployeeID);

		}

	}
}
