using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Services.Interfaces.FunctionalEntities.Functions;
using ApplicationCore.Services.Interfaces.FunctionalEntities.Services;
using ApplicationCore.Services.Interfaces.Validations;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Services.Implementations.FunctionalEntities.Services
{
	internal class GuideService : IGuideService
	{
		private IGuideFunction _guideFunction;
		private IGuideValidation _guideValidation;

		public GuideService(IGuideFunction guideFunction, IGuideValidation guideValidation)
		{
			_guideFunction = guideFunction;
			_guideValidation = guideValidation;
		}

		public BaseResponse<Seat[,]> PlacesForBooking(Session session, Hall hall)
		{
			try
			{
				_guideValidation.IsNotNull(session);
				_guideValidation.IsNotNull(hall);

				Seat[,] availableSeats = _guideFunction.PlacesForBooking(session, hall);

				return new BaseResponse<Seat[,]>
				{
					Data = availableSeats,
					StatusCode = new OkResult(),
					Description = "Ok result"
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Seat[,]>
				{
					StatusCode = new BadRequestResult(),
					Description = ex.Message
				}
			}
		}
		// TODO: доделать
		public BaseResponse<List<Session>> PriceOfImpressions(Schedule schedule)
		{
			throw new NotImplementedException();
		}

		public BaseResponse<List<Film>> RentMovies(List<Schedule> schedules)
		{
			throw new NotImplementedException();
		}

		public BaseResponse<List<Schedule>> ShowSchedule(List<Schedule> schedules)
		{
			throw new NotImplementedException();
		}
	}
}
