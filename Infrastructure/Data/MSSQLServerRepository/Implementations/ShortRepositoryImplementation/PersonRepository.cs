using ApplicationCore.Domain.Core.Models.Cinema.Films;
using Infrastructure.Data.MSSQLServerRepository.Connection.Extensions;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.ShortRepositoryImplementation
{
	public class PersonRepository : MSSSQLShortRepository<Film>
	{
		public PersonRepository(string connectionString)
			: base(connectionString,
				  "People",
				  $@"INSERT INTO People (Id,FirstName, MiddleName,LastName,FilmsId,Post) 
					VALUES(@Id,@Username,@Password,@FirstName, @MiddleName,@LastName,@DateOfBirthday,@Phone)",
				  $@"UPDATE People SET FirstName=@FirstName,MiddleName=@MiddleName, LastName=@LastName,
									FilmsId=@FilmsId,Post=@Post 
					WHERE Id=@Id")
		{
		}

		protected override Task<bool> InsertSqlCommand(SqlCommand sqlCommand, Film entity)
		{
			throw new NotImplementedException();
		}

		protected override Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, Film entity)
		{
			throw new NotImplementedException();
		}
	}
}
