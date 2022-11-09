using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	internal class ScheduleService : MainRepository<Schedule>
	{
		public ScheduleService(IRepository<Schedule> repository) : base(repository)
		{
		}
	}
}
