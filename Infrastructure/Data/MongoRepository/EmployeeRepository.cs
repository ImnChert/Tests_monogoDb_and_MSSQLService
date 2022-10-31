using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Business;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository
{
	internal class EmployeeRepository : MainMongoRepository, IRepository<Employee>
	{
		public EmployeeRepository(string connectionString) : base(connectionString, "employees")
		{
		}

		public async Task DeleteAsync(Employee entity)
		{
			var deleteFilter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			await _mongoCollection.DeleteOneAsync(deleteFilter);
		}

		public Task<List<Employee>> GetAll()
		{
			throw new NotImplementedException();
		}

		//public async Task<List<Employee>> GetAllAsync()
		//{
		//	var filter = new BsonDocument();
		//	var countries = new List<Employee>();

		//	var pipeline = new BsonDocument
		//	{
		//		{"$unwind", "$posts"}
		//	};

		//	var pipeline2 = new BsonDocument
		//	{
		//		{ "$project", new BsonDocument
		//			{
		//			{ "_id", "$_id"},
		//			{ "name", "$posts.name"},
		//			{ "post_id", "$posts._id"}
		//		} }
		//	};
		//	BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2 };
		//	List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

		//	List<Position> cities = new();

		//	//foreach (BsonDocument item in results)
		//	//{
		//	//	cities.Add(new Employee()
		//	//	{
		//	//		Name = item.GetValue("CityName").ToString(),
		//	//		Id = item.GetValue("CityId").ToInt32(),
		//	//		CountryId = item.GetValue("_id").ToInt32()
		//	//	});
		//	//}

		//	//return cities;

		//	using (var cursor = await _mongoCollection.FindAsync(filter))
		//	{
		//		while (await cursor.MoveNextAsync())
		//		{
		//			var user = cursor.Current;

		//			foreach (var item in user)
		//			{
					 
		//				countries.Add(new Employee()
		//				{
		//					Id = item.GetValue("_id").ToInt32(),
		//					Username = item.GetValue("Name").ToString(),
		//					Password = item.GetValue("Name").ToString(),
		//					FirstName = item.GetValue("Name").ToString(),
		//					MiddleName = item.GetValue("Name").ToString(),
		//					LastName = item.GetValue("Name").ToString(),
		//					//Positions = item.GetElement("Name")
		//				});
		//			}
		//		}
		//	}

		//	return countries;

		//}

		public async Task Insert(Employee entity)
		{
			var parser = new MongoParser();
			entity.Id = parser.MaxIndex(_mongoCollection) + 1;

			var arr = new BsonDocument();

			entity.Positions.ForEach(item =>
			{
				arr.AddRange(new BsonDocument
				{
					{"post_id", item.Id},
					{"postName", item.ToString()},
				});
			});

			var document = new BsonDocument
			{
				{ "_id", entity.Id },
				{"username",entity.Username },
				{"password",entity.Password },
				{"firstName",entity.FirstName },
				{"middleName",entity.MiddleName },
				{"lastName",entity.LastName },
				{"posts",  arr},
			};

			await _mongoCollection.InsertOneAsync(document);
		}

		public async Task Update(Employee entity)
		{
			var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			var update = Builders<BsonDocument>.Update.Set("username", entity.Username);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("password", entity.Password);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("firstName", entity.FirstName);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("middleName", entity.MiddleName);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("lastName", entity.LastName);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("posts", entity.Positions);
			await _mongoCollection.UpdateOneAsync(filter, update);
		}
	}
}
