using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services.Implementations.Repositories
{
	internal class FilmService : MainRepository<Film>
	{
		public FilmService(IRepository<Film> repository)
			: base(repository) { }
	}
}
