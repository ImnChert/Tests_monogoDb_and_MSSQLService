using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class EmployeeRepositoryService : MainRepository<Employee>
	{
		public EmployeeRepositoryService(IRepository<Employee> repository)
			: base(repository) { }
	}
}
