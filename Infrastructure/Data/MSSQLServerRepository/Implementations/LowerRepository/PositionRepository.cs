using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Core.Models.Roles.Staff.Positions;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using SharpCompress.Common;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.LowerRepository
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

		private Dictionary<string, Position> _positions = new Dictionary<string, Position>()
		{
			{"Admin", new Admin()}

		};

		protected override Position GetCommand(SqlDataReader sqlDataReader)
		{
			var position = _positions[(string)sqlDataReader["NamePosition"]];
			position.Id = (int)sqlDataReader["Id"];

			return position;
		}

		protected override async Task SetCommand(SqlCommand sqlCommand, ManyToMany<Position> value)
			=> value.ManyList.ForEach(item =>
			{
				sqlCommand.Parameters.Add("@EmployeeId", SqlDbType.NVarChar).Value = value.Id;
				sqlCommand.Parameters.Add("@PositionID", SqlDbType.NVarChar).Value = item.Id;
			});

		protected override void InsertCommand(SqlCommand sqlCommand, Position entity)
		{
			throw new NotImplementedException();
		}
	}
}

