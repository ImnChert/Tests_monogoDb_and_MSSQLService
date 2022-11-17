using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class SessionService : MainRepository<Session>
	{
		public SessionService(IRepository<Session> repository) : base(repository)
		{
		}
	}
}
