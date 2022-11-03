using Infrastructure.Data.MongoRepository.Connection;

namespace Infrastructure.Data.MongoRepository
{
	internal class UserRepository : MainMongoRepository
	{
		public UserRepository(string connectionString) 
			: base(connectionString, "registeredUsers")
		{
		}
	}
}
