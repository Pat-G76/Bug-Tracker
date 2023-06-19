using Bug_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Bug_Tracker.Data
{
    public interface ITicketRepository : IRepositoryBase<Ticket>
    {
        IEnumerable<Ticket> GetTicketsForProject(int projectId);
        IEnumerable<Ticket> GetTicketsForEmployee(string id);

	}
}
