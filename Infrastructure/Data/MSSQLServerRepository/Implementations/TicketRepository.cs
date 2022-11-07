using ApplicationCore.Domain.Core.Models.Cinema;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class TicketRepository : MainMSSQLServerRepository<Ticket>
    {
        protected override Ticket GetReader(SqlDataReader sqlDataReader)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> InsertSqlCommand(SqlCommand sqlCommand, Ticket entity)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
