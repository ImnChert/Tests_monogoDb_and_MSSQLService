using ApplicationCore.Domain.Core.Models.Roles;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;

namespace Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions
{
	public class UserRepository : MainMongoRepository<RegisteredUser>
	{
		public UserRepository(string connectionString)
			: base(connectionString, "registeredUser")
		{
		}

		protected override RegisteredUser Initialization(BsonDocument item)
			=> new RegisteredUser()
			{
				Id = item.GetValue("_id").ToInt32(),
				Username = item.GetValue("username").ToString() as string ?? "Undefined",
				Password = item.GetValue("password").ToString() as string ?? "Undefined",
				FirstName = item.GetValue("firstName").ToString() as string ?? "Undefined",
				LastName = item.GetValue("lastName").ToString() as string ?? "Undefined",
				MiddleName = item.GetValue("middleName").ToString() as string ?? "Undefined",
				DateOfBirthday = DateTime.Parse(item.GetValue("dateOfBirth").ToString() as string ?? "Undefined",
					CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
				Phone = item.GetValue("phone").ToString() as string ?? "Undefined"
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
