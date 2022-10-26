using ApplicationCore.Models.Roles;
using ApplicationCore.Models.Roles.Staff.Positions;
using Infrastructure.Models;

namespace ApplicationCore.Models.Cinema
{
	internal class Ticket: EntityBase
	{
		public Seat Seat { get; set; }
		public Session Session { get; set; }
		public RegisteredUser RegisteredUser { get; set; }
		public Cashier Cashier { get; set; }	
	}
}
