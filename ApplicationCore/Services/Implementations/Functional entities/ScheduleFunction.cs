using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Services.Interfaces.Functional_entities;
using ApplicationCore.Services.Interfaces.Validations;

namespace ApplicationCore.Services.Implementations.Functional_entities
{
	public class ScheduleFunction : IScheduleFunction
	{
		private IScheduleValidation _validation;

		public ScheduleFunction(IScheduleValidation validation)
		{
			this._validation = validation;
		}

		public void AddSession(Schedule schedule, Session session)
		{
			if (!_validation.ContainSession(schedule.Sessions, session))
				schedule.Sessions.Add(session);
			else
				throw new Exception();
		}

		public void AddTicket(RegisteredUser user, Session session, Seat seat)
		{
			TicketVerification(user, session, seat);

			// TODO: сделать
		}
		public void RemoveTicket(RegisteredUser user, Session session, Seat seat)
		{
			//TicketVerification(user, session, seat);

			// TODO: сделать
		}

		private void TicketVerification(RegisteredUser user, Session session, Seat seat)
		{
			if (!ContainSession(session))
				throw new Exception("The schedule does not contain a littered sessi");
			if (DoesTheUserHasAnEntryForThisSession(user, session))
				throw new Exception("The user has an entry for this session");
			if (ContainSeat(seat))
				throw new Exception("This place is already booked");
		}
	}
}
