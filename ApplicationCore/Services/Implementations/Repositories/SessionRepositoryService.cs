using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class SessionRepositoryService : MainRepository<Session>
	{
		public SessionRepositoryService(IRepository<Session> repository) : base(repository)
		{
		}
	}
}
