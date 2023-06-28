using ContosoUniversity.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bug_Tracker.Models
{
    public class Employee : IdentityUser
    {

        [DisplayName("First Name : ")]
        [Required(ErrorMessage = "Enter your firstname")]
        public string? FirstName { get; set; }

        [DisplayName("Last Name : ")]
        [Required(ErrorMessage = "Enter your lastname")]
        public string? LastName { get; set; }
        public DateTime DateCaptured { get; set; }

        [ForeignKey("EmployeeId")]
        public ICollection<Comment>? Comments { get; set; }

        [ForeignKey("EmployeeId")]
		public ICollection<ProjectEmployee>? ProjectEmployees { get; set; }

		[ForeignKey("EmployeeId")]
		public ICollection<Ticket>? Tickets { get; set; }

	}
}
