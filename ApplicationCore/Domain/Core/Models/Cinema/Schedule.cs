namespace ApplicationCore.Domain.Core.Models.Cinema
{
	public class Schedule : EntityBase
	{
		public List<Session> Sessions { get; set; } = new List<Session>();
		public Hall Hall { get; set; }
		public DateTime Date { get; set; }

		public void Notify(Session session, Exception ex)
			=> Sessions.Where(s => s == session).Select(s => s).ToList().ForEach(s => s.Notify(ex));
	}
}
