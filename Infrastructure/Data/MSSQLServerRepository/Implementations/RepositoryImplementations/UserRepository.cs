using ApplicationCore.Domain.Core.Models.Roles;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using Infrastructure.Data.MSSQLServerRepository.Connection.Extensions;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	public class UserRepository : MSSQLRepository<RegisteredUser>
	{
		public UserRepository(string connectionString)
			: base(connectionString,
				  "RegisteredUsers",
				  $"INSERT INTO RegisteredUsers (Id,Username,Password,FirstName, MiddleName,LastName,DateOfBirthday,Phone) VALUES(@Id,@Username,@Password,@FirstName, @MiddleName,@LastName,@DateOfBirthday,@Phone)",
				  $"UPDATE RegisteredUsers SET Username=@Username, Password=@Password, FirstName=@FirstName, MiddleName=@MiddleName,LastName=@LastName,DateOfBirthday=@DateOfBirthday,Phone=@Phone WHERE Id=@Id",
				  $"SELECT Id,Username,Password,FirstName, MiddleName,LastName,DateOfBirthday,Phone FROM RegisteredUsers",
				  $"SELECT Id,Username,Password,FirstName, MiddleName,LastName,DateOfBirthday,Phone FROM RegisteredUsers WHERE Id=@id")
		{
		}

		protected override RegisteredUser GetReader(SqlDataReader sqlDataReader)
			=> new RegisteredUser()
			{
				Id = (int)sqlDataReader["Id"],
				Username = sqlDataReader["Username"] as string ?? "Undefined",
				Password = sqlDataReader["Password"] as string ?? "Undefined",
				FirstName = sqlDataReader["FirstName"] as string ?? "Undefined",
				MiddleName = sqlDataReader["MiddleName"] as string ?? "Undefined",
				LastName = sqlDataReader["LastName"] as string ?? "Undefined",
				DateOfBirthday = DateTime.Parse((string)sqlDataReader["DateOfBirthday"]),
				Phone = sqlDataReader["Phone"] as string ?? "Undefined",
			};

		protected override void InsertCommand(SqlCommand sqlCommand, RegisteredUser entity)
		{
			sqlCommand.Parameters.Add("@Username", SqlDbType.NVarChar).Value = entity.Username;
			sqlCommand.Parameters.Add("@Password", SqlDbType.NVarChar).Value = entity.Password;
			sqlCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = entity.FirstName;
			sqlCommand.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = entity.MiddleName;
			sqlCommand.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = entity.LastName;
			sqlCommand.Parameters.Add("@DateOfBirthday", SqlDbType.DateTime).Value = entity.DateOfBirthday;
			sqlCommand.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = entity.Phone;
		}
	}
}
