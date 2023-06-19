using Bug_Tracker.Data;
using Bug_Tracker.Data.DataAccess;
using Bug_Tracker.Models;
using Bug_Tracker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bug_Tracker.Controllers
{
	[Authorize(Roles = "administrator, developer")]
	public class ProjectController : Controller
	{
		public readonly IWrapperRepository wrapper;
		public readonly UserManager<Employee> userManager;

		private readonly int PageSize = 6;

		public ProjectController(IWrapperRepository _wrapper,
								 UserManager<Employee> _userManager)								
		{
			wrapper = _wrapper;
			userManager = _userManager;
		}

		public async Task<IActionResult> Index(string searchString = "", int page = 1, string sortBy = "TimeCreated")
		{

			string orderByDirection = "";
			Expression<Func<Project, object>> orderBy;
			Expression<Func<Project, bool>> Where;
			IEnumerable<Project> projects;

			ViewBag.TimeSort = sortBy == "TimeCreated" ? "TimeCreated_desc" : "TimeCreated";
			ViewBag.NameSort = sortBy == "ProjectName" ? "ProjectName_desc" : "ProjectName";
			ViewBag.searchString = searchString;
			ViewBag.Filter = sortBy;

			if(sortBy.EndsWith("_desc"))
			{
				int startIndex = sortBy.LastIndexOf("_desc");

				sortBy = sortBy.Remove(startIndex);

				orderByDirection = "desc";
			}
			else
			{
				orderByDirection = "asc";
			}

			orderBy = p => EF.Property<object>(p, sortBy);


			Employee employee = await userManager.FindByNameAsync(User.Identity.Name);

			int iTotalItems = 0;

			if(!string.IsNullOrEmpty(searchString) && User.IsInRole("administrator"))
			{
				iTotalItems = wrapper.Project.GetAllItems().Where(p => p.ProjectName.ToLower().Contains(searchString.ToLower())).Count();

				projects = wrapper.Project.GetWithOptions(new QueryOptions<Project>()
				{
					Where = p => p.ProjectName.ToLower().Contains(searchString.ToLower()),
					OrderBy = orderBy,
					PageSize = PageSize,
					OrderByDirection = orderByDirection,
					PageNumber = page
				});
			}
			else if (string.IsNullOrEmpty(searchString) && User.IsInRole("administrator"))
			{
				iTotalItems = wrapper.Project.GetAllItems().Count();

				projects = wrapper.Project.GetWithOptions(new QueryOptions<Project>()
				{					
					OrderBy = orderBy,
					PageSize = PageSize,
					OrderByDirection = orderByDirection,
					PageNumber = page
				});
			}
			else if(!string.IsNullOrEmpty(searchString))
			{
				var lstID = wrapper.ProjectEmployee.GetAllItems().Where(pe => pe.EmployeeId == employee.Id).Select(pe => pe.ProjectID);

				iTotalItems = wrapper.Project.GetAllItems().Where(p => p.ProjectName.ToLower().Contains(searchString.ToLower()) && lstID.Contains(p.ProjectID)).Count();

				projects = wrapper.Project.GetWithOptions(new QueryOptions<Project>()
				{
					Where = p => p.ProjectName.ToLower().Contains(searchString.ToLower()) && lstID.Contains(p.ProjectID),
					OrderBy = orderBy,
					PageSize = PageSize,
					OrderByDirection = orderByDirection,
					PageNumber = page
				});
			}
			else
			{
				var lstID = wrapper.ProjectEmployee.GetAllItems().Where(pe => pe.EmployeeId == employee.Id).Select(pe => pe.ProjectID);

				iTotalItems = wrapper.Project.GetAllItems().Where(p => lstID.Contains(p.ProjectID)).Count();

				projects = wrapper.Project.GetWithOptions(new QueryOptions<Project>()
				{
					Where = p => lstID.Contains(p.ProjectID),
					OrderBy = orderBy,
					PageSize = PageSize,
					OrderByDirection = orderByDirection,
					PageNumber = page
				});
			}

			

			//if (!User.IsInRole("administrator"))
			//{


			//	var lstID = wrapper.ProjectEmployee.GetAllItems().Where(pe => pe.EmployeeId == employee.Id).Select(pe => pe.ProjectID);

			//	projects = projects.Where(p => lstID.Contains(p.ProjectID));
			//}


			var pageInfo = new PageInformation()
			{
				CurrentPage = page,
				ItemsPerPage = PageSize,
				TotalItems = iTotalItems
			};



			return View(new ProjectList() { pageInfo = pageInfo, projects = projects});
		}

		[Authorize(Roles = "administrator")]
		public IActionResult CreateProject()
		{

			Project project = new Project();

			ViewBag.Request = "Create";

			return View(project);
		}


		[Authorize(Roles = "administrator")]
		public IActionResult UpdateProject(int id)
		{

			Project project = wrapper.Project.GetById(id);

			if(project is null)
			{
				return NotFound();
			}

			ViewBag.Request = "Update";

			return View("CreateProject", project);
		}

		[Authorize(Roles = "administrator")]
		[HttpPost, ActionName("CreateProject")]
		public IActionResult ProjectUpdateCreate(Project project)
		{

			ViewBag.Request = wrapper.Project.GetById(project.ProjectID) == null ? "Create" : "Update";

			if (ModelState.IsValid)
			{
				try
				{
					if(project.ProjectID == 0)
					{

						project.TimeCreated = DateTime.Now;

						wrapper.Project.Add(project);

						wrapper.saveChanges();

					}
					else
					{

						Project oldProject = wrapper.Project.GetById(project.ProjectID);

						oldProject.ProjectName = project.ProjectName;
						oldProject.ProjectDescription = project.ProjectDescription;

						wrapper.Project.Update(oldProject);

						wrapper.saveChanges();

					}

					return RedirectToAction("Index", "ProjectEmployee", project.ProjectID);

				}
				catch
				{
					ModelState.AddModelError("", $"Failed to {ViewBag.Request} a project. Try again.");
				}



			}


			return View(project);

		}

	}
}
