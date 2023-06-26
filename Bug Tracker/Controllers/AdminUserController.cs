using Bug_Tracker.Data;
using Bug_Tracker.Data.DataAccess;
using Bug_Tracker.Infrastructure;
using Bug_Tracker.Models;
using Bug_Tracker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security;

namespace Bug_Tracker.Controllers
{
	[Authorize(Roles = "administrator")]
	public class AdminUserController : Controller
	{

		private readonly UserManager<Employee> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IPasswordValidator<Employee> passwordValidator;
		private readonly IUserValidator<Employee> userValidator;
		public readonly IWrapperRepository wrapper;
		private readonly IPasswordHasher<Employee> passwordHasher;


		private readonly int pageSize = 6;

		[TempData]
		private string ResponseMessage { get; set; }

        public AdminUserController( UserManager<Employee> _userManager,
									RoleManager<IdentityRole> _roleManager,
									IPasswordValidator<Employee> _passwordValidator,
									IUserValidator<Employee> _userValidator,
									IWrapperRepository _wrapper,
									IPasswordHasher<Employee> _passwordHasher)
		{
			userManager = _userManager;
			roleManager = _roleManager;
			passwordValidator = _passwordValidator;
			userValidator = _userValidator;
			wrapper = _wrapper;
			passwordHasher = _passwordHasher;
		}


        public async Task<IActionResult> Index(int page = 1, string sortBy = "Unknown", string searchString = "")
		{

			IEnumerable<Employee> users;

			ViewBag.searchString = searchString;
			ViewBag.role = sortBy;
			sortBy = sortBy == "Unknown" ? "" : sortBy;

			int totalPages;

			if (searchString.IsNullOrEmpty())
			{

				totalPages = wrapper.Employee.GetAllItems().Count();

				users = await wrapper.Employee.GetWithOptions(new QueryOptions<Employee>()
				{
					OrderByRole = sortBy,
					PageNumber = page,
					PageSize = pageSize
				});
				
			}
			else
			{
				searchString = searchString.ToLower().Trim();

				totalPages = wrapper.Employee.GetAllItems().Where(e => e.FirstName.ToLower().Contains(searchString) || e.LastName.ToLower().Contains(searchString) || e.UserName.ToLower().Contains(searchString)).Count();

				users = await wrapper.Employee.GetWithOptions(new QueryOptions<Employee>()
				{
					OrderByRole = sortBy,
					PageNumber = page,
					PageSize = pageSize,
					Where = e => e.FirstName.ToLower().Contains(searchString) || e.LastName.ToLower().Contains(searchString) || e.UserName.ToLower().Contains(searchString)
				});
				
			}

			Dictionary<string, string> userRoles = new Dictionary<string, string>();

			foreach(Employee employee in users)
			{
				var roles = await userManager.GetRolesAsync(employee);

				userRoles.Add(employee.UserName, string.Join(",", roles));
			}

			List<string> allRoles = roleManager.Roles.Select(r => r.Name).ToList();
			allRoles.Insert(0, "Unknown");			

			allRoles.Remove(ViewBag.role);

			allRoles.Insert(0, ViewBag.role);

			var pageInfo = new PageInformation
			{
				CurrentPage = page,
				ItemsPerPage = pageSize,
				TotalItems = totalPages
			};
			
			return View(new UserListModel { Users = users, userRoles = userRoles, pageInfo = pageInfo, allRoles = allRoles});
		}

		public async Task<IActionResult> UserDetails(string id)
		{

			IEnumerable<Project> projects = wrapper.ProjectEmployee.ProjectsForEmployee(id);

			Dictionary<Project, IEnumerable<Ticket>> projectTicket = new Dictionary<Project, IEnumerable<Ticket>>();

			foreach(Project project in projects)
			{
				projectTicket.Add(project, wrapper.Ticket.GetTicketsForProject(project.ProjectID));
			}

			Employee employee = await userManager.FindByIdAsync(id.ToString());

			UserInformation userInformation = new UserInformation();

			var roleList = await userManager.GetRolesAsync(employee);

            userInformation.employee = employee;
			userInformation.roleNames = roleList.ToList();
			userInformation.ProjectWithTickets = projectTicket;

			return View(userInformation);
		}

