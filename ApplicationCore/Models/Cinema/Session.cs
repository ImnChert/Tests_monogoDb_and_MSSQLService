using ApplicationCore.Models.Cinema.Films;

namespace ApplicationCore.Models.Cinema
{
	internal class Session
	{
		public FilmDistribution FilmDistribution { get; }

		public DateTime StartTime { get; }
		public DateTime FinishTime { get; }

		public Session(FilmDistribution filmDistribution)
			=> FilmDistribution = filmDistribution;
		// TODO: сделать время

	}
}
