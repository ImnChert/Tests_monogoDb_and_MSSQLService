using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	internal class FilmService : MainRepository<Film>
	{
		public FilmService(IRepository<Film> repository)
			: base(repository) { }
	}
}
