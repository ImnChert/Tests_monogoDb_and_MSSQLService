using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models;

namespace ApplicationCore.Services.Interfaces.Functional_entities.Services
{
	public interface IScheduleService
	{
		public BaseResponse<bool> AddSession(Session session);
		public BaseResponse<bool> AddTicket(RegisteredUser user, Session session, Seat seat);
		public BaseResponse<bool> RemoveTicket(RegisteredUser user, Session session, Seat seat);
		public BaseResponse<bool> ConfirmPayment(Session session, Ticket ticket, Employee employee);
	}
}
