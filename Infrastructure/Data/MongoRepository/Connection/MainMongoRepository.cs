using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Connection
{
    internal abstract class MainMongoRepository
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
    }
}
