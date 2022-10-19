using ApplicationCore.Models.Roles;
using ApplicationCore.Models.Roles.Staff.Positions;

namespace ApplicationCore.Models.Cinema
{
	internal class Ticket
	{
		public Seat Seat { get; set; }
		public Session Session { get; set; }
		public RegisteredUser RegisteredUser { get; set; }
		public Cashier Cashier { get; set; }	
	}
}
