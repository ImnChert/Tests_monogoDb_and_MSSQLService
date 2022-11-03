using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository
{
	internal class CategoryRepository : MainMongoRepository<Category>
	{
		public CategoryRepository(string connectionString) 
			: base(connectionString, "categores")
		{
		}

		public override Task<List<Category>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public override Task<Category> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public override async Task<bool> InsertAsync(Category entity)
		{

			return true;
		}

		public override Task<bool> UpdateAsync(Category entity)
		{
			throw new NotImplementedException();
		}
	}
}
