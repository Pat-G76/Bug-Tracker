using Bug_Tracker.Models;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class ProjectEmployee
    {
        public int ProjectEmployeeID { get; set; }
        public int ProjectID { get; set; }
		public string EmployeeId { get; set; }

        public Project Project { get; set; }
        public Employee Employee { get; set; }
    }
}
