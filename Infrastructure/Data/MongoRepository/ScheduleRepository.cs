using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MongoRepository.Connection;

namespace Infrastructure.Data.MongoRepository
{
	internal class ScheduleRepository : MainMongoRepository, IRepository<Schedule>
	{
		public ScheduleRepository(string connectionString) 
			: base(connectionString, "schedule")
		{
		}

		public Task DeleteAsync(Schedule entity) => throw new NotImplementedException();


		public Task<List<Schedule>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Schedule> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public Task InsertAsync(Schedule entity)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Schedule entity)
		{
			throw new NotImplementedException();
		}
	}
}
