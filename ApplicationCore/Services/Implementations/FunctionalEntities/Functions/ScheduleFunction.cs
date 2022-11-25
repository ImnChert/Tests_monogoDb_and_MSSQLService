using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Services.Interfaces.Functional_entities;

namespace ApplicationCore.Services.Implementations.FunctionalEntities.Implementations
{
	public class ScheduleFunction : IScheduleFunction
	{
		private Schedule _schedule;

		public ScheduleFunction(Schedule schedule)
		{
			_schedule = schedule;
		}

		public bool AddSession(Session session)
		{
			_schedule.Sessions.Add(session);
			return true;
		}

		public bool AddTicket(RegisteredUser user, Session session, Seat seat)
		{
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
	}
}
