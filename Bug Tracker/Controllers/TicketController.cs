using Bug_Tracker.Data;
using Bug_Tracker.Models;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        public readonly UserManager<Employee> userManager;
        public readonly IWrapperRepository wrapper;

        public TicketController(UserManager<Employee> _userManager,
                                IWrapperRepository _wrapper)
        {
            userManager = _userManager;
            wrapper = _wrapper;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> CreateTicket(int id)
        {

			Employee employee = await userManager.FindByNameAsync(User.Identity.Name);
			Ticket ticket = new Ticket();

			ticket.ProjectID = id;
			ticket.EmployeeId = employee.Id;


            if (wrapper.Project.GetById(id) == null)
            {
				return NotFound();
			}
            else if( !await userManager.IsInRoleAsync(employee, "administrator") && !IsInProject(employee, id) )
            {
				return NotFound();
			}


			ViewData["ProjectName"] = wrapper.Project.GetById(id).ProjectName;
			ViewBag.Changes = "Create";
			ViewBag.Priorities = wrapper.Priority.GetAllItems();
            ViewBag.IssueTypes = wrapper.IssueType.GetAllItems();


            return View(ticket);
        }

		public async Task<IActionResult> UpdateTicket(int id)
		{

            Ticket ticket = wrapper.Ticket.GetById(id);
			Employee employee = await userManager.FindByNameAsync(User.Identity.Name);

			if (ticket == null)
            {
                return NotFound();
            }
            else if (!await userManager.IsInRoleAsync(employee, "administrator") && !IsInProject(employee, id))
			{
				return NotFound();
			}

            ViewData["ProjectName"] = wrapper.Project.GetById( ticket.ProjectID );
			ViewBag.Changes = "Update";
			ViewBag.Priorities = wrapper.Priority.GetAllItems();
			ViewBag.IssueTypes = wrapper.IssueType.GetAllItems();

			return View("CreateTicket", ticket);

		}

		[HttpPost]
		public async Task<IActionResult> CreateTicket(Ticket ticket, int chosenPriority, int chosenIssueType)
        {

            if(ticket.DueDate < DateTime.Now)
                ModelState.AddModelError("", "You cannot add a due date that already passed");


			Employee assignee = string.IsNullOrEmpty(ticket.AssigneeFirstName) && string.IsNullOrEmpty(ticket.AssigneeLastName) ? null : userManager.Users.FirstOrDefault(e => e.FirstName.ToLower() == ticket.AssigneeFirstName.ToLower() && e.LastName.ToLower() == ticket.AssigneeLastName.ToLower());

			if (assignee != null)
            {

                if(!IsInProject(assignee, ticket.ProjectID))
                {
                    ModelState.AddModelError("", "The assignee is not in the project. Talk to the administrator");
                }                

			}
			else
            {
                ModelState.AddModelError("", "Assignee FirstName and/or LastName do not match");
            }


			if (ModelState.IsValid)
            {
                
                if (ticket.TicketID == 0)
                 {

                    ticket.PriorityID = chosenPriority;
                    ticket.IssueTypeID = chosenIssueType;
                    ticket.Created = DateTime.Now;
                    ticket.StatusID = wrapper.Status.GetByTitle("New").StatusID;

                    wrapper.Ticket.Add(ticket);
                    wrapper.saveChanges();

                    
                }
                else
                {

                    Ticket oldTicket = wrapper.Ticket.GetById(ticket.TicketID);

                    oldTicket.TicketName = ticket.TicketName;
                    oldTicket.TicketDescription = ticket.TicketDescription;
                    oldTicket.AssigneeFirstName = ticket.AssigneeFirstName;
                    oldTicket.AssigneeLastName = ticket.AssigneeLastName;
                    
					oldTicket.PriorityID = chosenPriority;
					oldTicket.IssueTypeID = chosenIssueType;										

					wrapper.Ticket.Update(oldTicket);
					wrapper.saveChanges();

				}

				return RedirectToAction("Index", "Project");

			}

			ViewData["ProjectName"] = wrapper.Project.GetById(ticket.ProjectID).ProjectName;
			ViewBag.Changes = ticket.TicketID == 0 ? "Create" : "Update";

			ViewBag.Priorities = wrapper.Priority.GetAllItems();
			ViewBag.IssueTypes = wrapper.IssueType.GetAllItems();


			return View(ticket);

        }

        private bool IsInProject(Employee employee, int projectID)
		{

			IEnumerable<ProjectEmployee> projectEmployees = wrapper.ProjectEmployee.GetAllItems().Where(pe => pe.ProjectID == projectID);

            return projectEmployees.FirstOrDefault(pe => pe.EmployeeId == employee.Id) != null;
    

		}

	}
}
