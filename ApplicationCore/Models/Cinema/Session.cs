using ApplicationCore.Interfaces.Observer;
using ApplicationCore.Models.Cinema.Films;

namespace ApplicationCore.Models.Cinema
{
	internal class Session : ISubject
	{
		public Film Film { get; }
		public DateTime StartTime { get; }
		public DateTime FinishTime => StartTime + Film.Duration;
		public decimal Price => TimePrice(StartTime);
		public List<Ticket> Tickets { get; set; } = new List<Ticket>();

		public Session(Film film, DateTime startTime)
		{
			Film = film;
			StartTime = startTime;
		}
		public Session(Film film, DateTime startTime, List<Ticket> tickets) : this(film, startTime)
		{
			Tickets = tickets;
		}

		private decimal TimePrice(DateTime time) // TODO: simplefactory?
		{
			if (time.Hour >= 20)
			{
				return 200;
			}
			else if (time.Hour >= 16 && time.Hour < 20)
			{
				return 150;
			}
			else if (time.Hour >= 12 && time.Hour < 16)
			{
				return 100;
			}
			else if (time.Hour >= 8 && time.Hour < 12)
			{
				return 80;
			}

			return 0;
		}

		public void Attach(IObserver observer)
		{
			throw new NotImplementedException();
		}

		public void Detach(IObserver observer)
		{
			throw new NotImplementedException();
		}

		public void Notify(Exception ex)
			=> Tickets.ForEach(t => t.RegisteredUser.Update(this, ex));
		// TODO: Вместо исключения исользовать события
		// TODO: сделать время

	}
}
