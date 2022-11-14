using ApplicationCore.Domain.Core.Models.Roles.Staff;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.ManyToMany
{
	internal class PositionRepository : MainMSSQLServerManyToManyRepository<Position>
	{
		public PositionRepository(string connectionString)
			: base(connectionString,
				  $@"SELECT Positions.Id, Positions.NamePosition
					FROM Employes
					JOIN EmployeePosition ON EmployeeId = Employes.Id
					JOIN Positions ON PositionID = Positions.Id
					WHERE Employes.Id=@Id",
				  $@"INSERT INTO EmployeePosition (EmployeeId,PositionID) VALUES(@EmployeeId,@PositionID)")
		{
		}

		protected override Position GetCommand(SqlDataReader sqlDataReader)
		{
			throw new NotImplementedException();
		}

		protected override Task<bool> SetCommand(SqlCommand sqlCommand, ManyToMany<Position> value)
		{
			throw new NotImplementedException();
		}
	}
}
