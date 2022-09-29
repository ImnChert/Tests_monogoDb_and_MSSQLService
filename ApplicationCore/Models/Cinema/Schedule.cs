namespace ApplicationCore.Models.Cinema
{
	internal class Schedule
	{
		public List<Session> Sessions { get; } = new List<Session>();

		public Schedule()
		{ }

		public void Add(Session session)
		{
			if (Contains(session))
				Sessions.Add(session);
			else
				throw new Exception();
		}

		private bool Contains(Session session)
			=> Sessions.Where(s => !((s.FinishTime < session.StartTime) || (s.StartTime > session.FinishTime))).Any();

	}
}
