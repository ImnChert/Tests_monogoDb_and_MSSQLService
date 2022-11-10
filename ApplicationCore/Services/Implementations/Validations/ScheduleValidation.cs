using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Services.Interfaces.Validations;

namespace ApplicationCore.Services.Implementations.Validations
{
	public class ScheduleValidation : IScheduleValidation
	{
		public bool ContainSession(List<Session> sessions, Session session)
			=> sessions.Where(s => !((s.FinishTime < session.StartTime) || (s.StartTime > session.FinishTime))).Any();
		// TODO: сделать этот метод
		public bool ContainSeat(List<Session> sessions, Seat seat)
			=> sessions.Any(s => s.Tickets.Any(t => t.Seat == seat));
		public bool DoesTheUserHasAnEntryForThisSession(List<Session> sessions, RegisteredUser user, Session session)
			=> sessions.Where(s => s == session).Any(s => s.Tickets.Any(t => t.RegisteredUser == user));
	}
}
