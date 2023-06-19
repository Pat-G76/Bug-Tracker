using System.ComponentModel.DataAnnotations;

namespace Bug_Tracker.Models
{
    public class EmployeeTicket
    {
        public int EmployeeTicketID { get; set; }
        public string EmployeeId { get; set; }
		public int TicketID { get; set; }

        public Employee? Employee { get; set; }
        public Ticket? Ticket { get; set; }

    }
}
