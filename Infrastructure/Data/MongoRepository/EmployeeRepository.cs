using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.Metrics;

namespace Infrastructure.Data.MongoRepository
{
	internal class EmployeeRepository : MainMongoRepository, IRepository<Employee>
	{
		public EmployeeRepository(string connectionString) : base(connectionString, "employees")
		{
		}

		public async Task<List<Employee>> GetAllAsync()
		{
			var filter = new BsonDocument();
			var employees = new List<Employee>();

			using (var cursor = await _mongoCollection.FindAsync(filter))
			{
				var parse = new MongoParser();
				while (await cursor.MoveNextAsync())
				{
					var user = cursor.Current;

					foreach (var item in user)
					{
						employees.Add(new Employee()
						{
							Id = item.GetValue("_id").ToInt32(),
							Username = item.GetValue("username").ToString(),
							Password = item.GetValue("password").ToString(),
							FirstName = item.GetValue("firstName").ToString(),
							MiddleName = item.GetValue("middleName").ToString(),
							LastName = item.GetValue("lastName").ToString(),
							Positions = parse.ParsePositions(item.GetValue("posts"))
						});
					}
				}
			}

			return employees;
		}

		public async Task<Employee> GetById(int id)
		{
			var employee = new Employee();
			var filter = new BsonDocument("_id", id);

			using (var cursor = await _mongoCollection.FindAsync(filter))
			{
				if (await cursor.MoveNextAsync())
				{
					if (cursor.Current.Count() == 0)
						return null;

					var elements = cursor.Current.ToList();
					var item = elements[0];

					var parse = new MongoParser();

					employee.Id = item.GetValue("_id").ToInt32();
					employee.Username = item.GetValue("username").ToString();
					employee.Password = item.GetValue("password").ToString();
					employee.FirstName = item.GetValue("firstName").ToString();
					employee.MiddleName = item.GetValue("middleName").ToString();
					employee.LastName = item.GetValue("lastName").ToString();
					employee.Positions = parse.ParsePositions(item.GetValue("posts"));
				}
			}

			return employee;
		}

		public async Task InsertAsync(Employee entity)
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

		public async Task UpdateAsync(Employee entity)
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

		public async Task DeleteAsync(Employee entity)
		{
			var deleteFilter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			await _mongoCollection.DeleteOneAsync(deleteFilter);
		}
	}
}
