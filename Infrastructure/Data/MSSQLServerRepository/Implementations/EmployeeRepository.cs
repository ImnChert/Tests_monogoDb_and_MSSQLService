using ApplicationCore.Domain.Core.Models.Roles.Staff;
using Infrastructure.Data.MSSQLServerRepository.Connection;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class EmployeeRepository : MainMSSQLServerRepository<Employee>
	{
    }
}
