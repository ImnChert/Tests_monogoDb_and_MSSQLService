using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Interfaces.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Connection
{
	public abstract class MainMongoRepository<T>
		: IRepository<T> where T : EntityBase
	{
		protected readonly IMongoCollection<BsonDocument> _mongoCollection;
		private readonly string _nameDatabase = "Cinema";

		public MainMongoRepository(string connectionString, string nameCollection)
		{
			if (connectionString == null)
				throw new ArgumentNullException(nameof(connectionString));

			var mongoClient = new MongoClient(connectionString);
			IMongoDatabase mongoDatabase = mongoClient.GetDatabase(_nameDatabase);
			_mongoCollection = mongoDatabase.GetCollection<BsonDocument>(nameCollection);
		}

		public async Task<List<T>> GetAllAsync()
		{
			var filter = new BsonDocument();
			var collection = new List<T>();

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				while (await cursor.MoveNextAsync())
				{
					IEnumerable<BsonDocument> filmsBson = cursor.Current;

					foreach (BsonDocument item in filmsBson)
					{
						collection.Add(Initialization(item));
					}
				}
			}

			return collection;
		}

		public async Task<T?> GetById(int id)
		{
			var filter = new BsonDocument("_id", id);

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				if (await cursor.MoveNextAsync())
				{
					if (cursor.Current.Count() == 0)
						return null;

					var elements = cursor.Current.ToList();
					BsonDocument item = elements[0];

					return Initialization(item);
				}
			}

			return null;
		}

		protected abstract T Initialization(BsonDocument item);

		public abstract Task<bool> InsertAsync(T entity);

		public abstract Task<bool> UpdateAsync(T entity);

		public async Task<bool> DeleteAsync(T entity)
		{
			var deleteFilter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			await _mongoCollection.DeleteOneAsync(deleteFilter);

			return true;
		}
	}
}
