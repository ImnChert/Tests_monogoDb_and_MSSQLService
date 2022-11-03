using Infrastructure.Data.MongoRepository.Connection;

namespace Infrastructure.Data.MongoRepository
{
	internal class TicketRepository : MainMongoRepository
	{
		public TicketRepository(string connectionString) 
			: base(connectionString, "films")
		{
		}
	}
}
