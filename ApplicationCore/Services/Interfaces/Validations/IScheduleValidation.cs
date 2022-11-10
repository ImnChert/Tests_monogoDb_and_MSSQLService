using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles;

namespace ApplicationCore.Services.Interfaces.Validations
{
	public interface IScheduleValidation
	{
		public abstract static bool ContainSession(List<Session> sessions, Session session);
		public abstract static bool ContainSeat(List<Session> sessions, Seat seat);
		public abstract static bool DoesTheUserHasAnEntryForThisSession(List<Session> sessions, RegisteredUser user, Session session);
	}
}
