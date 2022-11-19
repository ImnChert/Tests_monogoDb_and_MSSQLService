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

		//public override async Task<List<Distributor>> GetAllAsync()
		//{
		//	var filter = new BsonDocument();
		//	var distributors = new List<Distributor>();

		//	using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
		//	{
		//		while (await cursor.MoveNextAsync())
		//		{
		//			IEnumerable<BsonDocument> filmsBson = cursor.Current;

		//			foreach (BsonDocument item in filmsBson)
		//			{
		//				distributors.Add(InitializationDistributor(item));
		//			}
		//		}
		//	}

		//	return distributors;
		//}

		//public override async Task<Distributor> GetById(int id)
		//{
		//	var distributor = new Distributor();
		//	var filter = new BsonDocument("_id", id);

		//	using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
		//	{
		//		if (await cursor.MoveNextAsync())
		//		{
		//			if (cursor.Current.Count() == 0)
		//				return null;

		//			var elements = cursor.Current.ToList();
		//			BsonDocument item = elements[0];

		//			distributor = InitializationDistributor(item);
		//		}
		//	}

		//	return distributor;
		//}

		protected override Distributor Initialization(BsonDocument item)
			=> new Distributor()
			{
				Id = item.GetValue("_id").ToInt32(),
				NameCompany = item.GetValue("nameCompany").ToString()
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
