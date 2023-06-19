using Microsoft.AspNetCore.Mvc;

namespace Bug_Tracker.Controllers
{
    public class ProjectTicketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
