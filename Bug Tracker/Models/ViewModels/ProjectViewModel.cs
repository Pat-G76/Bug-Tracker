namespace Bug_Tracker.Models.ViewModels
{
	public class ProjectList
	{
		public PageInformation pageInfo { get; set; }
		public IEnumerable<Project> projects { get; set; }
	}

	public class ProjectEmpoloyeeList
	{ 
	
		public Project project { get; set; }
		public IEnumerable<Employee> employees { get; set; }
		public List<string> searchNames { get; set; }
	
	}

}
