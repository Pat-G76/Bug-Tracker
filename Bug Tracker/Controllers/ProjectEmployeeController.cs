﻿using Bug_Tracker.Data;
using Bug_Tracker.Models;
using Bug_Tracker.Models.ViewModels;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;

namespace Bug_Tracker.Controllers
{
	[Authorize]
	public class ProjectEmployeeController : Controller
	{

		private readonly UserManager<Employee> userManager;
		private readonly IWrapperRepository wrapper;

		public ProjectEmployeeController(UserManager<Employee> _userManager,
										 IWrapperRepository _wrapper)
		{
			userManager = _userManager;
			wrapper = _wrapper;
		}


		public IActionResult Index(int id)
		{


			Project project = wrapper.Project.GetById(id);

			IEnumerable<Employee> employees = wrapper.ProjectEmployee.EmployeesForProject(id).Select(pe => pe.Employee);

			List<string> names = new List<string>();

			
			foreach (Employee employee in userManager.Users)
			{						
				names.Add($"{employee.FirstName} {employee.LastName}");
			}



			return View(new ProjectEmpoloyeeList { employees = employees, project = project, searchNames = names});
		}

		[HttpPost]
		public IActionResult Index(int id, string fullName)
		{

			if(ModelState.IsValid)
			{

				fullName = fullName.Trim();

				string[] bothNames = fullName.Split(new char[] { ' ' });

				if (bothNames.Length == 2)
				{

					Employee employee = userManager.Users.FirstOrDefault(e => e.FirstName == bothNames[0] && e.LastName == bothNames[1]);

					if(employee == null)
					{
						ModelState.AddModelError("", "Employee not found. Please use the hint to search for an employee.");
					}
					else
					{
						ProjectEmployee projectEmployee = new ProjectEmployee();

						projectEmployee.ProjectID = id;
						projectEmployee.EmployeeId = employee.Id;

						wrapper.ProjectEmployee.Add(projectEmployee);

						wrapper.saveChanges();

						return RedirectToAction("Index");

						//wrapper.ProjectEmployee.Add()
					}

				}
				else
				{
					ModelState.AddModelError("", "Enter the employee FirstName first and then lastName. Make sure you separate them by a single space");
				}

				//Employee employee = await userManager.Users.Where(e => e.)

			}

			Project project = wrapper.Project.GetById(id);

			IEnumerable<Employee> employees = wrapper.ProjectEmployee.EmployeesForProject(id).Select(pe => pe.Employee);

			List<string> names = new List<string>();


			foreach (Employee employee in userManager.Users)
			{
				names.Add($"{employee.FirstName} {employee.LastName}");
			}



			return View(new ProjectEmpoloyeeList { employees = employees, project = project, searchNames = names });


		}

	}
}
