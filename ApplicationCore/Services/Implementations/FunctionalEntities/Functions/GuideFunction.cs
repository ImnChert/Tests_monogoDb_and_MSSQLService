using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Services.Interfaces.FunctionalEntities.Functions;

namespace ApplicationCore.Services.Implementations.FunctionalEntities.Functions
{
	public class GuideFunction : IGuideFunction
	{
		public Seat[,] PlacesForBooking(Session session, Hall hall)
		{
			throw new NotImplementedException();
		}

		public List<Session> PriceOfImpressions(Schedule schedule)
		{
			throw new NotImplementedException();
		}

		public List<Film> RentMovies(List<Schedule> schedules)
		{
			throw new NotImplementedException();
		}

		public List<Schedule> ShowSchedule(List<Schedule> schedules)
		{
			throw new NotImplementedException();
		}
	}
}
