using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;

namespace ApplicationCore.Services.Interfaces.Validations
{
	public interface IScheduleValidation : IIsNotNull
	{
		public bool ContainSession(Session session);
		public bool ContainSeat(Seat seat);
		public bool DoesTheUserHasAnEntryForThisSession(RegisteredUser user, Session session);
	}
}
