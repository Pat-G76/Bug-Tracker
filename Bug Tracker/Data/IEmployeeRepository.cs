using Bug_Tracker.Data.DataAccess;
using Bug_Tracker.Models;
using System.Linq.Expressions;

namespace Bug_Tracker.Data
{
    public interface IEmployeeRepository 
    {
        Employee GetEmployeeWithProjects(string id);
		Task<Employee> GetEmployeeByComment(int commentID);
		IEnumerable<Employee> GetAllItems();
		
		Task<IEnumerable<Employee>> GetWithOptions(QueryOptions<Employee> options);
	}
}
