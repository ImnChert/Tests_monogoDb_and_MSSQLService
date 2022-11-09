using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	internal class UserService : MainRepository<RegisteredUser>
	{
		public UserService(IRepository<RegisteredUser> repository) : base(repository)
		{
		}
	}
}
