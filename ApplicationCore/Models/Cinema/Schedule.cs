using ApplicationCore.Interfaces;
using ApplicationCore.Models.Roles;

namespace ApplicationCore.Models.Cinema
{
	internal class Schedule
	{
		public Dictionary<Session, List<Ticket>> Sessions { get; } = new Dictionary<Session, List<Ticket>>();
		public Hall Hall { get; set; }
		public DateTime Date { get; set; }
		
		public Schedule()
		{ }

		public void AddSession(Session session)
		{
			if (!ContainSession(session))
				Sessions.Add(session, null);
			else
				throw new Exception();
		}

		private bool ContainSession(Session session)
			=> Sessions.Where(s => !((s.Key.FinishTime < session.StartTime) || (s.Key.StartTime > session.FinishTime))).Any();
		// TODO: сделать этот метод

		private bool ContainSeat(Seat seat)
			=> Sessions.Any(s => s.Value.Any(t => t.Seat == seat));
		private bool DoesTheUserHasAnEntryForThisSession(RegisteredUser user, Session session)
			=> Sessions.Where(s => s.Key == session).Any(s => s.Value.Any(t => t.RegisteredUser == user));

		public void AddTicket(RegisteredUser user, Session session, Seat seat)
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
	}
}
