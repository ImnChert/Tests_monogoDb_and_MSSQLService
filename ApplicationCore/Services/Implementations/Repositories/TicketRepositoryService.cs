using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class TicketRepositoryService : MainRepository<Ticket>
	{
		public TicketRepositoryService(IRepository<Ticket> repository) : base(repository)
		{
		}
	}
}
