using ApplicationCore.Domain.Core.Models.Cinema;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations
{
    internal class CategoryRepository : MainMongoRepository<Category>
    {
        public CategoryRepository(string connectionString)
            : base(connectionString, "categores")
        {
        }

        public override async Task<List<Category>> GetAllAsync()
        {
            var filter = new BsonDocument();
            var categories = new List<Category>();

            using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> user = cursor.Current;

                    foreach (BsonDocument item in user)
                    {
                        categories.Add(InitializationFilm(item));
                    }
                }
            }

            return categories;
        }

        public override async Task<Category> GetById(int id)
        {
            var category = new Category();
            var filter = new BsonDocument("_id", id);

            using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();
                    BsonDocument item = elements[0];

                    var parse = new MongoParser();

                    category = InitializationFilm(item);
				}
            }

            return category;
        }

		public Category InitializationFilm(BsonDocument item) => new Category()
		{
			Id = item.GetValue("_id").ToInt32(),
			Name = item.GetValue("username").ToString(),
			Price = item.GetValue("password").ToDecimal()
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
            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

            var update = Builders<BsonDocument>.Update.Set("name", entity.Name);
            await _mongoCollection.UpdateOneAsync(filter, update);

            update = Builders<BsonDocument>.Update.Set("price", entity.Price);
            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
    }
}
