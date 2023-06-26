using Bug_Tracker.Data;
using Bug_Tracker.Models;
using Bug_Tracker.Models.ViewModels;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Sockets;
using System.Configuration;

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


        public async Task<IActionResult> TicketDetails(int id)
        {

            Ticket ticket = wrapper.Ticket.GetById(id);            

            if(ticket == null)
            {
                return NotFound();
            }

			ViewBag.IsSubmitted = false;

			TicketDetails ticketDetails = await TicketInfo(id);

			return View(ticketDetails);
        }

        [ActionName("TicketDetails")]
        [HttpPost]
		public async Task<IActionResult> AddComment(int id, string comment)
        {

            Employee employee = await userManager.FindByNameAsync(User.Identity.Name);

            Ticket ticket = wrapper.Ticket.GetById(id);

            if (!string.IsNullOrEmpty(comment))
            {

                Comment newComment = new Comment();

                newComment.TimeCreated = DateTime.Now;
                newComment.TicketID = id;
                newComment.Body = comment;
                newComment.EmployeeId = employee.Id;

                wrapper.Comment.Add(newComment);

                wrapper.saveChanges();

            }


            

			ViewBag.IsSubmitted = true;

			TicketDetails ticketDetails = await TicketInfo(id);

            return View(ticketDetails);
                
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
		public IActionResult CreateTicket(Ticket ticket, int chosenPriority, int chosenIssueType)
        {

            if(ticket.DueDate < DateTime.Now)
                ModelState.AddModelError("", "You cannot add a due date that already passed");


			Employee assignee = string.IsNullOrEmpty(ticket.AssigneeFirstName) || string.IsNullOrEmpty(ticket.AssigneeLastName) ? null : 
                                userManager.Users.FirstOrDefault(e => e.FirstName.ToLower() == ticket.AssigneeFirstName.Trim().ToLower() && e.LastName.ToLower() == ticket.AssigneeLastName.Trim().ToLower());

			if (assignee != null)
            {

                if(!IsInProject(assignee, ticket.ProjectID))
                {
                    ModelState.AddModelError("", "The assignee is not in the project. Talk to the administrator");
                }                
                else
                {
                    ticket.AssigneeFirstName = assignee.FirstName;
                    ticket.AssigneeLastName = assignee.LastName;
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

		public IActionResult UpdateTicketStatus(int statusID, int ticketID)
        {

            Ticket ticket = wrapper.Ticket.GetById(ticketID);

            Status status = wrapper.Status.GetById(statusID);

            if(ticket != null && status != null)
            {

				ticket.OpenedDate = ticket.OpenedDate == default(DateTime) && status.StatusTitle.ToLower() != "new" ? DateTime.Now : ticket.OpenedDate;				


				ticket.StatusID = statusID;

                wrapper.Ticket.Update(ticket);

                wrapper.saveChanges();

            }

            return RedirectToAction("TicketDetails", new { id = ticketID });

        }

		private async Task<TicketDetails> TicketInfo(int id)
        {

			Ticket ticket = wrapper.Ticket.GetById(id);

			if (ticket == null)
			{
                return null;
			}

			Employee Creator = await userManager.FindByIdAsync(ticket.EmployeeId);


			TicketDetails ticketDetails = new TicketDetails();

			ticketDetails.Ticket = ticket;
			ticketDetails.Status = wrapper.Status.GetById(ticket.StatusID);
			ticketDetails.Priority = wrapper.Priority.GetById(ticket.PriorityID);
			ticketDetails.IssueType = wrapper.IssueType.GetById(ticket.IssueTypeID);
			ticketDetails.Project = wrapper.Project.GetById(ticket.ProjectID);
			ticketDetails.Assignee = userManager.Users.First(e => e.FirstName == ticket.AssigneeFirstName && e.LastName == ticket.AssigneeLastName);
			ticketDetails.TicketCreator = Creator;
            ticketDetails.Statuses = wrapper.Status.GetAllItems();
			ticketDetails.Employees = wrapper.Employee.GetAllItems();
			ticketDetails.Comments = wrapper.Comment.GetAllItems().Where(c => c.TicketID == id).Reverse();

            return ticketDetails;

		}

		private bool IsInProject(Employee employee, int projectID)
		{

			IEnumerable<ProjectEmployee> projectEmployees = wrapper.ProjectEmployee.GetAllItems().Where(pe => pe.ProjectID == projectID);

            return projectEmployees.FirstOrDefault(pe => pe.EmployeeId == employee.Id) != null;
    

		}

	}
}
