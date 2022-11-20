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
		{
			if (session == null)
				return false;
			return _schedule.Sessions.Contains(session);
		}

		public bool ContainSeat(Seat seat)
		{
			if (seat == null)
				return false;
			return _schedule.Sessions
			.Any(s => s.Tickets
				.Any(t => t.Seat.NumberRow == seat.NumberRow && t.Seat.NumberColumn == seat.NumberColumn)
			);
		}

		public bool DoesTheUserHasAnEntryForThisSession(RegisteredUser user, Session session)
		{
			if (session == null)
				return false;

			if (user == null)
				return false;

			return _schedule.Sessions
			.Where(s => s == session)
			.Any(s => s.Tickets
				.Any(t => t.RegisteredUser.Phone == user.Phone)
			);
		}
	}
}
