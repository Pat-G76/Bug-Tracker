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

            ViewBag.Priorities = wrapper.Priority.GetAllItems();
            ViewBag.IssueTypes = wrapper.IssueType.GetAllItems();

            Ticket ticket = new Ticket();

            ticket.ProjectID = id;

            return View(ticket);
        }

        [HttpPost]
		public async Task<IActionResult> CreateTicket(string FirstName, string LastName, Ticket ticket, string chosenPriority, string chosenIssueType)
        {

            if(ModelState.IsValid)
            {

            }

			ViewBag.Priorities = wrapper.Priority.GetAllItems();
			ViewBag.IssueTypes = wrapper.IssueType.GetAllItems();


			return View(ticket);

        }


	}
}
