using ApplicationCore.Domain.Core.Models.Cinema.Films;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using Infrastructure.Data.MSSQLServerRepository.Connection.Extensions;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.ShortRepositoryImplementation
{
	public class PersonShortRepository : MSSQLRepository<OneToMany<Person>
	{
		public PersonShortRepository(string connectionString)
			 : base(connectionString,
				  "People",
				  $@"INSERT INTO People (Id,FirstName, MiddleName,LastName,FilmsId,Post) 
					VALUES(@Id,@Username,@Password,@FirstName, @MiddleName,@LastName,@DateOfBirthday,@Phone)",
				  $@"UPDATE People SET FirstName=@FirstName,MiddleName=@MiddleName, LastName=@LastName,
									FilmsId=@FilmsId,Post=@Post 
					WHERE Id=@Id",
				  $"SELECT Id,FirstName, MiddleName,LastName,FilmsId,Post FROM People",
				  $"SELECT Id,FirstName, MiddleName,LastName,FilmsId,Post FROM People WHERE Id=@id",
				  $"SELECT Id,FirstName, MiddleName,LastName,FilmsId,Post FROM Tickets WHERE FilmsId=@FilmsId",
				  "@FilmsId")
		{
		}

		protected override OneToMany<Person> GetReader(SqlDataReader sqlDataReader)
		=> new OneToMany<Person>()
		{
			Value = new Person()
			{
				Id = (int)sqlDataReader["Id"],
				FirstName = sqlDataReader["FirstName"] as string ?? "Undefined",
				MiddleName = sqlDataReader["MiddleName"] as string ?? "Undefined",
				LastName = sqlDataReader["LastName"] as string ?? "Undefined",
				Post = sqlDataReader["Post"] as string ?? "Undefined"
			}
		};

		protected override void InsertCommand(SqlCommand sqlCommand, OneToMany<Person> entity)
		{
			sqlCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = entity.Value.FirstName;
			sqlCommand.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = entity.Value.MiddleName;
			sqlCommand.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = entity.Value.LastName;
			sqlCommand.Parameters.Add("@Post", SqlDbType.NVarChar).Value = entity.Value.Post;
			sqlCommand.Parameters.Add("@FilmsId", SqlDbType.NVarChar).Value = entity.AdditionId;
		}
	}
}
