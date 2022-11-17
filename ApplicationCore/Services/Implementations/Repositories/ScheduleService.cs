using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class ScheduleService : MainRepository<Schedule>
	{
		public ScheduleService(IRepository<Schedule> repository) : base(repository)
		{
		}
	}
}
