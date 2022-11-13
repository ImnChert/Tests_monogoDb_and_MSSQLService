using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Services.Interfaces.Functional_entities;
using ApplicationCore.Services.Interfaces.Validations;

namespace ApplicationCore.Services.Implementations.Functional_entities
{
	public class ScheduleFunction : IScheduleFunction
	{
		private IScheduleValidation _validation;
		private Schedule _schedule;

		public ScheduleFunction(IScheduleValidation validation, Schedule schedule)
		{
			_validation = validation;
			_schedule = schedule;
		}

		public bool AddSession(Session session)
		{
			if (!_validation.ContainSession(session))
				_schedule.Sessions.Add(session);
			else
				throw new Exception();
			return true;
		}

		public bool AddTicket(RegisteredUser user, Session session, Seat seat)
		{
			TicketVerification(user, session, seat);

			_schedule.Sessions
				.Where(s => s == session)
				.First()
				.Tickets.Add(new Ticket()
				{
					Seat = seat,
					RegisteredUser = user
				});
			return true;
		}

		public bool RemoveTicket(RegisteredUser user, Session session, Seat seat)
			=> _schedule.Sessions
				.Where(s => s == session)
				.First()
				.Tickets.Remove(new Ticket()
				{
					Seat = seat,
					RegisteredUser = user
				});

		public bool ConfirmPayment(Session session, Ticket ticket, Employee employee)
		{
			session.Tickets.Where(t => t == ticket).First().Cashier = employee;

			return true;
		}

		private void TicketVerification(RegisteredUser user, Session session, Seat seat)
		{
			if (!_validation.ContainSession(session))
				throw new Exception("The schedule does not contain a littered sessi");
			if (_validation.DoesTheUserHasAnEntryForThisSession(user, session))
				throw new Exception("The user has an entry for this session");
			if (_validation.ContainSeat(seat))
				throw new Exception("This place is already booked");
		}
	}
}
