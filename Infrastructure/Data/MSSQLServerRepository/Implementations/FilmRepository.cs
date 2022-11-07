using ApplicationCore.Domain.Core.Models.Cinema.Films;
using Infrastructure.Data.MSSQLServerRepository.Connection;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class FilmRepository : MainMSSQLServerRepository<Film>
	{
    }
}
