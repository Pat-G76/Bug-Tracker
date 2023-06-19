using Bug_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Bug_Tracker.Data
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        IEnumerable<Comment> GetProjectComments(int projectId);
    }

}
