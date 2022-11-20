using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using Infrastructure.Data.MSSQLServerRepository.Implementations.LowerRepository;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	public class EmployeeRepository : MainMSSQLServerRepository<Employee>
	{
		private IManyToManyRepository<Position> _positionManyToManyRepository;

		public EmployeeRepository(string connectionString, IManyToManyRepository<Position> positionManyToManyRepository)
			: base(connectionString,
				  "Employes",
				  $"INSERT INTO RegisteredUsers (Id,Username,Password,FirstName, MiddleName,LastName) VALUES(@Id,@Username,@Password,@FirstName, @MiddleName,@LastName)",
				  $"UPDATE RegisteredUsers SET Username=@Username, Password=@Password, FirstName=@FirstName, MiddleName=@MiddleName,LastName=@LastName WHERE Id=@Id",
				  $"SELECT Id,Username,Password,FirstName, MiddleName,LastName FROM RegisteredUsers",
				  $"SELECT Id,Username,Password,FirstName, MiddleName,LastName FROM RegisteredUsers WHERE Id=@id")
		{
			_positionManyToManyRepository = positionManyToManyRepository;
		}

		public EmployeeRepository(string connectionString)
			: this(connectionString, new PositionRepository(connectionString))
		{
		}

		protected override Employee GetReader(SqlDataReader sqlDataReader)
			=> new Employee()
			{
				Id = (int)sqlDataReader["Id"],
				Username = sqlDataReader["Username"] as string ?? "Undefined",
				Password = sqlDataReader["Password"] as string ?? "Undefined",
				FirstName = sqlDataReader["FirstName"] as string ?? "Undefined",
				MiddleName = sqlDataReader["MiddleName"] as string ?? "Undefined",
				LastName = sqlDataReader["LastName"] as string ?? "Undefined",
				Positions = _positionManyToManyRepository.GetManyToManyAsync((int)sqlDataReader["Id"]).Result
			};

		protected override void InsertCommand(SqlCommand sqlCommand, Employee entity)
		{
			sqlCommand.Parameters.Add("@Username", SqlDbType.NVarChar).Value = entity.Username;
			sqlCommand.Parameters.Add("@Password", SqlDbType.NVarChar).Value = entity.Password;
			sqlCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = entity.FirstName;
			sqlCommand.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = entity.MiddleName;
			sqlCommand.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = entity.LastName;
			_positionManyToManyRepository.SetManyToMany(entity.Id, entity.Positions);
		}
	}
}
