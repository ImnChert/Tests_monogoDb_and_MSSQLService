using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class CategoryService : MainRepository<Category>
	{
		public CategoryService(IRepository<Category> repository)
			: base(repository) { }
	}
}
