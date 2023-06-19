using Bug_Tracker.Data.DataAccess;
using Bug_Tracker.Models;
using System.Linq.Expressions;

namespace Bug_Tracker.Data
{
    public interface IEmployeeRepository 
    {
        Employee GetEmployeeWithProjects(string id);
        IEnumerable<Employee> GetEmployeeWithTickets(string id);
		IEnumerable<Employee> GetAllItems();
		//IEnumerable<Employee> FindByCondition(Expression<Func<Employee, bool>> expression);
		//Task<IEnumerable<Employee>> SortByRole(string roleName);

		Task<IEnumerable<Employee>> GetWithOptions(QueryOptions<Employee> options);
	}
}
