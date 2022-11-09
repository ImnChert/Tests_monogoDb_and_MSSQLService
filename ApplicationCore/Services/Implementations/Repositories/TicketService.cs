using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;

namespace ApplicationCore.Services.Implementations.Repositories
{
	internal class TicketService : MainRepository<Ticket>
	{
		public TicketService(IRepository<Ticket> repository) : base(repository)
		{
		}
	}
}
