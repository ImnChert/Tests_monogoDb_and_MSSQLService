using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class CategoryRepositoryService : MainRepository<Category>
	{
		public CategoryRepositoryService(IRepository<Category> repository)
			: base(repository) { }
	}
}
