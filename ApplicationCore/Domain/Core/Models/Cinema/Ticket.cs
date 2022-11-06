using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Core.Models.Roles.Staff.Positions;

namespace ApplicationCore.Domain.Core.Models.Cinema
{
	public class Ticket: EntityBase
	{
		public Seat Seat { get; set; }
		public Session Session { get; set; }
		public RegisteredUser RegisteredUser { get; set; }
		public Employee Cashier { get; set; }
	}
}
