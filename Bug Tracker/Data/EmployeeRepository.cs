using Bug_Tracker.Data.DataAccess;
using Bug_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Bug_Tracker.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext dbContext;
		private readonly UserManager<Employee> userManager;
        public EmployeeRepository(AppDbContext _dbContext,
								  UserManager<Employee> _userManager)							  

        {
            dbContext = _dbContext;
			userManager = _userManager;
		}

		public IEnumerable<Employee> GetAllItems()
		{
			return dbContext.Set<Employee>();
		}


		public async Task<IEnumerable<Employee>> GetWithOptions(QueryOptions<Employee> options)
		{
			IQueryable<Employee> items = dbContext.Set<Employee>();


			if (true)
			{

				List<Employee> members = new List<Employee>();
				List<Employee> nonMembers = new List<Employee>();

				foreach (Employee user in items)
				{

					var roles = await userManager.GetRolesAsync(user);


					if (options.OrderByRole == "")
					{
						if (roles.Count == 0)
						{
							members.Add(user);
						}
						else
						{
							nonMembers.Add(user);
						}

					}
					else
					{
						if (await userManager.IsInRoleAsync(user, options.OrderByRole))
						{
							members.Add(user);
						}
						else
						{
							nonMembers.Add(user);
						}
					}
				}

				var all = members.Union(nonMembers);

				items = all.AsQueryable();

			}

			if (options.HasWhere)
			{
				items = items.Where(options.Where);
			}

			if (options.HasOrderBy)
			{
				items = items.OrderBy(options.OrderBy);
			}

			if (options.HasPaging)
			{
				items = items.Skip((options.PageNumber - 1) * options.PageSize).Take(options.PageSize);
			}


			return items;
		}

		public Employee GetEmployeeWithProjects(string id)
        {
            return dbContext.Set<Employee>().Include(e => e.ProjectEmployees).ThenInclude(e => e.Project).First(e => e.Id == id);
		}

		public async Task<Employee> GetEmployeeByComment(int commentID)
		{

			Comment comment = dbContext.Comments.FirstOrDefault(c => c.CommentID == commentID);


			return await userManager.FindByIdAsync(comment.EmployeeId);

		}
	}
}
