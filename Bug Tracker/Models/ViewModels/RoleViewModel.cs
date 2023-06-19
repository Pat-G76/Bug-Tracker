using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bug_Tracker.Models.ViewModels
{

	public class RoleEditModel
	{
		public IdentityRole Role { get; set; }
		public IEnumerable<IdentityUser> Members { get; set; }
		public IEnumerable<IdentityUser> NonMembers { get; set; }
		public Dictionary<string, string> userRoles { get; set; }
		public PageInformation pageInfo { get; set; }
	}

	public class RoleModificationModel
	{
		[Required]
		public string RoleName { get; set; }
		public string RoleId { get; set; }
		public string[] IdsToAdd { get; set; }
		public string[] IdsToDelete { get; set; }
	}

}

