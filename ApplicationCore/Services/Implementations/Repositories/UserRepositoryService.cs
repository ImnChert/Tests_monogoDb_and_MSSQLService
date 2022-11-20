using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class UserRepositoryService : MainRepository<RegisteredUser>
	{
		public UserRepositoryService(IRepository<RegisteredUser> repository) : base(repository)
		{
		}
	}
}
