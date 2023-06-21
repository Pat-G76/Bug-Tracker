namespace Bug_Tracker.Models.ViewModels
{
	public class TicketInformation
	{

		public IEnumerable<Employee> employees { get; set; }
		public Dictionary<string, List<Ticket>> ticketsByStatus { get; set; }
		public IEnumerable<Priority> priorities { get; set; }
		public IEnumerable<IssueType> issueTypes { get; set; }
		public Project project { get; set; }

	}
}
