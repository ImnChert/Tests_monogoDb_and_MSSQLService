using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	public class PersonRepository : MainMSSQLServerRepository<Person>
	{
		private readonly Film _film;

		public PersonRepository(string connectionString, Film film)
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
			_film = film;
		}

		protected override Person GetReader(SqlDataReader sqlDataReader)
			=> new Person()
			{
				Id = (int)sqlDataReader["Id"],
				FirstName = sqlDataReader["FirstName"] as string ?? "Undefined",
				MiddleName = sqlDataReader["MiddleName"] as string ?? "Undefined",
				LastName = sqlDataReader["LastName"] as string ?? "Undefined",
				Post = sqlDataReader["Post"] as string ?? "Undefined"
			};

		protected override void InsertCommand(SqlCommand sqlCommand, Person entity)
		{
			sqlCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = entity.FirstName;
			sqlCommand.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = entity.MiddleName;
			sqlCommand.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = entity.LastName;
			sqlCommand.Parameters.Add("@Post", SqlDbType.NVarChar).Value = entity.Post;
			sqlCommand.Parameters.Add("@FilmsId", SqlDbType.NVarChar).Value = _film.Id;
		}
	}
}
