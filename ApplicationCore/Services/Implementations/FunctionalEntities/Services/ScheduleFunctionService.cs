using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Services.Interfaces.Functional_entities;
using ApplicationCore.Services.Interfaces.Functional_entities.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Services.Implementations.FunctionalEntities.Services
{
	public class ScheduleFunctionService : IScheduleService
	{
		private IScheduleFunction _scheduleFunction;

		public ScheduleFunctionService(IScheduleFunction scheduleFunction)
		{
			_scheduleFunction = scheduleFunction;
		}

		public BaseResponse<bool> AddSession(Session session)
		{
			try
			{
				var assert = _scheduleFunction.AddSession(session);

				return new BaseResponse<bool>()
				{
					Data = assert,
					Description = "Successfully",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Data = false,
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public BaseResponse<bool> AddTicket(RegisteredUser user, Session session, Seat seat)
		{
			try
			{
				var assert = _scheduleFunction.AddTicket(user, session, seat);

				return new BaseResponse<bool>()
				{
					Data = assert,
					Description = "Successfully",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Data = false,
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public BaseResponse<bool> ConfirmPayment(Session session, Ticket ticket, Employee employee)
		{
			try
			{
				var assert = _scheduleFunction.ConfirmPayment(session, ticket, employee);

				return new BaseResponse<bool>()
				{
					Data = assert,
					Description = "Successfully",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Data = false,
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public BaseResponse<bool> RemoveTicket(RegisteredUser user, Session session, Seat seat)
		{
			try
			{
				var assert = _scheduleFunction.AddTicket(user, session, seat);

				return new BaseResponse<bool>()
				{
					Data = assert,
					Description = "Successfully",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Data = false,
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}
	}
}
