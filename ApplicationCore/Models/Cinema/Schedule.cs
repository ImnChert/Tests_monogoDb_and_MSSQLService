using ApplicationCore.Interfaces.Observer;
using ApplicationCore.Models.Roles;

namespace ApplicationCore.Models.Cinema
{
	internal class Schedule
	{
		public List<Session> Sessions { get; } = new List<Session>();
		public Hall Hall { get; set; }
		public DateTime Date { get; set; }
		
		public Schedule()
		{ }

		public void AddSession(Session session)
		{
			if (!ContainSession(session))
				Sessions.Add(session);
			else
				throw new Exception();
		}

		private bool ContainSession(Session session)
			=> Sessions.Where(s => !((s.FinishTime < session.StartTime) || (s.StartTime > session.FinishTime))).Any();
		// TODO: сделать этот метод
		private bool ContainSeat(Seat seat)
			=> Sessions.Any(s => s.Tickets.Any(t => t.Seat == seat));
		private bool DoesTheUserHasAnEntryForThisSession(RegisteredUser user, Session session)
			=> Sessions.Where(s => s == session).Any(s => s.Tickets.Any(t => t.RegisteredUser == user));

		public void AddTicket(RegisteredUser user, Session session, Seat seat)
		{
			TicketVerification(user, session, seat);

			// TODO: сделать
		}
		public void RemoveTicket(RegisteredUser user, Session session, Seat seat)
		{
			TicketVerification(user, session, seat);

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

		public void Notify(Session session, Exception ex)
			=> Sessions.Where(s => s == session).Select(s => s).ToList().ForEach(s => s.Notify(ex));
		
	}
}
