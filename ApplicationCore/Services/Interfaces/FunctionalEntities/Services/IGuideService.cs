using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models;

namespace ApplicationCore.Services.Interfaces.FunctionalEntities.Services
{
	internal interface IGuideService
	{
		public BaseResponse<List<Film>> RentMovies(List<Schedule> schedules);
		public BaseResponse<List<Schedule>> ShowSchedule(List<Schedule> schedules);
		public BaseResponse<Seat[,]> PlacesForBooking(Session session, Hall hall);
		public BaseResponse<List<Session>> PriceOfImpressions(Schedule schedule);
	}
}
