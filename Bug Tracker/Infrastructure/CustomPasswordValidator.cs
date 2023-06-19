using Bug_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bug_Tracker.Infrastructure
{
	public class CustomPasswordValidator : PasswordValidator<Employee>
	{

		public async override Task<IdentityResult> ValidateAsync(UserManager<Employee> userManager,
																 Employee employee, string password)
		{

			IdentityResult result = await base.ValidateAsync(userManager, employee, password);

			List<IdentityError> lstErrors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

			List<string> names = new () { employee.FirstName, employee.LastName, employee.UserName };

			bool isIn = false;

			foreach(string name in names)
			{
				if (password.ToLower().Contains(name.ToLower()))
				{
					isIn = true;
				}
			}

			if(isIn)
			{

				IdentityError error = new IdentityError();

				error.Code = "HasNames";
				error.Description = "You must not put your firstname, lastname or username in your password";

				lstErrors.Add(error);

			}

			return lstErrors.Count() > 0 ? IdentityResult.Failed(lstErrors.ToArray()) : IdentityResult.Success;

		}

	}
}
