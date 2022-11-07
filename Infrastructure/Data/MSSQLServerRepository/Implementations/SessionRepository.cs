using ApplicationCore.Domain.Core.Models.Cinema;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class SessionRepository : MainMSSQLServerRepository<Session>
    {
        protected override Session GetReader(SqlDataReader sqlDataReader)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> InsertSqlCommand(SqlCommand sqlCommand, Session entity)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, Session entity)
        {
            throw new NotImplementedException();
        }
    }
}
