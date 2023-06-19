using Bug_Tracker.Data;
using Bug_Tracker.Models;
using delTemp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }

        int[] nums = new int[] { 1, 4, 6, 87, 5 };

        public IActionResult Dashboard()
        {
            
            
            ViewBag.Roles = string.Join(", ", roleManager.Roles.Select(r => r.Name));


            return View();
        }

    }
}