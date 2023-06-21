using Bug_Tracker.Data;
using Bug_Tracker.Models;
using Bug_Tracker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class ProjectTicketController : Controller
    {
        private readonly UserManager<Employee> userManager;
        private readonly IWrapperRepository wrapper;

        public ProjectTicketController(UserManager<Employee> _userManager,
									   IWrapperRepository _wrapper)
        {
            userManager = _userManager;
            wrapper = _wrapper;
        }

        public async Task<IActionResult> Index(int id)
        {

            if(wrapper.Project.GetById(id) == null)
            {
                return NotFound();
            }

			Employee employee = await userManager.FindByNameAsync(User.Identity.Name);            

			List<Ticket> createdTickets = new List<Ticket>();
			List<Ticket> assignedTickets = new List<Ticket>();
			List<Ticket> restOfTickets = new List<Ticket>();
			List<Ticket> finalTickets = new List<Ticket>();


			Stack<Ticket> ticketsStack = new Stack<Ticket>(wrapper.Ticket.GetAllItems());

            while(ticketsStack.Count > 0)
            {
                Ticket ticket = ticketsStack.Pop();

                if(ticket.EmployeeId == employee.Id)
                {
                    createdTickets.Add(ticket);
                }
                else if(ticket.AssigneeFirstName == employee.FirstName && ticket.AssigneeLastName == employee.LastName)
                {
                    assignedTickets.Add(ticket);
                }
                else
                {
                    restOfTickets.Add(ticket);
                }

            }

            finalTickets.AddRange(assignedTickets);
            finalTickets.AddRange(createdTickets);
            finalTickets.AddRange(restOfTickets);


            Dictionary<string, List<Ticket>> ticketsByStatus = new Dictionary<string, List<Ticket>>();

            foreach(Status status in wrapper.Status.GetAllItems())
            {

                ticketsByStatus.Add(status.StatusTitle, new List<Ticket>());

            }


            foreach(Ticket ticket in finalTickets)
            {
                string statusTitle = wrapper.Status.GetById(ticket.StatusID).StatusTitle;


                ticketsByStatus[statusTitle].Add(ticket);

            }

			TicketInformation ticketList = new TicketInformation()
            {
                ticketsByStatus = ticketsByStatus,
                issueTypes = wrapper.IssueType.GetAllItems(),
                priorities = wrapper.Priority.GetAllItems(),
                project = wrapper.Project.GetById(id),
                employees = userManager.Users
            };

			return View(ticketList);
        }
    }
}
