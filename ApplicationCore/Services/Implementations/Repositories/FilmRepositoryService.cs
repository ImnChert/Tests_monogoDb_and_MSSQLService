using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class FilmRepositoryService : MainRepository<Film>
	{
		public FilmRepositoryService(IRepository<Film> repository)
			: base(repository) { }
	}
}
