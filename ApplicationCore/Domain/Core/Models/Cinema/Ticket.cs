using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;

namespace ApplicationCore.Domain.Core.Models.Cinema
{
	public class Ticket : EntityBase
	{
		public Seat Seat { get; set; }
		//public Session Session { get; set; }
		public RegisteredUser RegisteredUser { get; set; }
		public Employee Cashier { get; set; } = null!;

		public override bool Equals(object? obj)
		{
			return obj is Ticket ticket &&
				   EqualityComparer<Seat>.Default.Equals(Seat, ticket.Seat) &&
				   EqualityComparer<RegisteredUser>.Default.Equals(RegisteredUser, ticket.RegisteredUser);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Seat, RegisteredUser);
		}
	}
}
