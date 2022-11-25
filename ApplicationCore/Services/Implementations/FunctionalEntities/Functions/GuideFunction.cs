using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Services.Interfaces.FunctionalEntities.Functions;

namespace ApplicationCore.Services.Implementations.FunctionalEntities.Functions
{
	public class GuideFunction : IGuideFunction
	{
		/// <summary>
		/// if seat equals null, then seat is't available
		/// </summary>
		/// <param name="session">session</param>
		/// <param name="hall">hall</param>
		/// <returns>available seat</returns>
		public Seat[,] PlacesForBooking(Session session, Hall hall)
		{
			var availableSeat = PreparingOfSeats(hall);

			for (int i = 0; i < hall.Row; i++)
			{
				for (int j = 0; j < hall.Column; j++)
				{
					if (session.Tickets.Any(ticket => ticket.Seat == availableSeat[i, j]))
					{
						availableSeat[i, j] = null;
					}
				}
			}

			return availableSeat;
		}

		private Seat[,] PreparingOfSeats(Hall hall)
		{
			var availableSeat = new Seat[hall.Row, hall.Column];

			for (int i = 0; i < hall.Row; i++)
			{
				for (int j = 0; j < hall.Column; j++)
				{
					availableSeat[i, j] = new Seat()
					{
						NumberRow = i + 1,
						NumberColumn = j + 1,
					};
				}
			}

			return availableSeat;
		}

		public Dictionary<Session, decimal> PriceOfImpressions(Schedule schedule)
		{
			var dictionary = new Dictionary<Session, decimal>();

			schedule.Sessions.ForEach(session =>
			{
				var sum = session.Film.BasePrice +
						session.Price +
						session.Tickets.Sum(ticket => ticket.Seat.Category.Price);
				dictionary.Add(session, sum);
			});

			return dictionary;
		}

		public List<Film> RentMovies(List<Schedule> schedules)
		{
			var rentFilms = new List<Film>();
			schedules.ForEach(schedule =>
							schedule.Sessions.ForEach(session =>
													rentFilms.Add(session.Film)));
			rentFilms.Distinct();

			return rentFilms;
		}

		public List<Schedule> ShowSchedule(List<Schedule> schedules)
			=> schedules.Where(schedule => schedule.Date >= DateTime.Now).ToList();
	}
}
