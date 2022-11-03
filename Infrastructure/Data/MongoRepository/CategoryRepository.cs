using Infrastructure.Data.MongoRepository.Connection;

namespace Infrastructure.Data.MongoRepository
{
	internal class CategoryRepository : MainMongoRepository
	{
		public CategoryRepository(string connectionString) 
			: base(connectionString, "categores")
		{
		}
	}
}
