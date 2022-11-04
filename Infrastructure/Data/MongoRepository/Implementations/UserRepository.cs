using ApplicationCore.Domain.Core.Models.Roles;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations
{
    internal class UserRepository : MainMongoRepository<RegisteredUser>
    {
        public UserRepository(string connectionString)
            : base(connectionString, "registeredUsers")
        {
        }

        public override Task<List<RegisteredUser>> GetAllAsync()
        {
            throw new NotImplementedException();
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

                    user.Id = item.GetValue("_id").ToInt32();
                    user.Username = item.GetValue("username").ToString();
                    user.Password = item.GetValue("password").ToString();
                    user.FirstName = item.GetValue("firstName").ToString();
                    user.LastName = item.GetValue("lastName").ToString();
                    user.MiddleName = item.GetValue("middleName").ToString();
                    user.DateOfBirthday = DateTime.Parse((string)item.GetValue("dateOfBirth"));
                    user.Phone = item.GetValue("phone").ToString();
                }
            }

            return user;
        }

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
