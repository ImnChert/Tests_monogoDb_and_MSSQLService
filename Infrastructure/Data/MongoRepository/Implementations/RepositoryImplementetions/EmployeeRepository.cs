using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using Infrastructure.Data.MongoRepository.Implementations.GetAllByIdImplementations;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions
{
	public class EmployeeRepository : MainMongoRepository<Employee>
	{
		private IGetAllById<Position> _positionsGetAllById;

		public EmployeeRepository(string connectionString, IGetAllById<Position> positionsGetAllById)
			: base(connectionString, "employees")
		{
			_positionsGetAllById = positionsGetAllById;
		}

		public EmployeeRepository(string connectionString)
			: base(connectionString, "employees")
		{
			_positionsGetAllById = new PositionGetAllById(_mongoCollection);
		}

		protected override Employee Initialization(BsonDocument item)
			=> new Employee()
			{
				Id = item.GetValue("_id").ToInt32(),
				Username = item.GetValue("username").ToString() as string ?? "Undefined",
				Password = item.GetValue("password").ToString() as string ?? "Undefined",
				FirstName = item.GetValue("firstName").ToString() as string ?? "Undefined",
				MiddleName = item.GetValue("middleName").ToString() as string ?? "Undefined",
				LastName = item.GetValue("lastName").ToString() as string ?? "Undefined",
				Positions = _positionsGetAllById.GetAllByIdOneToMany(item.GetValue("_id").ToInt32()).Result.ToList()
			};

		public override async Task<bool> InsertAsync(Employee entity)
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

			return true;
		}

		public override async Task<bool> UpdateAsync(Employee entity)
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

			return true;
		}
	}
}
