namespace Bug_Tracker.Data
{
    public interface IWrapperRepository
    {
        ICommentRepository Comment { get; }
        IEmployeeRepository Employee { get; }
        IProjectRepository Project { get; }
        ITicketRepository Ticket { get; }
		IStatusRepository Status { get; }
        IPriorityRepository Priority { get; }
		IEmployeeTicketRepository EmployeeTicket { get; }
        IProjectEmployeeRepository ProjectEmployee { get; }
        IIssueTypeRepository IssueType { get; }

        void saveChanges();
    }
}
