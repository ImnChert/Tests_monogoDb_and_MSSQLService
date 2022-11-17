﻿using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class EmployeeService : MainRepository<Employee>
	{
		public EmployeeService(IRepository<Employee> repository)
			: base(repository) { }
	}
}
