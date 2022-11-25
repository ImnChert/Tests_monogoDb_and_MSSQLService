using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Services.Interfaces.FunctionalEntities.Functions;
using ApplicationCore.Services.Interfaces.FunctionalEntities.Services;
using ApplicationCore.Services.Interfaces.Validations;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Services.Implementations.FunctionalEntities.Services
{
	internal class GuideFunctionService : IGuideService
	{
		private IGuideFunction _guideFunction;
		private IGuideValidation _guideValidation;

		public GuideFunctionService(IGuideFunction guideFunction, IGuideValidation guideValidation)
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
				};
			}
		}

		public BaseResponse<List<Session>> PriceOfImpressions(Schedule schedule)
		{
			try
			{
				_guideValidation.IsNotNull(schedule);

				List<Session> prices = _guideFunction.PriceOfImpressions(schedule);

				return new BaseResponse<List<Session>>
				{
					Data = prices,
					StatusCode = new OkResult(),
					Description = "Ok result"
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Session>>
				{
					StatusCode = new BadRequestResult(),
					Description = ex.Message
				};
			}
		}

		public BaseResponse<List<Film>> RentMovies(List<Schedule> schedules)
		{
			try
			{
				_guideValidation.IsNotNull(schedules);

				List<Film> rentMovies = _guideFunction.RentMovies(schedules);

				return new BaseResponse<List<Film>>
				{
					Data = rentMovies,
					StatusCode = new OkResult(),
					Description = "Ok result"
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Film>>
				{
					StatusCode = new BadRequestResult(),
					Description = ex.Message
				};
			}
		}

		public BaseResponse<List<Schedule>> ShowSchedule(List<Schedule> schedules)
		{
			try
			{
				_guideValidation.IsNotNull(schedules);

				List<Schedule> showSchedule = _guideFunction.ShowSchedule(schedules);

				return new BaseResponse<List<Schedule>>
				{
					Data = showSchedule,
					StatusCode = new OkResult(),
					Description = "Ok result"
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Schedule>>
				{
					StatusCode = new BadRequestResult(),
					Description = ex.Message
				};
			}
		}
	}
}
