using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bug_Tracker.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [Required(ErrorMessage = "Fill in the comment")]
        public string? Body { get; set; }

        public string? TimeDetails { get; set; }

        public string EmployeeId { get; set; }
		public int ProjectID { get; set; }


		public Employee Employee { get; set; }		        
        public Project Project { get; set; }

    }
}
