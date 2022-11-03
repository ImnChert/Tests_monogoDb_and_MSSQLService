using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Interfaces.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Connection
{
    internal abstract class MainMongoRepository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly IMongoCollection<BsonDocument> _mongoCollection;
        private readonly string _nameDatabase = "Cinema";

        public MainMongoRepository(string connectionString, string nameCollection)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(_nameDatabase);
            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>(nameCollection);
        }

        public abstract Task<List<T>> GetAllAsync();
        public abstract Task<T> GetById(int id);
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
