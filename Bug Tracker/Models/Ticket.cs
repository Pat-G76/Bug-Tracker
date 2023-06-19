using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bug_Tracker.Models
{
    public class Ticket
    {
        public int TicketID { get; set; }

        [DisplayName("Ticket Name")]
        [Required(ErrorMessage = "Please enter the ticket name")]
        public string TicketName { get; set; }

		public string Assignee { get; set; }

		[DisplayName("Ticket Description")]
        [Required(ErrorMessage = "Please enter the description")]
        public string? TicketDescription { get; set; }

   
        public DateTime OpenedDate { get; set; }

		public DateTime Created { get; set; }

		[DisplayName("Due date")]
		[Required(ErrorMessage = "Please specify the ticket deadline")]
		public DateTime DueDate { get; set; }


		public int ProjectID { get; set; }
		public int StatusID { get; set; }
        public int PriorityID { get; set; }
		public int IssueTypeID { get; set; }

		public Project Project { get; set; }
		public Status Status { get; set; }
		public Priority Priority { get; set; }
		public IssueType IssueType { get; set; }


		[ForeignKey("TicketID")]
		public ICollection<EmployeeTicket>? EmployeeTickets { get; set; }
        
    }
}
