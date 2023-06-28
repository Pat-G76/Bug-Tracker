using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bug_Tracker.Models.ViewModels
{

	public class RoleListModel
	{
		public IQueryable<IdentityRole>? Roles { get; set; }
		public List<int>? Members { get; set; }
		
	}

}

