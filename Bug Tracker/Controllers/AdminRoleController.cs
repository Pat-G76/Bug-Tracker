using Bug_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Bug_Tracker.Controllers
{
	[Authorize(Roles = "administrator")]
	public class AdminRoleController : Controller
	{

		private readonly RoleManager<IdentityRole> roleManager;
		private readonly UserManager<Employee> userManager;

        [TempData] /*Assigned value will only be available until next request */
        public string Message { get; set; }

        public AdminRoleController(RoleManager<IdentityRole> _roleManager,
								   UserManager<Employee> _userManager)
		{
			roleManager = _roleManager;
			userManager = _userManager;
		}


		public async Task<IActionResult> Index()
		{

			ViewData["Message"] = Message;

			var roles = roleManager.Roles;

			Dictionary<string, int> userInRoles = new Dictionary<string, int>();

			foreach (IdentityRole role in roles)
			{

				var users = await userManager.GetUsersInRoleAsync(role.Name);

				userInRoles.Add(role.Name, users.Count());

			}

			return View(userInRoles);
		}


		public async Task<IActionResult> EditRole(string id)
		{
			IdentityRole role = await roleManager.FindByNameAsync(id);

			if(role == null)
			{
				return NotFound();
			}

			ViewBag.Changes = "Edit";

			return View(role);

		}

		public IActionResult CreateRole()
		{
			ViewBag.Changes = "Create";

			return View("EditRole", new IdentityRole());

		}

		[HttpPost, ActionName("EditRole")]
		public async Task<IActionResult> RoleChanges(string Name, string Id)
		{

			IdentityRole role = await roleManager.FindByIdAsync(Id);

			ViewBag.Changes = role == null ? "Create" : "Edit";

			if (ModelState.IsValid)
			{
				if (role is null)
				{

					IdentityResult result = await roleManager.CreateAsync(new IdentityRole(Name));

					if(result.Succeeded)
					{

                        Message = $"Role {Name} has been succesfully added.";

                        return RedirectToAction("Index");
					}

					AddingErrorsToModelState(result);

				}
				else
				{

					role.Name = Name;

					IdentityResult result = await roleManager.UpdateAsync(role);

					if(result.Succeeded)
					{

                        Message = $"Role {Name} has been updated succesfully.";

                        return RedirectToAction("Index");
					}

					AddingErrorsToModelState(result);

				}

			}
		

			return View(role == null ? new IdentityRole() : role);

		}

		public async Task<IActionResult> DeleteRole(string id)
		{

			IdentityRole role = await roleManager.FindByNameAsync(id);

			if (role is null)
			{

				return NotFound();
			}

			await roleManager.DeleteAsync(role);

            Message = $"Role {role.Name} has been succesfully deleted.";

            return RedirectToAction("Index");

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
