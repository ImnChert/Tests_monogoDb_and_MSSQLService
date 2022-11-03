using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MongoRepository.Connection;

namespace Infrastructure.Data.MongoRepository
{
	internal class ScheduleRepository : MainMongoRepository<Schedule>
	{
		public ScheduleRepository(string connectionString) 
			: base(connectionString, "schedule")
		{
		}

		public override Task<List<Schedule>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public override Task<Schedule> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public override Task<bool> InsertAsync(Schedule entity)
		{
			throw new NotImplementedException();
		}

		public override Task<bool> UpdateAsync(Schedule entity)
		{
			throw new NotImplementedException();
		}
	}
}
