using ApplicationCore.Domain.Core.Interfaces.Observer;
using ApplicationCore.Domain.Core.Models.Cinema.Films;

namespace ApplicationCore.Domain.Core.Models.Cinema
{
	public class Session : EntityBase, ISubject
	{
		public Film Film { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime FinishTime { get; set; } //=> //StartTime + Film.Duration; // TODO: сделать
		public decimal Price => TimePrice(StartTime);
		public List<Ticket> Tickets { get; set; } = new List<Ticket>();

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

		public override bool Equals(object? obj)
		{
			return obj is Session session &&
				   Id == session.Id;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}


		// TODO: Вместо исключения исользовать события
		// TODO: сделать время
	}
}
