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


        public async Task<IActionResult> CreateTicket(int id)
        {

            Project project = wrapper.Project.GetById(id);

            IEnumerable<Status> statuses = wrapper.Status.GetAllItems();
            IEnumerable<Priority> priorities = wrapper.Priority.GetAllItems();
            IEnumerable<IssueType> issueTypes = wrapper.IssueType.GetAllItems();



            return View();
        }

    }
}
