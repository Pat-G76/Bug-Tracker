using Bug_Tracker.Models;

namespace Bug_Tracker.Data
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {

        public CommentRepository(AppDbContext dbContext)
                                : base (dbContext)
                                    
        {

        }

        public IEnumerable<Comment> GetProjectComments(int projectId)
        {
            return dbContext.Comments.Where(c => c.ProjectID == projectId);
        }

    }
}
