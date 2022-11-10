using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;

namespace ApplicationCore.Services.Interfaces.Functional_entities
{
	internal interface IScheduleFunction
	{
		public void AddSession(Schedule schedule, Session session);
		public void AddTicket(Schedule schedule, RegisteredUser user, Session session, Seat seat);
		public void RemoveTicket(Schedule schedule, RegisteredUser user, Session session, Seat seat);
	}
}
