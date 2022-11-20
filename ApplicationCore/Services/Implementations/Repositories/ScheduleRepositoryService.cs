using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class ScheduleRepositoryService : MainRepository<Schedule>
	{
		public ScheduleRepositoryService(IRepository<Schedule> repository) : base(repository)
		{
		}
	}
}
