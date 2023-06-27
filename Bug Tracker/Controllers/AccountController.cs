using Bug_Tracker.Models;
using Bug_Tracker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly UserManager<Employee> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<Employee> signInManager;


        public AccountController(UserManager<Employee> _userManager,
                                 RoleManager<IdentityRole> _roleManager,
                                 SignInManager<Employee> _signInManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;
        }

		[AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            

            return View(new LoginModel
            {
				returnUrl = returnUrl
            });
        }
        
        [AllowAnonymous]
		public async Task<IActionResult> DemoLogin()
		{

            Employee employee = await userManager.FindByNameAsync("explorer");

            if(employee != null)
            {
                await signInManager.SignOutAsync();

                await signInManager.SignInAsync(employee, false);

                return RedirectToAction("Index", "Project");
            }

            return RedirectToAction("Login", "Account");

		}


		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		[HttpPost]		
        public async Task<IActionResult> Login(LoginModel user)
        {

            if (ModelState.IsValid)
            {

                Employee employee = await userManager.FindByNameAsync(user.UserName);                

                if (employee != null)
                {

                    var isCorrectPassword = await signInManager.CheckPasswordSignInAsync(employee, user.Password, false);

                    if(isCorrectPassword.Succeeded)
                    {
                        var roles = await userManager.GetRolesAsync(employee);

						if (roles.Count == 0)
                        {
                            ModelState.AddModelError("", "Please wait for the administrator to register you.");

                            return View(user);

                        }

                        await signInManager.PasswordSignInAsync(employee, user.Password, true, false);

                        return Redirect(user?.returnUrl ?? "/Project/Index");

                    }                    
                   
                }

				ModelState.AddModelError("", "Invalid username or password");

			}
			
			return View(user);

        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            if (!ModelState.IsValid)
            {                

                return View(user);

            }

            Employee employee = new ()
            {
                FirstName = char.ToUpper(user.FirstName[0]) + user.FirstName.Substring(1).ToLower(),
                LastName = char.ToUpper(user.LastName[0]) + user.LastName.Substring(1).ToLower(),
                UserName = user.UserName,
                Email = user.Email,
                DateCaptured = DateTime.Now,
                PhoneNumber = user.PhoneNumber
                
            };

            IdentityResult result = await userManager.CreateAsync(employee, user.Password);

            if(result.Succeeded)
            {

                return View("RegistrationSuccess");

            }


            AddingErrorsToModelState(result);

            return View(user);

        }

        private void AddingErrorsToModelState(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }


        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();

            return RedirectToAction("Dashboard", "Home");

        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
