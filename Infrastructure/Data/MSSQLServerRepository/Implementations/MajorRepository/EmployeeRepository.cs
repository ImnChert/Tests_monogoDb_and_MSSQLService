using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	internal class EmployeeRepository : MainMSSQLServerRepository<Employee>
	{
		private IManyToManyRepository<Position> _repository;

		public EmployeeRepository(string connectionString)
			: base(connectionString,
				  "Employes",
				  $"INSERT INTO RegisteredUsers (Id,Username,Password,FirstName, MiddleName,LastName) VALUES(@Id,@Username,@Password,@FirstName, @MiddleName,@LastName)",
				  $"UPDATE RegisteredUsers SET Username=@Username, Password=@Password, FirstName=@FirstName, MiddleName=@MiddleName,LastName=@LastName WHERE Id=@Id",
				  $"SELECT Id,Username,Password,FirstName, MiddleName,LastName FROM RegisteredUsers",
				  $"SELECT Id,Username,Password,FirstName, MiddleName,LastName FROM RegisteredUsers WHERE Id=@id")
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
				Positions = _repository.GetManyToManyAsync((int)sqlDataReader["Id"]).Result
			};

		protected override void InsertCommand(SqlCommand sqlCommand, Employee entity)
		{
			sqlCommand.Parameters.Add("@Username", SqlDbType.NVarChar).Value = entity.Username;
			sqlCommand.Parameters.Add("@Password", SqlDbType.NVarChar).Value = entity.Password;
			sqlCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = entity.FirstName;
			sqlCommand.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = entity.MiddleName;
			sqlCommand.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = entity.LastName;
			_repository.SetManyToMany(entity.Id, entity.Positions);
		}
	}
}
