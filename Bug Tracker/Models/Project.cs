using ContosoUniversity.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bug_Tracker.Models
{
    public class Project
    {
        public int ProjectID { get; set; }

        [DisplayName("Project Name")]
        [Required(ErrorMessage = "Enter the project name")]
        public string? ProjectName { get; set; }

        [DisplayName("Project Description")]
        [Required(ErrorMessage = "Enter the description on what the project is about")]
        public string? ProjectDescription { get; set; }

        public DateTime TimeCreated { get; set; }


        [ForeignKey("ProjectID")]
        public List<Ticket>? Tickets { get; set; }

        [ForeignKey("ProjectID")]
        public ICollection<ProjectEmployee>? ProjectEmployees { get; set; }
    }
}
