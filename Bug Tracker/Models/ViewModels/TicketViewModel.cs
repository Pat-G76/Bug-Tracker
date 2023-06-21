namespace Bug_Tracker.Models.ViewModels
{
	public class TicketInformation
	{

		public IEnumerable<Employee> employees { get; set; }
		public Dictionary<string, List<Ticket>> ticketsByStatus { get; set; }
		public IEnumerable<Priority> priorities { get; set; }
		public IEnumerable<IssueType> issueTypes { get; set; }
		public Project project { get; set; }


		public string GetColor(int priorityID)
		{

			switch(priorityID)
			{

				case 5: return "#FF0000"; 
				case 4: return "#FFA500";
				case 3: return "#FFFF00";
				case 2: return "#00FF00";
				case 1: return "#0000ff";
				default: return "#ffffff";

			}
		
		}

	}
}
