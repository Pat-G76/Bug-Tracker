﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Bug_Tracker.Models
{
	public class Status
	{
		public int StatusID { get; set; }
		public string? StatusTitle { get; set; }

		[ForeignKey("StatusID")]
		public ICollection<Ticket>? Tickets { get; set; }
	}
}
