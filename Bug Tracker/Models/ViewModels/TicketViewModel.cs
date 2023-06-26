namespace Bug_Tracker.Models.ViewModels
{
	public class TicketsInformation
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

	public class TicketDetails
	{

		public Ticket Ticket { get; set; }
		public Status Status { get; set; }
		public Priority Priority { get; set; }
		public IssueType IssueType { get; set; }
		public Project Project { get; set; }
		public Employee Assignee { get; set; }
		public Employee TicketCreator { get; set; }
		public IEnumerable<Comment> Comments { get; set; }
		public IEnumerable<Status> Statuses { get; set; }
		public IEnumerable<Employee> Employees { get; set; }


		public string GetCommentPostedDate(DateTime date)
		{

			string posted = "posted ";

			int totalDays = (int)(DateTime.Now - date).TotalDays;
			int totalHours = (int)(DateTime.Now - date).TotalHours;
			int totalMinutes = (int)(DateTime.Now - date).TotalMinutes;
			
			//date.Month

			int temp;

			if ( totalDays > 365 )
			{
				posted += ( totalDays % 365 ) + " year";
			}
			else if (DateTime.Now.Month <= date.Month && DateTime.Now.Year > date.Year)
			{
				if (DateTime.Now.Month == date.Month && DateTime.Now.Day < date.Day)
				{
					posted += "11 month";
				}
				else
				{
					int remainingMonths = 12 - date.Month;

					posted += (remainingMonths + DateTime.Now.Month) + " month";
				}

			}
			else if (DateTime.Now.Month > date.Month)
			{
				posted += ( DateTime.Now.Month - date.Month ) + " month" ;
			}
			else if (totalDays > 0)
			{
				posted += totalDays + " day";
			}
			else if(totalHours > 0)
			{
				posted += totalHours + " hour";
			}
			else
			{
				posted += totalMinutes + " minute";
			}


			bool IsPlural = (posted.Split(new char[] { ' ' })[1]).Equals("1") ? false : true;


			posted = IsPlural ? posted + "s ago" : posted + " ago";

			return posted; 
		}

	}

}
