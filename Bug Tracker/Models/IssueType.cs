﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Bug_Tracker.Models
{
	public class IssueType
	{
		public int IssueTypeID { get; set; }
		public string? TypeTitle { get; set; }
		[ForeignKey("TypeID")]
		public ICollection<Ticket>? Tickets { get; set; }

	}
}
