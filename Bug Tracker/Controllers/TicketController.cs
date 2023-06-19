using Bug_Tracker.Data;
using Bug_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bug_Tracker.Controllers
{
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

    }
}
