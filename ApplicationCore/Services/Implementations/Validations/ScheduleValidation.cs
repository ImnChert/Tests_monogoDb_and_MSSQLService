using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Services.Interfaces.Validations;

namespace ApplicationCore.Services.Implementations.Validations
{
	public class ScheduleValidation : IScheduleValidation
	{
		private Schedule _schedule;

		public ScheduleValidation(Schedule schedule)
		{
			_schedule = schedule;
		}

		public bool ContainSession(Session session)
			=> _schedule.Sessions.Where(s => !((s.FinishTime < session.StartTime) || (s.StartTime > session.FinishTime))).Any();
		// TODO: сделать этот метод
		public bool ContainSeat(Seat seat)
			=> _schedule.Sessions.Any(s => s.Tickets.Any(t => t.Seat == seat));
		public bool DoesTheUserHasAnEntryForThisSession(RegisteredUser user, Session session)
			=> _schedule.Sessions.Where(s => s == session).Any(s => s.Tickets.Any(t => t.RegisteredUser == user));
	}
}
