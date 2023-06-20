using Bug_Tracker.Data;
using Bug_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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


        public IActionResult CreateTicket(int id)
        {

			ViewBag.Changes = "Create";
			ViewBag.Priorities = wrapper.Priority.GetAllItems();
            ViewBag.IssueTypes = wrapper.IssueType.GetAllItems();

            Ticket ticket = new Ticket();

            ticket.ProjectID = id;

            return View(ticket);
        }

		public IActionResult UpdateTicket(int id)
		{


            Ticket ticket = wrapper.Ticket.GetById(id);

            if(ticket == null)
            {
                return NotFound();
            }


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


            if(ModelState.IsValid)
            {

                if(ticket.TicketID == 0)
                {

                    ticket.PriorityID = chosenPriority;
                    ticket.IssueTypeID = chosenIssueType;
                    ticket.Created = DateTime.Now;
                    ticket.StatusID = wrapper.Status.GetByTitle("New").StatusID;

                }
                else
                {

                }

            }

			ViewBag.Priorities = wrapper.Priority.GetAllItems();
			ViewBag.IssueTypes = wrapper.IssueType.GetAllItems();


			return View(ticket);

        }


	}
}
