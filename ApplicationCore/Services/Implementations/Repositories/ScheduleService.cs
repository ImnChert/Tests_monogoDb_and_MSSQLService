using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services.Implementations.Repositories
{
	internal class ScheduleService : MainRepository<Schedule>
	{
		public ScheduleService(IRepository<Schedule> repository) : base(repository)
		{
		}
	}
}
