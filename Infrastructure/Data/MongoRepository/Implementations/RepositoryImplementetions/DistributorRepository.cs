using ApplicationCore.Domain.Core.Models.Cinema.Films;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions
{
	internal class DistributorRepository : MainMongoRepository<Distributor>
	{
		public DistributorRepository(string connectionString, string nameCollection)
			: base(connectionString, nameCollection)
		{
		}

		protected override Distributor Initialization(BsonDocument item)
			=> new Distributor()
			{
				Id = item.GetValue("_id").ToInt32(),
				NameCompany = item.GetValue("nameCompany").ToString() as string ?? "Undefined"
			};

		public override async Task<bool> InsertAsync(Distributor entity)
		{
			var parser = new MongoParser();
			entity.Id = parser.MaxIndex(_mongoCollection) + 1;

			var document = new BsonDocument
			{
				{ "_id", entity.Id },
				{"nameCompany",entity.NameCompany }
			};

			await _mongoCollection.InsertOneAsync(document);

			return true;
		}

		public override async Task<bool> UpdateAsync(Distributor entity)
		{
			var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			var update = Builders<BsonDocument>.Update.Set("nameCompany", entity.NameCompany);
			await _mongoCollection.UpdateOneAsync(filter, update);

			return true;
		}
	}
}
