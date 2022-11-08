using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
	internal class PositionRepository : MainMSSQLServerManyToManyRepository<Position>
	{
		public PositionRepository(string connectionString) 
			: base(connectionString,
				  $@"SELECT Positions.Id, Positions.NamePosition
					FROM Employes
					JOIN EmployeePosition ON EmployeeId = Employes.Id
					JOIN Positions ON PositionID = Positions.Id
					WHERE Positions.Id=@Id", 
				  setManyToManyQuery)
		{
		}
	}
}
