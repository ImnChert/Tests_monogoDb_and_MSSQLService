using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class UserService : MainRepository<RegisteredUser>
	{
		public UserService(IRepository<RegisteredUser> repository) : base(repository)
		{
		}
	}
}
