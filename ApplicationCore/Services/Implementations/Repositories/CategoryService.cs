using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using ApplicationCore.Services.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class CategoryService : MainRepository<Category>
	{
		public CategoryService(IRepository<Category> categoryRepository)
			: base(categoryRepository) { }
	}
}
