using ApplicationCore.Domain.Core.Models.Cinema;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions
{
	public class CategoryRepository : MainMongoRepository<Category>
	{
		public CategoryRepository(string connectionString)
			: base(connectionString, "categores")
		{
		}

		protected override Category Initialization(BsonDocument item)
			=> new Category()
			{
				Id = item.GetValue("_id").ToInt32(),
				Name = item.GetValue("name").ToString() as string ?? "Undefined",
				Price = item.GetValue("price").ToDecimal()
			};

		public override async Task<bool> InsertAsync(Category entity)
		{
			var parser = new MongoParser();
			entity.Id = parser.MaxIndex(_mongoCollection) + 1;

			var document = new BsonDocument
			{
				{ "_id", entity.Id },
				{"name", entity.Name },
				{"price",entity.Price }
			};

			await _mongoCollection.InsertOneAsync(document);

			return true;
		}

		public override async Task<bool> UpdateAsync(Category entity)
		{
			FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("name", entity.Name);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("price", entity.Price);
			await _mongoCollection.UpdateOneAsync(filter, update);

			return true;
		}
	}
}
