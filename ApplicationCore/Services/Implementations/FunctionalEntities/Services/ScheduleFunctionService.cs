using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Services.Interfaces.Functional_entities;
using ApplicationCore.Services.Interfaces.Functional_entities.Services;
using ApplicationCore.Services.Interfaces.Validations;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace ApplicationCore.Services.Implementations.FunctionalEntities.Services
{
	public class ScheduleFunctionService : IScheduleService
	{
		private IScheduleFunction _scheduleFunction;
		private IScheduleValidation _scheduleValidation;

		public ScheduleFunctionService(IScheduleFunction scheduleFunction, IScheduleValidation scheduleValidation)
		{
			_scheduleFunction = scheduleFunction;
			_scheduleValidation = scheduleValidation;
		}

		public BaseResponse<bool> AddSession(Session session)
		{
			try
			{
				_scheduleValidation.IsNotNull(session);

				bool assert = false;

				if (!_scheduleValidation.ContainSession(session))
				{
					assert = _scheduleFunction.AddSession(session);
				}

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
				_scheduleValidation.IsNotNull(user);
				_scheduleValidation.IsNotNull(session);
				_scheduleValidation.IsNotNull(seat);

				TicketVerification(user, session, seat);

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

		private void TicketVerification(RegisteredUser user, Session session, Seat seat)
		{
			if (!_scheduleValidation.ContainSession(session))
				throw new Exception("The schedule does not contain a littered sessi");
			if (_scheduleValidation.DoesTheUserHasAnEntryForThisSession(user, session))
				throw new Exception("The user has an entry for this session");
			if (_scheduleValidation.ContainSeat(seat))
				throw new Exception("This place is already booked");
		}

		public BaseResponse<bool> ConfirmPayment(Session session, Ticket ticket, Employee employee)
		{
			try
			{
				_scheduleValidation.IsNotNull(session);
				_scheduleValidation.IsNotNull(ticket);
				_scheduleValidation.IsNotNull(employee);

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
				_scheduleValidation.IsNotNull(user);
				_scheduleValidation.IsNotNull(session);
				_scheduleValidation.IsNotNull(seat);

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
