using ApplicationCore.Domain.Core.Models.Roles;
using Infrastructure.Data.MSSQLServerRepository.Connection;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class UserRepository : MainMSSQLServerRepository<RegisteredUser>
	{
    }
}
