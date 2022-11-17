using ApplicationCore.Domain.Core.Models.Roles;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;

namespace Infrastructure.Data.MongoRepository.Implementations
{
	public class UserRepository : MainMongoRepository<RegisteredUser>
	{
		public UserRepository(string connectionString)
			: base(connectionString, "registeredUser")
		{
		}

		public override async Task<List<RegisteredUser>> GetAllAsync()
		{
			var filter = new BsonDocument();
			var registeredUsers = new List<RegisteredUser>();

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				var parse = new MongoParser();
				while (await cursor.MoveNextAsync())
				{
					IEnumerable<BsonDocument> user = cursor.Current;

					foreach (BsonDocument item in user)
					{
						registeredUsers.Add(InitializationUser(item));
					}
				}
			}

			return registeredUsers;
		}

		public override async Task<RegisteredUser> GetById(int id)
		{
			var user = new RegisteredUser();
			var filter = new BsonDocument("_id", id);

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				if (await cursor.MoveNextAsync())
				{
					if (cursor.Current.Count() == 0)
						return null;

					var elements = cursor.Current.ToList();
					BsonDocument item = elements[0];

					user = InitializationUser(item);
				}
			}

			return user;
		}

		public RegisteredUser InitializationUser(BsonDocument item) => new RegisteredUser()
		{
			Id = item.GetValue("_id").ToInt32(),
			Username = item.GetValue("username").ToString(),
			Password = item.GetValue("password").ToString(),
			FirstName = item.GetValue("firstName").ToString(),
			LastName = item.GetValue("lastName").ToString(),
			MiddleName = item.GetValue("middleName").ToString(),
			DateOfBirthday = DateTime.Parse(item.GetValue("dateOfBirth").ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
			Phone = item.GetValue("phone").ToString()
		};

		public override async Task<bool> InsertAsync(RegisteredUser entity)
		{
			var parser = new MongoParser();
			entity.Id = parser.MaxIndex(_mongoCollection) + 1;

			var document = new BsonDocument
			{
				{ "_id", entity.Id },
				{"username", entity.Username },
				{"password",entity.Password },
				{"firstName",entity.FirstName },
				{"lastName",entity.LastName },
				{"middleName",entity.MiddleName },
				{"dateOfBirth",entity.DateOfBirthday },
				{"phone",entity.Phone }
			};

			await _mongoCollection.InsertOneAsync(document);

			return true;
		}

		public override async Task<bool> UpdateAsync(RegisteredUser entity)
		{
			var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			var update = Builders<BsonDocument>.Update.Set("username", entity.Username);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("password", entity.Password);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("firstName", entity.FirstName);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("lastName", entity.LastName);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("middleName", entity.MiddleName);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("dateOfBirth", entity.DateOfBirthday);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("phone", entity.Phone);
			await _mongoCollection.UpdateOneAsync(filter, update);

			return true;
		}
	}
}
