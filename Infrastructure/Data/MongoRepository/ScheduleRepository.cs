using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace Infrastructure.Data.MongoRepository
{
	internal class ScheduleRepository : IRepository<Schedule>
	{
		public Task DeleteAsync(Schedule entity)
		{
			throw new NotImplementedException();
		}


		public Task<List<Schedule>> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task Insert(Schedule entity)
		{
			throw new NotImplementedException();
		}

		public Task Update(Schedule entity)
		{
			throw new NotImplementedException();
		}
	}
}
