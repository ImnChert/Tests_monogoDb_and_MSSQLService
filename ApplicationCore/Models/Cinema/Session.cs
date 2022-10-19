using ApplicationCore.Models.Cinema.Films;

namespace ApplicationCore.Models.Cinema
{
	internal class Session
	{
		public Film Film { get; }

		public DateTime StartTime { get; }
		public DateTime FinishTime { get; }
		public decimal Price
		{
			get
			{
				return Price;
				// TODO: если такое время то такая цена если такое то такая
			}
		}

		public Session(Film filmDistribution)
			=> Film = filmDistribution;
		// TODO: сделать время

	}
}
