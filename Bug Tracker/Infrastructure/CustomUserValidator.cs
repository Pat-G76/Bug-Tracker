using Bug_Tracker.Models;
using Microsoft.AspNetCore.Identity;

namespace Bug_Tracker.Infrastructure
{
    public class CustomUserValidator : UserValidator<Employee>
    {

        public override async Task<IdentityResult> ValidateAsync(UserManager<Employee> userManager, Employee user)
        {

            IdentityResult result = await base.ValidateAsync(userManager, user);

            List<IdentityError> lstErrors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            if(user.FirstName == user.LastName)
            {
                IdentityError error = new IdentityError();
                error.Code = "SameFirstNameAndLastName";
                error.Description = "Your firstname can't be the same as your lastname.";

                lstErrors.Add(error);
            }

            if(user.UserName == user.FirstName || user.UserName == user.LastName)
            {
                IdentityError error = new IdentityError();
                error.Code = "UsernameSameAsLast/Firstname";
                error.Description = "Your Username can't be the same as your LastName or FirstName";
                lstErrors.Add(error);
            }


            return lstErrors.Count() > 0 ? IdentityResult.Failed(lstErrors.ToArray()) : IdentityResult.Success;
        }

    }
}
