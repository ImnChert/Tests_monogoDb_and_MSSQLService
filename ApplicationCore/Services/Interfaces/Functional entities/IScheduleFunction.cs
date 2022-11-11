using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;

namespace ApplicationCore.Services.Interfaces.Functional_entities
{
	public interface IScheduleFunction
	{
		public void AddSession(Session session);
		public void AddTicket(RegisteredUser user, Session session, Seat seat);
		public void RemoveTicket(RegisteredUser user, Session session, Seat seat);
	}
}
