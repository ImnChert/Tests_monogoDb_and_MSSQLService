using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;

namespace ApplicationCore.Services.Interfaces.Functional_entities
{
	public interface IScheduleFunction
	{
		public bool AddSession(Session session);
		public bool AddTicket(RegisteredUser user, Session session, Seat seat);
		public bool RemoveTicket(RegisteredUser user, Session session, Seat seat);
		public bool ConfirmPayment(Session session, Ticket ticket, Employee employee);
	}
}