		public async Task<IActionResult> EditUserDetails(string id)
		{

			Employee employee = await userManager.FindByIdAsync(id.ToString());


			UserUpdateModel userEdit = new UserUpdateModel();

			userEdit.PhoneNumber = employee.PhoneNumber;
			userEdit.UserName = employee.UserName;
			userEdit.LastName = employee.LastName;
			userEdit.FirstName = employee.FirstName;
			userEdit.Email = employee.Email;


			var lstRoles = await userManager.GetRolesAsync(employee);

			string[] allRoles = roleManager.Roles.Select(r => r.Name).ToArray();

			userEdit.allRoles = allRoles;
			userEdit.userRoles = lstRoles.ToArray();

			return View(userEdit);

		}

		[HttpPost]
		public async Task<IActionResult> EditUserDetails(UserUpdateModel userEdit, string password)
		{

			userEdit.userRoles = userEdit.allRoles;

			ModelState.Remove("password");
			ModelState.Remove("userRoles");
			ModelState.Remove("allRoles");

			if (userEdit.userRoles == null)
			{
				 
				userEdit.userRoles = new string[] {};

			}
		

			if (ModelState.IsValid)
			{

				Employee user = await userManager.FindByNameAsync(userEdit.UserName);

				user.FirstName = userEdit.FirstName;
				user.LastName = userEdit.LastName;
				user.Email = userEdit.Email;
				user.PhoneNumber = userEdit.PhoneNumber;

				IdentityResult result = await userValidator.ValidateAsync(userManager, user);

				IdentityResult passed = await userManager.AddToRolesAsync(user, userEdit.userRoles);

				IEnumerable<string> lstRoles = await userManager.GetRolesAsync(user);

				await userManager.RemoveFromRolesAsync(user, lstRoles);

				await userManager.AddToRolesAsync(user, userEdit.userRoles);

				if (result.Succeeded)
				{					

					bool passwordChanged = string.IsNullOrEmpty(password) ? false : true;

					if(passwordChanged)
					{
						IdentityResult passwordResult = await passwordValidator.ValidateAsync(userManager, user, password);
						
						if(passwordResult.Succeeded)
						{
							string hashedPassword = passwordHasher.HashPassword(user, password);

							user.PasswordHash = hashedPassword;

							IdentityResult finalResult = await userManager.UpdateAsync(user);
							

							if(finalResult.Succeeded)
							{
								return RedirectToAction("Index");
							}
							else
							{
								AddingErrorsToModelState(finalResult);
							}
						}
						else
						{
							AddingErrorsToModelState(passwordResult);
						}

					}
					else
					{

						await userManager.UpdateAsync(user);

						return RedirectToAction("Index");
					}

				}
				else
				{
					
					AddingErrorsToModelState(result);
				}




			}


			userEdit.allRoles = roleManager.Roles.Select(r => r.Name).ToArray();

			return View(userEdit);


		}

		public async Task<IActionResult> DeleteUser(string id)
		{

			Employee employee = await userManager.FindByIdAsync(id);


			if (employee != null)
			{

				IdentityResult result = await userManager.DeleteAsync(employee);

				if(result.Succeeded)
				{
					ResponseMessage = $"Successfully deleted {employee.FirstName} {employee.LastName}. Username - {employee.UserName}";
				}
				else
				{
					ResponseMessage = $"Something went wrong and {employee.FirstName} {employee.LastName} failed to be removed.";
				}

				return RedirectToAction("Index");
			}
			else
			{
				return NotFound();
			}

		}



		private void AddingErrorsToModelState(IdentityResult result)
		{
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
		}

	}
}
