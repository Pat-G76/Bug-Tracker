using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Bug_Tracker.Models.ViewModels
{

    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter your username")]
        [UIHint("username")]        
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [UIHint("password")]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name = "First name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [Display(Name = "Last name")]
        public string? LastName { get; set; }

		[Required(ErrorMessage = "Please enter your phone number")]
		[Display(Name = "Phone Number")]
		public string? PhoneNumber { get; set; }

		[Required(ErrorMessage = "Enter email")]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }

	public class UserListModel
	{
		public IEnumerable<IdentityUser> Users { get; set; }
		public Dictionary<string, string> userRoles { get; set; }
		public PageInformation pageInfo { get; set; }
        public List<string> allRoles { get; set; }
	}

	public class UserInformation
    {

        public Dictionary<Project, IEnumerable<Ticket>> ProjectWithTickets { get; set; }
        public Employee employee { get; set; }
        public List<string> roleNames { get; set; }

	}

	public class UserEditInformation
	{
		public RegisterModel registerModel { get; set; }
		public List<string> allRoles { get; set; }
		public List<string> employeeRoles { get; set; }

        //public Dictionary<string, bool> rolesChecked { get; set; } 


	}


	public class UserUpdateModel
	{
		public string? UserName { get; set; }

		[Required(ErrorMessage = "Please enter your first name")]
		[Display(Name = "First name")]
		public string? FirstName { get; set; }

		[Required(ErrorMessage = "Please enter your last name")]
		[Display(Name = "Last name")]
		public string? LastName { get; set; }

		[Required(ErrorMessage = "Please enter your phone number")]
		[Display(Name = "Phone Number")]
		public string? PhoneNumber { get; set; }

		[Required(ErrorMessage = "Enter email")]
		[Display(Name = "E-mail")]
		[EmailAddress]
		public string? Email { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string? Password { get; set; } 

		public string[] userRoles { get; set; }
		public string[] allRoles { get; set; }


	}

}
