using ApplicationCore.Domain.Core.Models.Roles;
using Infrastructure.Data.MongoRepository.Connection;

namespace Infrastructure.Data.MongoRepository
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

		public override Task<RegisteredUser> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public override Task<bool> InsertAsync(RegisteredUser entity)
		{
			throw new NotImplementedException();
		}

		public override Task<bool> UpdateAsync(RegisteredUser entity)
		{
			throw new NotImplementedException();
		}
	}
}
