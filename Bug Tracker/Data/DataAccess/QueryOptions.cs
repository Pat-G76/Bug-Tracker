using System.Linq.Expressions;

namespace Bug_Tracker.Data.DataAccess
{
	public class QueryOptions<T>
	{

		public Expression<Func<T, object>> OrderBy { get; set; }
		public Expression<Func<T, bool>> Where { get; set; }
		public string OrderByDirection { get; set; } = "asc";
		public string OrderByRole { get; set; } = "";
		public int PageNumber { get; set; }
		public int PageSize { get; set; }


		public bool HasOrderBy => OrderBy != null;
		public bool HasWhere => Where != null;
		public bool HasPaging => PageNumber > 0 && PageSize > 0;

	}
}
