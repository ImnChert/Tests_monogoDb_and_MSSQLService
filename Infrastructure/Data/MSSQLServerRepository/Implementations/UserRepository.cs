using ApplicationCore.Domain.Core.Models.Roles;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class UserRepository : MainMSSQLServerRepository<RegisteredUser>
    {
        protected override RegisteredUser GetReader(SqlDataReader sqlDataReader)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> InsertSqlCommand(SqlCommand sqlCommand, RegisteredUser entity)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, RegisteredUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
