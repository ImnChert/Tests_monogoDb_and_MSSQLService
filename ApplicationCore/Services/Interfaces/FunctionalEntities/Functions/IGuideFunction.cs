using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;

namespace ApplicationCore.Services.Interfaces.FunctionalEntities.Functions
{
	internal interface IGuideFunction
	{
		public List<Film> RentMovies(List<Schedule> schedules);
		public List<Schedule> ShowSchedule(List<Schedule> schedules);
		public Seat[,] PlacesForBooking(Session session, Hall hall);
		public List<Session> PriceOfImpressions(Schedule schedule);
	}
}
