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

            if (!context.IssueTypes.Any())
            {

                context.IssueTypes.AddRange(
                    new IssueType { TypeTitle = "Defect" },
                    new IssueType { TypeTitle = "Enhancement" },
                    new IssueType { TypeTitle = "Feature" },
                    new IssueType { TypeTitle = "Task" },
                    new IssueType { TypeTitle = "Patch" }
                    );
            }

            context.SaveChanges();

        }

		private static readonly List<Employee> SeedUsers = new()
		{
		   new Employee {UserName = "explorer", FirstName = "Guest", LastName = "Visitor", Email = "visitor@gmail.com", PhoneNumber = "066 234 5665", DateCaptured = DateTime.Now},
		   new Employee {UserName = "Admin", FirstName = "Patrick", LastName = "Smith", Email = "admin@gmail.com", PhoneNumber = "066 883 5545", DateCaptured = DateTime.Now},		   
		   new Employee {UserName = "putswane", FirstName = "Jonas", LastName = "Maredi", Email = "jonny@gmail.com", PhoneNumber = "066 563 3465", DateCaptured = DateTime.Now},
		   new Employee {UserName = "comedy404", FirstName = "Piet", LastName = "Mashao", Email = "piet@gmail.com", PhoneNumber = "066 563 1225", DateCaptured = DateTime.Now},
		   new Employee {UserName = "psycho69", FirstName = "Patrick", LastName = "Bateman", Email = "paul@gmail.com", PhoneNumber = "066 563 5165", DateCaptured = DateTime.Now},
		   new Employee {UserName = "healer", FirstName = "Freddy", LastName = "Johnson", Email = "freddy@gmail.com", PhoneNumber = "063 563 6565", DateCaptured = DateTime.Now},
		   new Employee {UserName = "joy66", FirstName = "Betty", LastName = "Maredi", Email = "betty@gmail.com", PhoneNumber = "066 654 5665", DateCaptured = DateTime.Now}
		   
		};

		public static async void CreateAdminUser(IApplicationBuilder app)
        {


			string adminRole = "administrator";
			string employeesRole = "developer";
			int index = 0;


			UserManager<Employee> userManager = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<UserManager<Employee>>();

            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            if (!await roleManager.RoleExistsAsync(adminRole))         
                await roleManager.CreateAsync(new IdentityRole(adminRole));		
			
			
			if (!await roleManager.RoleExistsAsync(employeesRole))
				await roleManager.CreateAsync(new IdentityRole(employeesRole));


            if (userManager.Users.Count() > 0)
            {

				foreach (Employee employee in SeedUsers)
				{

					IdentityResult result = await userManager.CreateAsync(employee, "Secret123$");

					if (result.Succeeded)
					{
						await userManager.AddToRoleAsync(employee, adminRole);
					}

					adminRole = index > 2 ? employeesRole : adminRole;

					index++;

				}
            }

        }

    }
}
