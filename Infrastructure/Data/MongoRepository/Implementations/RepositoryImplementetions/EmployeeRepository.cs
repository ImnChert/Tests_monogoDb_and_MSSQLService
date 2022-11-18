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

		public override async Task<List<Employee>> GetAllAsync()
		{
			var filter = new BsonDocument();
			var employees = new List<Employee>();

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				var parse = new MongoParser();
				while (await cursor.MoveNextAsync())
				{
					IEnumerable<BsonDocument> user = cursor.Current;

					foreach (BsonDocument item in user)
					{
						employees.Add(InitializationEmployee(item, parse));
					}
				}
			}

			return employees;
		}

		public override async Task<Employee> GetById(int id)
		{
			var employee = new Employee();
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

					employee = InitializationEmployee(item, parse);
				}
			}

			return employee;
		}

		public Employee InitializationEmployee(BsonDocument item, MongoParser parse) => new Employee()
		{
			Id = item.GetValue("_id").ToInt32(),
			Username = item.GetValue("username").ToString(),
			Password = item.GetValue("password").ToString(),
			FirstName = item.GetValue("firstName").ToString(),
			MiddleName = item.GetValue("middleName").ToString(),
			LastName = item.GetValue("lastName").ToString(),
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
