using Microsoft.EntityFrameworkCore;
using Bug_Tracker.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace Bug_Tracker.Data
{
    public class SeedData
    {

		public static void EnsurePopulated(IApplicationBuilder app)
		{
			AppDbContext context = app.ApplicationServices
				.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();
			}

			if (!context.Priorities.Any())
			{
				context.Priorities.AddRange(
					new Priority { PriorityLevel = "Trivial" },
					new Priority { PriorityLevel = "Minor" },
					new Priority { PriorityLevel = "Major" },
					new Priority { PriorityLevel = "Critical" },
					new Priority { PriorityLevel = "Blocker" }
					);
			}

			context.SaveChanges();

			if (!context.Statuses.Any())
			{

				context.Statuses.AddRange(
					new Status { StatusTitle = "New" },
					new Status { StatusTitle = "Open" },
					new Status { StatusTitle = "Fixed" },
					new Status { StatusTitle = "Retest" },
					new Status { StatusTitle = "Closed" }
					);
			}

			context.SaveChanges();
		}
		
		public static async void CreateAdminUser(IApplicationBuilder app)
        {
			const string adminUserName = "Admin";
			const string adminfirstName = "Patrick";
			const string adminlastName = "Malapane";
			const string adminPassword = "Secret123$";
			const string adminEmail = "admin@gmail.com";
			const string adminCellphone = "066 563 5665";
			const string adminRole = "administrator";
			const string employeesRole = "developer";


			UserManager<Employee> userManager = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<UserManager<Employee>>();

            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            if (await roleManager.FindByNameAsync(adminRole) == null)
            {

                await roleManager.CreateAsync(new IdentityRole(adminRole));
                await roleManager.CreateAsync(new IdentityRole(employeesRole));				
			}

			Employee user = new Employee
            {
                FirstName = adminfirstName,
                LastName = adminlastName,
                Email = adminEmail,
                UserName = adminUserName,
                PhoneNumber = adminCellphone,
                DateCaptured = DateTime.Now
            };

            if (await userManager.FindByNameAsync(adminUserName) == null)
            {

                IdentityResult result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }


            }

        }

    }
}
