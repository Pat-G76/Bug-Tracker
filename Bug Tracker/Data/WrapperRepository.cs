using Bug_Tracker.Models;
using Microsoft.AspNetCore.Identity;

namespace Bug_Tracker.Data
{
    public class WrapperRepository : IWrapperRepository
    {
        private CommentRepository _comment;
        private EmployeeRepository _employee;
        private ProjectRepository _project;
        private TicketRepository _ticket;
		private PriorityRepository _priority;
		private StatusRepository _status;
        private ProjectEmployeeRepository _project_employee;
        private IssueTypeRepository _issueType;
        
        private UserManager<Employee> userManager;

		private AppDbContext dbContext;

        public WrapperRepository(AppDbContext _dbContext,
			                     UserManager<Employee> _userManager)
        {
            dbContext = _dbContext;
            userManager = _userManager;
		}

        public ICommentRepository Comment
        {
            get
            {
                if (_comment == null)
                {
                    _comment = new CommentRepository(dbContext);
                }

                return _comment;               
            }
        }

        public IEmployeeRepository Employee
        {
            get
            {
                if (_employee == null)
                {
                    _employee = new EmployeeRepository(dbContext, userManager);
                }

                return _employee;
            }
        }

        public IProjectRepository Project
        {
            get
            {
                if (_project == null)
                {
                    _project = new ProjectRepository(dbContext);
                }

                return _project;
            }
        }

        public ITicketRepository Ticket
        {
            get
            {
                if (_ticket == null)
                {
                    _ticket = new TicketRepository(dbContext);
                }

                return _ticket;
            }
        }

		public IProjectEmployeeRepository ProjectEmployee
        {
            get
            {
                if(_project_employee == null)
                {
                    _project_employee = new ProjectEmployeeRepository(dbContext);
                }

                return _project_employee;
            }
        }

		public IStatusRepository Status
        {
            get
            {
                if(_status == null)
                {
                    _status = new StatusRepository(dbContext);
                }

                return _status;
            }
        }
		public IPriorityRepository Priority
        {
            get
            {
                if(_priority == null)
                {
                    _priority = new PriorityRepository(dbContext);
                }

                return _priority;
            }
        }

		public IIssueTypeRepository IssueType
        {
            get
            {
                if(_issueType == null)
                {
                    _issueType = new IssueTypeRepository(dbContext);

				}

                return _issueType;

			}
        }

		public void saveChanges()
        {
            dbContext.SaveChanges();
        }

    }
}
