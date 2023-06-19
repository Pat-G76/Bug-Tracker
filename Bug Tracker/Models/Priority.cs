using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bug_Tracker.Models
{
	public class Priority
	{
		public int PriorityID { get; set; }

		public string? PriorityLevel { get; set; }

		[ForeignKey("PriorityID")]
		public List<Ticket> Tickets { get; set; }
	}
}
