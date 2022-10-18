using ApplicationCore.Models.Roles;

namespace ApplicationCore.Models.Cinema
{
	internal class Ticket
	{
		public Seat Seat { get; set; }
		public Session Session { get; set; }
		public RegisteredUser RegisteredUser { get; set; }
		
	}
}
