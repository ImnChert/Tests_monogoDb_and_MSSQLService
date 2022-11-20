using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	public class PersonRepository : MainMSSQLServer, IUpdateAsync<Film>
	{
		public PersonRepository(string connectionString, Film film)
			: base(connectionString,
				  "People",
				  $@"INSERT INTO People (Id,FirstName, MiddleName,LastName,FilmsId,Post) 
					VALUES(@Id,@Username,@Password,@FirstName, @MiddleName,@LastName,@DateOfBirthday,@Phone)",
				  $@"UPDATE People SET FirstName=@FirstName,MiddleName=@MiddleName, LastName=@LastName,
									FilmsId=@FilmsId,Post=@Post 
					WHERE Id=@Id")
		{
			_film = film;
		}

		public Task<bool> UpdateAsync(Film entity)
		{
			throw new NotImplementedException();
		}
	}
}
