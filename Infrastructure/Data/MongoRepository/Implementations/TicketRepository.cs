using ApplicationCore.Domain.Core.Models.Cinema;
using Infrastructure.Data.MongoRepository.Connection;

namespace Infrastructure.Data.MongoRepository.Implementations
{
    internal class TicketRepository : MainMongoRepository<Ticket>
    {
        public TicketRepository(string connectionString)
            : base(connectionString, "films")
        {
        }

        public override Task<List<Ticket>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<Ticket> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> InsertAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
