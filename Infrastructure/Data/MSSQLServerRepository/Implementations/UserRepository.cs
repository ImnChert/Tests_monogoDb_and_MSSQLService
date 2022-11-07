using ApplicationCore.Domain.Core.Models.Roles;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
    internal class UserRepository : MainMSSQLServerRepository<RegisteredUser>
    {
        public UserRepository(string connectionString, string tableName, string insertQuery, string updateQuery, string getAllQuery, string getByIdQuery) 
            : base(connectionString, 
                  tableName, 
                  insertQuery, 
                  updateQuery, 
                  getAllQuery, 
                  getByIdQuery)
        {
        }

        protected override RegisteredUser GetReader(SqlDataReader sqlDataReader)
        {
            throw new NotImplementedException();
        }

        protected override void InsertCommand(SqlCommand sqlCommand, RegisteredUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
